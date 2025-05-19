using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewTrello.Context;

namespace NewTrello.Repositories;

public abstract class BaseRepository<TEntity>(AppDbContext context) where TEntity : class
{
    protected readonly AppDbContext _context = context;

    // Metodo com include
    //public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
    //    params Expression<Func<TEntity, object>>[] includes)
    //{
    //    IQueryable<TEntity> query = _context.Set<TEntity>();

    //    foreach (var include in includes)
    //        query = query.Include(include);

    //    return await query.ToListAsync();
    //}

    // Metoodo com include
    //public virtual async Task<TEntity?> GetByIdAsync(
    //    Guid id,
    //    params Expression<Func<TEntity, object>>[] includes)
    //{
    //    IQueryable<TEntity> query = _context.Set<TEntity>();

    //    foreach (var include in includes)
    //        query = query.Include(include);

    //    return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    //}

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await _context.Set<TEntity>().FindAsync(id);

    public virtual async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

    public virtual void Delete(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

    public virtual async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
