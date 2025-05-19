using NewTrello.Dtos;

namespace NewTrello.Services.Interfaces;

public interface IColumnService
{
    Task<IEnumerable<ColumnResponseDTO>> GetAllColumnsAsync();
    Task<ColumnResponseDTO?> GetColumnByIdAsync(Guid id);
    Task<IEnumerable<ColumnResponseDTO?>> GetColumnsByBoard(Guid boardId);
    Task<ColumnResponseDTO?> CreateColumnAsync(ColumnCreateRequestDTO dto);
    Task<ColumnResponseDTO?> UpdateColumnAsync(Guid id, ColumnUpdateRequestDTO dto);
    Task<bool> CheckIfColumnNameAlreadyUsed(string name);
    Task DeleteColumnAsync(Guid id);
}
