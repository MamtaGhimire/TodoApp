namespace TodoApp.DTOs
{
    public class CreateTodoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Default status
        public DateTime DueDate { get; set; } = DateTime.Now;
    }
}