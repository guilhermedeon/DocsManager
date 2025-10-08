using System.ComponentModel.DataAnnotations;
using DocsManager.Core.Domain.Documents.Revisions;

namespace DocsManager.Core.Domain.Documents
{
    /*
        Código: Campo de texto (obrigatório e único)
        Título: Campo de texto (obrigatório)
        Revisão: Dropdown com valores fixos: 0, A, B, C, D, E, F, G
        Data Planejada: Campo de data
        Valor: Campo monetário (validado)
    */

    public class Document
    {
        public required Guid Guid { get; set; }

        public required string Code { get; set; }

        public required string Title { get; set; }

        public required Revision Revision { get; set; }

        public DateTime? PlannedDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Value { get; set; }
    }
}
