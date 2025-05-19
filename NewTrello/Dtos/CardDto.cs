using System.ComponentModel.DataAnnotations;

namespace NewTrello.Dtos;

public class CardResponseDTO
{
    public Guid Id { get; set; }

    [Required]
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }

    public Guid? UserId { get; set; }
    public Guid ColumnId { get; set; }
}

public class CardCreateRequestDTO
{
    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }

    [Required]
    public Guid ColumnId { get; set; }

    public Guid? AssignedUserId { get; set; }
}

public class CardUpdateRequestDTO
{
    [StringLength(100)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Status { get; set; }

    public Guid? ColumnId { get; set; }

    public Guid? AssignedUserId { get; set; }
}

