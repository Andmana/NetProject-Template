namespace NetProject.ViewModels
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Entity { get; set; }
    }
}
