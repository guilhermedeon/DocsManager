using DocsManager.Core.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
