namespace Domain.Errors
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = true;
        public Error Error { get; set; }
    }
}