using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NewTrello.Models;

[Table("User")]
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }

    [Required]
    [StringLength(100)]
    public string? Password { get; set; }

    [JsonIgnore]
    public ICollection<Board>? Boards { get; set; }
    public ICollection<Card>? AssignedCards { get; set; }
}
