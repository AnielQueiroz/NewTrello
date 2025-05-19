using AutoMapper;
using NewTrello.Dtos;
using NewTrello.Models;
using NewTrello.Repositories;
using NewTrello.Services.Interfaces;

namespace NewTrello.Services;

public class ColumnService(BoardRepository boardRepository, ColumnRepository repo, IMapper mapper) : IColumnService
{
    private readonly ColumnRepository _repo = repo;
    private readonly BoardRepository _boardRepository = boardRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> CheckIfColumnNameAlreadyUsed(string name)
    {
        return await _repo.GetAllAsync()
            .ContinueWith(task => task.Result
            .Any(c => c.Name == name));
    }

    public async Task<ColumnResponseDTO?> CreateColumnAsync(ColumnCreateRequestDTO dto)
    {
        var boardExists = await _boardRepository.GetByIdAsync(dto.BoardId);
        if (boardExists is null)
            return null;

        var column = _mapper.Map<Column>(dto);
        column.Id = Guid.NewGuid();

        await _repo.AddAsync(column);
        await _repo.SaveAsync();

        return _mapper.Map<ColumnResponseDTO>(column);
    }

    public async Task DeleteColumnAsync(Guid id)
    {
        var column = await _repo.GetByIdAsync(id);
        if (column is not null)
        {
            _repo.Delete(column);
            await _repo.SaveAsync();
        }
    }

    public async Task<IEnumerable<ColumnResponseDTO>> GetAllColumnsAsync()
    {
        var columns = await _repo.GetAllWithIncludesAsync();
        return _mapper.Map<IEnumerable<ColumnResponseDTO>>(columns);
    }

    public async Task<IEnumerable<ColumnResponseDTO?>> GetColumnsByBoard(Guid boardId)
    {
        var columns = await _repo.GetColumnsByBoard(boardId);
        return _mapper.Map<IEnumerable<ColumnResponseDTO>>(columns);
    }

    public async Task<ColumnResponseDTO?> GetColumnByIdAsync(Guid id)
    {
        var column = await _repo.GetByIdWithIncludesAsync(id);
        return _mapper.Map<ColumnResponseDTO?>(column);
    }

    public async Task<ColumnResponseDTO?> UpdateColumnAsync(Guid id, ColumnUpdateRequestDTO dto)
    {
        var column = await _repo.GetByIdAsync(id);
        if (column is null)
            return null;

        _mapper.Map(dto, column);
        await _repo.SaveAsync();
        return _mapper.Map<ColumnResponseDTO>(column);
    }
}
