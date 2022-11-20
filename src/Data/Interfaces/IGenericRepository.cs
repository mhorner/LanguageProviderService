namespace Data.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(Guid id);
    Task<bool> Create(TEntity entity);
    Task<bool> Delete(Guid id);
    Task<bool> Update(TEntity entity);
}