using AutoMapper;
using NewTrello.Dtos;
using NewTrello.Models;
using NewTrello.Repositories;
using NewTrello.Services.Interfaces;

namespace NewTrello.Services;

public class BoardService(BoardRepository repo, UserRepository userRepo, IMapper mapper) : IBoardService
{
    private readonly BoardRepository _repo = repo;
    private readonly UserRepository _useRepo = userRepo;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<BoardResponseDTO>> GetAllBoardsAsync()
    {
        var boards = await _repo.GetAllWithIncludesAsync();
        return _mapper.Map<IEnumerable<BoardResponseDTO>>(boards);
    }

    public async Task<BoardResponseDTO?> GetBoardByIdAsync(Guid id)
    {
        var board = await _repo.GetByIdWithIncludesAsync(id);
        return _mapper.Map<BoardResponseDTO?>(board);
    }

    public async Task<bool> CheckIfBoardNameAlreadyUsed(string name)
    {
        return await _repo.GetAllAsync()
            .ContinueWith(task => task.Result
                .Any(b => b.Name == name));
    }

    public async Task<BoardResponseDTO?> CreateBoardAsync(BoardCreateDTO dto)
    {
        var userExists = await _useRepo.GetByIdAsync(dto.OwnerId);
        if (userExists is null)
        {
            return null;
        }

        var board = _mapper.Map<Board>(dto);
        board.Id = Guid.NewGuid();

        await _repo.AddAsync(board);
        await _repo.SaveAsync();

        return _mapper.Map<BoardResponseDTO>(board);
    }

    public async Task DeleteBoardAsync(Guid id)
    {
        var board = await _repo.GetByIdAsync(id);
        if (board is not null)
        {
            _repo.Delete(board);
            await _repo.SaveAsync();
        }
    }

    public async Task<BoardResponseDTO?> UpdateBoardAsync(Guid id, BoardUpdateDTO dto)
    {
        var board = await _repo.GetByIdAsync(id);
        if (board is null)
            return null;

        _mapper.Map(dto, board);
        await _repo.SaveAsync();

        return _mapper.Map<BoardResponseDTO>(board);
    }
}
