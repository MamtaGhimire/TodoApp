namespace TodoApp.Helpers
{

    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
