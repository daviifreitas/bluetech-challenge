using BuildingBlocks.Entity;
using Microsoft.EntityFrameworkCore;
using Schedule.Domain.Interfaces.Repository;
using Schedule.Infra.Data.DbContext;

namespace Schedule.Infra.Data.Repository;

public class RepositoryBase<T>(ScheduleDbContext dbContext) : IRepositoryBase<T> where T : class, IEntity
{
    public async Task<bool> CreateAsync(T entity)
    {
        entity.StatusDefault = StatusDefault.Active;
        entity.CreatedAt = DateTime.Now;
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        entity.StatusDefault = StatusDefault.Deleted;
        entity.UpdatedAt = DateTime.Now;
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var findAsync = await dbContext.Set<T>().FindAsync(id);
        
        return findAsync?.StatusDefault == StatusDefault.Active ? findAsync : null;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return dbContext.Set<T>().AsNoTracking().AsSplitQuery().AsEnumerable();
    }

    public async Task<IEnumerable<TX>> GetManyAsDynamicTypeByFilterAsync<TX>(Func<T, bool> filter, Func<T, TX> selector)
    {
        return dbContext.Set<T>().AsNoTracking().AsSplitQuery().AsEnumerable().Where(filter).Select(selector).ToList();
    }

    public async Task<T?> GetOneByFunctionAsync(Func<T, bool> func)
    {
        return dbContext.Set<T>().FirstOrDefault(func);
    }
}