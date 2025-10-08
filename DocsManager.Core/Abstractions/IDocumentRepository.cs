using DocsManager.Core.Domain.Documents;

namespace DocsManager.Core.Abstractions
{
    public interface IDocumentRepository
    {
        IQueryable<Document> Query();
        Task AddAsync(Document document);
        Task<Document?> GetByIdAsync(Guid id);
        Task UpdateAsync(Document document);
        Task DeleteAsync(Document document);
    }
}
