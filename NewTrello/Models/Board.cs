using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewTrello.Models;

[Table("Boards")]
public class Board
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public Guid OwnerId { get; set; }
    public User? Owner { get; set; }

    [JsonIgnore]
    public ICollection<Column>? Columns { get; set; }
}
