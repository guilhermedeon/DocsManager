namespace DocsManager.Core.Domain.Documents.DTO
{
    public class DocumentListFilterDTO
    {
        public string? SearchText { get; set; }
        public string? SortColumn { get; set; }
        public bool SortDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
