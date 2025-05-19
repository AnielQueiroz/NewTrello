using Microsoft.EntityFrameworkCore;
using NewTrello.Models;

namespace NewTrello.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Board>? Boards { get; set; }
    public DbSet<Card>? Cards { get; set; }
    public DbSet<Column>? Columns { get; set; }
}
