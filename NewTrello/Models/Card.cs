using System.Text.Json.Serialization;

namespace NewTrello.Models;

public class Card
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }

    public Guid ColumnId { get; set; }
    public required Column Column { get; set; }

    public Guid? AssignedUserId { get; set; }
    public User? AssignedUser { get; set; }
}
