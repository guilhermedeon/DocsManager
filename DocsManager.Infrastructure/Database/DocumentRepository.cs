using DocsManager.Core.Abstractions;
using DocsManager.Core.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsManager.Infrastructure.Database
{
    public class DocumentRepository(DocsContext context) : IDocumentRepository
    {
        public IQueryable<Document> Query()
        {
            return context.Documents.AsQueryable();
        }

        public async Task AddAsync(Document document)
        {
            await context.Documents.AddAsync(document);
            await context.SaveChangesAsync();
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await context.Documents.FindAsync(id);
        }

        public async Task UpdateAsync(Document document)
        {
            context.Documents.Update(document);
            await context.SaveChangesAsync();
        }

        public Task DeleteAsync(Document document)
        {
            context.Documents.Remove(document);
            return context.SaveChangesAsync();
        }
    }
}
