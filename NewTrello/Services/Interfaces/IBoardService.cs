using NewTrello.Dtos;

namespace NewTrello.Services.Interfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardResponseDTO>> GetAllBoardsAsync();
        Task<BoardResponseDTO?> GetBoardByIdAsync(Guid id);
        Task<BoardResponseDTO?> CreateBoardAsync(BoardCreateDTO dto);
        Task<BoardResponseDTO?> UpdateBoardAsync(Guid id, BoardUpdateDTO dto);
        Task<bool> CheckIfBoardNameAlreadyUsed(string name);
        Task DeleteBoardAsync(Guid id);
    }
}
