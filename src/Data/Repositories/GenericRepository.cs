using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
{
    private readonly DataContext context;

    protected GenericRepository(DataContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().AsNoTracking().ToListAsync();
    }
    
    public async Task<TEntity> GetById(Guid id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }
    
    public async Task<bool> Create(TEntity entity)
    {
        try
        {
            await context.Set<TEntity>().AddAsync(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var entity = await GetById(id);
            context.Set<TEntity>().Remove(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Update(TEntity entity)
    {
        try
        {
            context.Set<TEntity>().Update(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}