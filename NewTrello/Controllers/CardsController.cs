using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTrello.Dtos;
using NewTrello.Services.Interfaces;

namespace NewTrello.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CardsController(ICardService cardService) : ControllerBase
    {
        private readonly ICardService _cardService = cardService;

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CardDto>>> GetCards()
        //{
        //    var cards = await _cardService.GetAllCardsAsync();
        //    return Ok(cards);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<CardResponseDTO>> GetCard(Guid id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            if (card is null)
                return NotFound(new { message = "Cartão não encontrado!" });
            return Ok(card);
        }

        [HttpGet("column/{columnId}")]
        public async Task<ActionResult<CardResponseDTO>> GetCardsByColumn(Guid columnId)
        {
            var cards = await _cardService.GetCardsByColumn(columnId);
            
            return Ok(cards);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CardResponseDTO>> GetCardsByUser(Guid userId)
        {
            var cards = await _cardService.GetCardsByUser(userId);
            return Ok(cards);
        }

        [HttpPost]
        public async Task<ActionResult<CardResponseDTO>> CreateCard(CardCreateRequestDTO dto)
        {
            var cardDto = await _cardService.CreateCardAsync(dto);
            if (cardDto is null)
                return NotFound(new { message = "Coluna não encontrada"! });

            return CreatedAtAction(nameof(GetCard), new { id = cardDto.Id }, cardDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCard(Guid id, CardUpdateRequestDTO dto)
        {            
            var result = await _cardService.UpdateCardAsync(id, dto);
            if(!result.Success)
                return BadRequest(new { message = result.ErrorMessage });
            
            return Ok(new { message = "Atualizado com sucesso!", card = result.Data });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            var card = await _cardService.GetCardByIdAsync(id);
            if (card is null)
                return NotFound(new { message = "Cartão não encontrado!" });
            await _cardService.DeleteCardAsync(id);
            return Ok(new { message = "Deletado com sucesso!" });
        }
    }
}
