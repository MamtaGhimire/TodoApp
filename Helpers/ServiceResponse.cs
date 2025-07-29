namespace TodoApp.Helpers
{

    public class ServiceResponse<T>
    {
        public T? Data { get; set; }             // actual result data
        public bool IsSuccess { get; set; } = true;  // success flag
        public string? ErrorMessage { get; set; } 
        public string Message { get; set; } = string.Empty; // additional message
    }
}
