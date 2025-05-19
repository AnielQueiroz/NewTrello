using System.ComponentModel.DataAnnotations;

namespace NewTrello.Dtos;

public class ColumnResponseDTO
{
    public Guid Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    public Guid BoardId { get; set; }

    public List<CardResponseDTO>? Cards { get; set; }
}

public class ColumnCreateRequestDTO
{
    [Required]
    public required string Name { get; set; }
    public Guid BoardId { get; set; }
}

public class ColumnUpdateRequestDTO
{
    [Required]
    public required string Name { get; set; }
}