using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTrello.Dtos;
using NewTrello.Services.Interfaces;

namespace NewTrello.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColumnsController(IColumnService columnService) : ControllerBase
{
    private readonly IColumnService _columnService = columnService;

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<ColumnDto>>> GetColumns()
    //{
    //    var columns = await _columnService.GetAllColumnsAsync();
    //    return Ok(columns);
    //}

    [HttpGet("board/{boardId}")]
    public async Task<ActionResult<IEnumerable<ColumnResponseDTO>>> GetColumnByBoard(Guid boardId)
    {
        var columns = await _columnService.GetColumnsByBoard(boardId);

        return Ok(columns);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColumnResponseDTO>> GetColumn(Guid id)
    {
        var column = await _columnService.GetColumnByIdAsync(id);

        if (column is null)
            return NotFound(new { message = "Coluna não encontrada!" });

        return Ok(column);
    }

    [HttpPost]
    public async Task<ActionResult<ColumnResponseDTO>> CreateColumn(ColumnCreateRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new { message = "Erro de validação", errors });
        }

        var duplicate = await _columnService.CheckIfColumnNameAlreadyUsed(dto.Name);
        if (duplicate)
            return BadRequest(new { message = "Já existe uma coluna com este nome!" });

        var columnDto = await _columnService.CreateColumnAsync(dto);

        if (columnDto is null)
            return BadRequest(new { message = "Quadro não encontrado!" });

        return CreatedAtAction(nameof(GetColumn), new { id = columnDto.Id }, columnDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateColumn(Guid id, ColumnUpdateRequestDTO dto)
    {
        var columnExists = await _columnService.GetColumnByIdAsync(id);
        if (columnExists is null)
            return NotFound(new { message = "Coluna não encontrada!" });

        await _columnService.UpdateColumnAsync(id, dto);

        return Ok(new { message = "Atualizado com sucesso!" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColumn(Guid id)
    {
        var columnExists = await _columnService.GetColumnByIdAsync(id);
        if (columnExists is null)
            return NotFound(new { message = "Coluna não encontrada!" });

        await _columnService.DeleteColumnAsync(id);

        return NoContent();
    }
}
