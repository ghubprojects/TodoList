#nullable disable

namespace TodoList.Common.Models;

public partial class TodoItem {
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
