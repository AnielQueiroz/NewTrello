using Microsoft.EntityFrameworkCore;
using NewTrello.Context;
using NewTrello.Models;

namespace NewTrello.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context)
{
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users!.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users!
            .Where(u => u.Email == email)
            .Select(u => new User
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password
            })
            .FirstOrDefaultAsync();
    }
}
