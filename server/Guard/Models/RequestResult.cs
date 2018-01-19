namespace Guard.Models
{
    public enum RequestResultStatus
    {
        Success,
        Warning,
        Error
    }

    public class RequestResult
    {
        public RequestResultStatus Status { get; set; }
        public string Message { get; set; }
    }
}
