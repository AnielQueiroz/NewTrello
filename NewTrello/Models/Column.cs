using System.Text.Json.Serialization;

namespace NewTrello.Models;

public class Column
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public Guid BoardId { get; set; }
    public Board? Board { get; set; }

    [JsonIgnore]
    public ICollection<Card>? Cards { get; set; }
}
