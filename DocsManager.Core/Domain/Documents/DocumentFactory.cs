using DocsManager.Core.Domain.Documents.Revisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocsManager.Core.Domain.Documents
{
    public static class DocumentFactory
    {
        public static Document Create(string code, string title, Revision revision, DateTime? plannedDate = null, decimal? value = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code is required");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            if (value is not null && value < 0)
                throw new ArgumentException("Value must be greater than or equal to 0");

            return new Document
            {
                Guid = Guid.NewGuid(),
                Code = code.Trim(),
                Title = title.Trim(),
                Revision = revision,
                PlannedDate = plannedDate,
                Value = value
            };
        }
    }
}
