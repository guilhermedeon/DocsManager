using DocsManager.Core.Abstractions;
using DocsManager.Core.Application;
using DocsManager.Core.Domain.Documents;
using DocsManager.Core.Domain.Documents.DTO;
using DocsManager.Core.Domain.Documents.Revisions;
using Moq;

namespace DocsManager.Tests
{
    public class DocumentServiceTests
    {
        private readonly Mock<IDocumentRepository> _documentRepositoryMock;
        private readonly DocumentService _documentService;

        public DocumentServiceTests()
        {
            _documentRepositoryMock = new Mock<IDocumentRepository>();
            _documentService = new DocumentService(_documentRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateDocumentAsync_ShouldAddDocumentToRepository()
        {
            var createDto = new CreateDocumentDTO
            {
                Code = "DOC001",
                Title = "Test Document",
                Revision = Revision.A,
                PlannedDate = DateTime.UtcNow,
                Value = 100.50m
            };

            var result = await _documentService.CreateDocumentAsync(createDto);

            _documentRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Document>(d =>
                d.Code == createDto.Code &&
                d.Title == createDto.Title &&
                d.Revision == createDto.Revision &&
                d.PlannedDate == createDto.PlannedDate &&
                d.Value == createDto.Value
            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(createDto.Code, result.Code);
            Assert.Equal(createDto.Title, result.Title);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ShouldUpdateExistingDocument()
        {
            var existingDocument = new Document
            {
                Guid = Guid.NewGuid(),
                Code = "DOC001",
                Title = "Old Title",
                Revision = Revision.A,
                PlannedDate = DateTime.UtcNow,
                Value = 50.00m
            };

            var updateDto = new UpdateDocumentDTO
            {
                Guid = existingDocument.Guid,
                Code = "DOC002",
                Title = "Updated Title",
                Revision = Revision.B,
                PlannedDate = DateTime.UtcNow.AddDays(1),
                Value = 150.00m
            };

            _documentRepositoryMock.Setup(repo => repo.GetByIdAsync(existingDocument.Guid))
                .ReturnsAsync(existingDocument);

            var result = await _documentService.UpdateDocumentAsync(updateDto);

            _documentRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Document>(d =>
                d.Guid == updateDto.Guid &&
                d.Code == updateDto.Code &&
                d.Title == updateDto.Title &&
                d.Revision == updateDto.Revision &&
                d.PlannedDate == updateDto.PlannedDate &&
                d.Value == updateDto.Value
            )), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(updateDto.Code, result.Code);
            Assert.Equal(updateDto.Title, result.Title);
        }

        [Fact]
        public async Task UpdateDocumentAsync_ShouldThrowExceptionIfDocumentNotFound()
        {
            var updateDto = new UpdateDocumentDTO
            {
                Guid = Guid.NewGuid(),
                Code = "DOC002",
                Title = "Updated Title",
                Revision = Revision.B,
                PlannedDate = DateTime.UtcNow.AddDays(1),
                Value = 150.00m
            };

            _documentRepositoryMock.Setup(repo => repo.GetByIdAsync(updateDto.Guid))
                .ReturnsAsync((Document?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _documentService.UpdateDocumentAsync(updateDto));
        }

        [Fact]
        public void GetDocuments_ShouldReturnFilteredAndPaginatedDocuments()
        {
            var documents = new List<Document>
        {
            new() { Guid = Guid.NewGuid(), Code = "DOC001", Title = "Title1", Revision = Revision.A, Value = 100 },
            new() { Guid = Guid.NewGuid(), Code = "DOC002", Title = "Title2", Revision = Revision.B, Value = 200 },
            new() { Guid = Guid.NewGuid(), Code = "DOC003", Title = "Title3", Revision = Revision.C, Value = 300 }
        }.AsQueryable();

            var filter = new DocumentListFilterDTO
            {
                SearchText = "Title",
                SortColumn = "Code",
                SortDescending = false,
                PageNumber = 1,
                PageSize = 2
            };

            _documentRepositoryMock.Setup(repo => repo.Query()).Returns(documents);

            var (result, totalCount) = _documentService.GetDocuments(filter);

            Assert.Equal(3, totalCount); // Total documents matching the search
            Assert.Equal(2, result.Count()); // Page size
            Assert.Equal("DOC001", result.First().Code); // Sorted by Code
        }

        [Fact]
        public void GetDocuments_ShouldReturnAllDocumentsIfNoFilter()
        {
            var documents = new List<Document>
        {
            new() { Guid = Guid.NewGuid(), Code = "DOC001", Title = "Title1", Revision = Revision.A, Value = 100 },
            new() { Guid = Guid.NewGuid(), Code = "DOC002", Title = "Title2", Revision = Revision.B, Value = 200 },
            new() { Guid = Guid.NewGuid(), Code = "DOC003", Title = "Title3", Revision = Revision.C, Value = 300 }
        }.AsQueryable();
            var filter = new DocumentListFilterDTO
            {
                SearchText = "",
                SortColumn = "",
                SortDescending = false,
                PageNumber = 1,
                PageSize = 10
            };
            _documentRepositoryMock.Setup(repo => repo.Query()).Returns(documents);
            var (result, totalCount) = _documentService.GetDocuments(filter);
            Assert.Equal(3, totalCount); // Total documents
            Assert.Equal(3, result.Count()); // All documents
        }

        [Fact]
        public void GetDocuments_ShouldReturnEmptyIfNoDocuments()
        {
            var documents = new List<Document>().AsQueryable();
            var filter = new DocumentListFilterDTO
            {
                SearchText = "NonExistent",
                SortColumn = "Title",
                SortDescending = false,
                PageNumber = 1,
                PageSize = 10
            };
            _documentRepositoryMock.Setup(repo => repo.Query()).Returns(documents);
            var (result, totalCount) = _documentService.GetDocuments(filter);
            Assert.Equal(0, totalCount); // No documents matching the search
            Assert.Empty(result); // No documents returned
        }

        [Fact]
        public void GetDocuments_ShouldReturnSortedDocuments()
        {
            var documents = new List<Document>
        {
            new() { Guid = Guid.NewGuid(), Code = "DOC002", Title = "Title2", Revision = Revision.B, Value = 200 },
            new() { Guid = Guid.NewGuid(), Code = "DOC001", Title = "Title1", Revision = Revision.A, Value = 100 },
            new() { Guid = Guid.NewGuid(), Code = "DOC003", Title = "Title3", Revision = Revision.C, Value = 300 }
        }.AsQueryable();
            var filter = new DocumentListFilterDTO
            {
                SearchText = "",
                SortColumn = "Code",
                SortDescending = true,
                PageNumber = 1,
                PageSize = 10
            };
            _documentRepositoryMock.Setup(repo => repo.Query()).Returns(documents);
            var (result, totalCount) = _documentService.GetDocuments(filter);
            Assert.Equal(3, totalCount); // Total documents
            Assert.Equal("DOC003", result.First().Code); // Sorted by Code descending
        }
    }
}
