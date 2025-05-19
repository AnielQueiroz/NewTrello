using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewTrello.Context;
using NewTrello.Models;

namespace NewTrello.Repositories;

public class ColumnRepository(AppDbContext context) : BaseRepository<Models.Column>(context)
{
    public async Task<IEnumerable<Models.Column>> GetAllWithIncludesAsync()
    {
        return await _context.Columns!
            .Include(c => c.Board!)
            .Include(c => c.Cards!)
            .ToListAsync();
    }

    public async Task<Models.Column?> GetByIdWithIncludesAsync(Guid id)
    {
        return await _context.Columns!
            .Include(c => c.Board!)
            .Include(c => c.Cards!)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task <IEnumerable<Models.Column?>> GetColumnsByBoard(Guid boardId)
    {
        return await _context.Columns!
            .Where(c => c.BoardId == boardId)
            .ToListAsync();
    }
}
