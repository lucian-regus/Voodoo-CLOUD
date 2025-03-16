using Domain.Database;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IRepositoryBase<T> where T : EntityBase
{
    public IQueryable<T> Get(bool includeDeleted = false);
    public void Update(T entity);
    public void UpdateRange(IEnumerable<T> entities);
    public void Add(T entity);
    public void AddRange(IEnumerable<T> entities);
    public void Delete(T entity, bool hardDelete = false);
    public void DeleteRange(IEnumerable<T> entities, bool hardDelete = false);
    public Task<List<T>> GetAllAsync(bool includeDeleted = false);
} 

public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly DatabaseContext _databaseContext;
    private readonly DbSet<T> _dbSet;
    
    public RepositoryBase(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = databaseContext.Set<T>();
    }

    public IQueryable<T> Get(bool includeDeleted = false)
    {
        return includeDeleted
            ? _dbSet.IgnoreQueryFilters()
            : _dbSet;
    }

    public void Update(T entity)
    {
        foreach (var property in _databaseContext.Entry(entity).Properties.Where(p => !p.Metadata.IsKey()))
            property.IsModified = true;
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            Update(entity);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _dbSet.Add(entity);
        }
    }

    public void Delete(T entity, bool hardDelete = false)
    {
        if (hardDelete)
            _dbSet.Remove(entity);
        else
            entity.DeletedAt = DateTime.UtcNow;
    }

    public void DeleteRange(IEnumerable<T> entities, bool hardDelete = false)
    {
        if (hardDelete)
            _dbSet.RemoveRange(entities);
        else
        {
            var now = DateTime.UtcNow;
            
            foreach (var entity in entities)
                entity.DeletedAt = now;
        }
    }

    public async Task<List<T>> GetAllAsync(bool includeDeleted = false)
    {
        return await Get(includeDeleted).ToListAsync();
    }
}
