using eCommerceApp.Application.Exceptions;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories;

public class GenericRepository<TEntity>(AppDbContext context) : IGeneric<TEntity> where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await  context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        TEntity? entity = await context.Set<TEntity>().FindAsync(id);
        return entity!;
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        TEntity? entityToDelete = await context.Set<TEntity>().FindAsync(id);
        if (entityToDelete is null)
            return 0;
        
        context.Set<TEntity>().Remove(entityToDelete);
        return await context.SaveChangesAsync();
    }
}