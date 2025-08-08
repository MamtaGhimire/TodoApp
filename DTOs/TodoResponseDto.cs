namespace TodoApp.DTOs
{
    public class TodoResponseDto
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required DateTime? DueDate { get; set; }
        public required string? UserId { get; set; }
    }
}
