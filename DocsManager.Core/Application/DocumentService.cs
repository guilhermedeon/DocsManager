using DocsManager.Core.Abstractions;
using DocsManager.Core.Domain.Documents;
using DocsManager.Core.Domain.Documents.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsManager.Core.Application
{
    public class DocumentService(IDocumentRepository documentRepository)
    {
        public async Task<Document> CreateDocumentAsync(CreateDocumentDTO createDocumentDTO)
        {
            var document = new Document
            {
                Guid = Guid.NewGuid(),
                Code = createDocumentDTO.Code,
                Title = createDocumentDTO.Title,
                Revision = createDocumentDTO.Revision,
                PlannedDate = createDocumentDTO.PlannedDate,
                Value = createDocumentDTO.Value
            };

            await documentRepository.AddAsync(document);

            return document;
        }
        public async Task<Document> UpdateDocumentAsync(UpdateDocumentDTO updateDocumentDTO)
        {
            var document = await documentRepository.GetByIdAsync(updateDocumentDTO.Guid) ?? throw new KeyNotFoundException("Document not found");

            document.Code = updateDocumentDTO.Code;
            document.Title = updateDocumentDTO.Title;
            document.Revision = updateDocumentDTO.Revision;
            document.PlannedDate = updateDocumentDTO.PlannedDate;
            document.Value = updateDocumentDTO.Value;

            await documentRepository.UpdateAsync(document);

            return document;
        }

        public (IEnumerable<Document> Documents, int TotalCount) GetDocuments(DocumentListFilterDTO filter)
        {
             var query = documentRepository.Query();

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(d => d.Title.Contains(filter.SearchText) || d.Code.Contains(filter.SearchText));
            }

            if (!string.IsNullOrWhiteSpace(filter.SortColumn))
            {
                query = filter.SortColumn.ToLower() switch
                {
                    "title" => filter.SortDescending ? query.OrderByDescending(d => d.Title) : query.OrderBy(d => d.Title),
                    "code" => filter.SortDescending ? query.OrderByDescending(d => d.Code) : query.OrderBy(d => d.Code),
                    "revision" => filter.SortDescending ? query.OrderByDescending(d => d.Revision) : query.OrderBy(d => d.Revision),
                    "planneddate" => filter.SortDescending ? query.OrderByDescending(d => d.PlannedDate) : query.OrderBy(d => d.PlannedDate),
                    "value" => filter.SortDescending ? query.OrderByDescending(d => d.Value) : query.OrderBy(d => d.Value),
                    _ => query
                };
            }

            var totalCount = query.Count();

            query = query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

            var documents = query.ToList();
            return (documents, totalCount);
        }

        public IEnumerable<Document> GetAllDocuments()
        {
            return documentRepository.Query().ToList();
        }

        public async Task<Document?> GetDocumentByIdAsync(Guid id)
        {
            return await documentRepository.GetByIdAsync(id);
        }

        public async Task DeleteDocumentAsync(Guid id)
        {
            var document = await documentRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Document not found");
            await documentRepository.DeleteAsync(document);
        }
    }
}
