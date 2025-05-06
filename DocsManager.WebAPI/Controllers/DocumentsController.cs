using DocsManager.Core.Application;
using DocsManager.Core.Domain.Documents;
using DocsManager.Core.Domain.Documents.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocsManager.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentsController(DocumentService documentService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDocuments([FromQuery] DocumentListFilterDTO filter)
        {
            var (documents, totalCount) = documentService.GetDocuments(filter);
            return Ok(new { Documents = documents, TotalCount = totalCount });
        }

        [HttpGet("all")]
        [Produces<IEnumerable<Document>>]
        public IActionResult GetAllDocuments()
        {
            var documents = documentService.GetAllDocuments();
            return Ok(documents);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            try
            {
                var document = await documentService.GetDocumentByIdAsync(id);
                if (document == null)
                {
                    return NotFound(new { Message = "Document not found" });
                }
                return Ok(document);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDTO createDocumentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var document = await documentService.CreateDocumentAsync(createDocumentDTO);
                return CreatedAtAction(nameof(CreateDocument), new { id = document.Guid }, document);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDocument(Guid id, [FromBody] UpdateDocumentDTO updateDocumentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != updateDocumentDTO.Guid)
                return BadRequest(new { Message = "ID mismatch" });

            try
            {
                var updatedDocument = await documentService.UpdateDocumentAsync(updateDocumentDTO);
                return Ok(updatedDocument);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            try
            {
                await documentService.DeleteDocumentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
