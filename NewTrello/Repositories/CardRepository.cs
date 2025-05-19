using Microsoft.EntityFrameworkCore;
using NewTrello.Context;
using NewTrello.Models;

namespace NewTrello.Repositories;

public class CardRepository(AppDbContext context) : BaseRepository<Card>(context)
{
    public async Task<IEnumerable<Card>> GetAllWithIncludesAsync()
    {
        return await _context.Cards!
            .Include(c => c.Column)
            .Include(c => c.AssignedUser)
            .ToListAsync();
    }
    public async Task<Card?> GetByIdWithIncludesAsync(Guid id)
    {
        return await _context.Cards!
            .Include(c => c.Column!)
            .Include(c => c.AssignedUser!)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Card?>> GetCardsByColumn(Guid columnId)
    {
        return await _context.Cards!
            .Where(c => c.ColumnId == columnId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Card?>> GetCardsByUser(Guid userId)
    {
        return await _context.Cards!
            .Where(c => c.AssignedUserId == userId)
            .ToListAsync();
    }
}
