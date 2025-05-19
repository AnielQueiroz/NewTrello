using AutoMapper;
using NewTrello.Dtos;
using NewTrello.Models;
using NewTrello.Repositories;
using NewTrello.Services.Interfaces;

namespace NewTrello.Services;

public class CardService(ColumnRepository columnRepository, CardRepository repo, UserRepository userRepo, IMapper mapper) : ICardService
{
    private readonly IMapper _mapper = mapper;
    private readonly CardRepository _repo = repo;
    private readonly ColumnRepository _columnRepository = columnRepository;
    private readonly UserRepository _userRepo = userRepo;

    public async Task<CardResponseDTO?> CreateCardAsync(CardCreateRequestDTO dto)
    {
        var columnExists = await _columnRepository.GetByIdAsync(dto.ColumnId);
        if (columnExists is null)
            return null;

        var card = _mapper.Map<Card>(dto);
        card.Id = Guid.NewGuid();
        await _repo.AddAsync(card);
        await _repo.SaveAsync();
        return _mapper.Map<CardResponseDTO>(card);
    }

    public async Task DeleteCardAsync(Guid id)
    {
        var card = await _repo.GetByIdAsync(id);
        if (card is not null)
        {
            _repo.Delete(card);
            await _repo.SaveAsync();
        }
    }

    public async Task<IEnumerable<CardResponseDTO>> GetAllCardsAsync()
    {
        var cards = await _repo.GetAllWithIncludesAsync();
        return _mapper.Map<IEnumerable<CardResponseDTO>>(cards);
    }

    public async Task<CardResponseDTO?> GetCardByIdAsync(Guid id)
    {
        var card = await _repo.GetByIdWithIncludesAsync(id);
        return _mapper.Map<CardResponseDTO?>(card);
    }

    public async Task<IEnumerable<CardResponseDTO?>> GetCardsByColumn(Guid columnId)
    {
        var cards = await _repo.GetCardsByColumn(columnId);
        return _mapper.Map<IEnumerable<CardResponseDTO?>>(cards);
    }

    public async Task<IEnumerable<CardResponseDTO?>> GetCardsByUser(Guid userId)
    {
        var cards = await _repo.GetCardsByUser(userId);
        return _mapper.Map<IEnumerable<CardResponseDTO?>>(cards);
    }

    public async Task<ServiceResult<CardResponseDTO?>> UpdateCardAsync(Guid id, CardUpdateRequestDTO dto)
    {
        var card = await _repo.GetByIdAsync(id);
        if (card is null)
            return ServiceResult<CardResponseDTO?>.Fail("Cartão não encontrado!");

        if (card is not null)
        {
            if (dto.AssignedUserId.HasValue && card.AssignedUserId != dto.AssignedUserId) 
            {
                var user = await _userRepo.GetByIdAsync((Guid)dto.AssignedUserId);
                if (user is null)
                    return ServiceResult<CardResponseDTO?>.Fail("Usuário não encontrado!");

                card.AssignedUserId = dto.AssignedUserId;
            }
        }

        _mapper.Map(dto, card);
        await _repo.SaveAsync();

        var response = _mapper.Map<CardResponseDTO>(card);
        return ServiceResult<CardResponseDTO?>.Ok(response);
    }
}
