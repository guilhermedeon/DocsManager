using System.ComponentModel.DataAnnotations;
using DocsManager.Core.Domain.Documents.Revisions;

namespace DocsManager.Core.Domain.Documents.DTO
{
    public class CreateDocumentDTO
    {
        public required string Code { get; set; }

        public required string Title { get; set; }

        public required Revision Revision { get; set; }

        public DateTime? PlannedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Value { get; set; }
    }
}
