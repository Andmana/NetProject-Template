namespace NetProject.ViewModels
{
    public class SearchQuery
    {
        public string? sortOrder { get; set; }
        public string? searchTerm { get; set; }
        public int? pageNumber { get; set; }
        public int? showData { get; set; }

        public string generateQuery()
        {
            var queries = new List<string>();

            if (!string.IsNullOrEmpty(searchTerm))
                queries.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");

            if (!string.IsNullOrEmpty(sortOrder))
                queries.Add($"sortOrder={Uri.EscapeDataString(sortOrder)}");

            if (pageNumber.HasValue)
                queries.Add($"pageNumber={pageNumber.Value}");

            if (showData.HasValue)
                queries.Add($"showData={showData.Value}");

            string query = string.Join("&", queries);
            return query;
        }
    }
}
