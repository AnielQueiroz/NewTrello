using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTrello.Dtos;
using NewTrello.Services.Interfaces;

namespace NewTrello.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController(IBoardService boardService) : Controller
{
    private readonly IBoardService _boardService = boardService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BoardResponseDTO>>> GetBoards()
    {
        var boards = await _boardService.GetAllBoardsAsync();
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BoardResponseDTO>> GetBoard(Guid id)
    {
        var board = await _boardService.GetBoardByIdAsync(id);
        if (board is null)
            return NotFound(new { message = "Quadro não encontrado!" });

        return Ok(board);
    }

    [HttpPost]
    public async Task<ActionResult<BoardResponseDTO>> CreateBoard(BoardCreateDTO dto)
    {
        var boardNameAlreadyUsed = await _boardService.CheckIfBoardNameAlreadyUsed(dto.Name);
        if (boardNameAlreadyUsed)
            return BadRequest(new { message = "Já existe um quadro com este nome!" });

        var boardDto = await _boardService.CreateBoardAsync(dto);

        if (boardDto is null)
            return BadRequest(new { message = "Usuário não encontrado!" });

        return CreatedAtAction(nameof(GetBoard), new { id = boardDto.Id }, boardDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBoard(Guid id, BoardUpdateDTO dto)
    {
        var boardExist = await _boardService.GetBoardByIdAsync(id);
        if (boardExist is null)
            return NotFound(new { message = "Quadro não encontrado!" });

        await _boardService.UpdateBoardAsync(id, dto);

        return Ok(new { message = "Atualizado com sucesso!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(Guid id)
    {
        var boardExists = await _boardService.GetBoardByIdAsync(id);
        if (boardExists is null)
            return NotFound(new { message = "Quadro não encontrado!" });

        await _boardService.DeleteBoardAsync(id);

        return NoContent();
    }
}
