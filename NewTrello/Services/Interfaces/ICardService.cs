using NewTrello.Dtos;

namespace NewTrello.Services.Interfaces;

public interface ICardService
{
    Task<IEnumerable<CardResponseDTO>> GetAllCardsAsync();
    Task<CardResponseDTO?> GetCardByIdAsync(Guid id);
    Task<IEnumerable<CardResponseDTO?>> GetCardsByColumn(Guid columnId);
    Task<IEnumerable<CardResponseDTO?>> GetCardsByUser(Guid userId);
    Task<CardResponseDTO?> CreateCardAsync(CardCreateRequestDTO dto);
    Task<ServiceResult<CardResponseDTO?>> UpdateCardAsync(Guid id, CardUpdateRequestDTO dto);
    Task DeleteCardAsync(Guid id);
}
