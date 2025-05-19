using System.ComponentModel.DataAnnotations;

namespace NewTrello.Dtos;

public class BoardResponseDTO
{
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }

    public List<ColumnResponseDTO>? Columns { get; set; }
}

public class BoardUpdateDTO
{
    [Required]
    public required string Name { get; set; }
}

public class BoardCreateDTO
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public Guid OwnerId { get; set; }
}
