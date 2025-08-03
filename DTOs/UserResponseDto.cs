namespace TodoApp.DTOs
{
    public class UserResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public object? Data { get; set; } = null;
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        

    }
}
