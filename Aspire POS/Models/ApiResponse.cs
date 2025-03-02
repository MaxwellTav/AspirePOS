namespace Aspire_POS.Models
{
    public class ApiResponse
    {
        public string Data { get; set; }
        public string ErrorMessage { get; set; }
        public ApiResponseStatus Status { get; set; }
    }

    public enum ApiResponseStatus
    {
        Ok,
        NotFound,
        Error,
        Unauthorized
    }
}
