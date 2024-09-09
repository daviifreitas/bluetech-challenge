using Schedule.Domain.Interfaces.Repository;
using Schedule.Domain.Interfaces.Service;

namespace Schedule.Domain.Services;

public class ServiceBase<T>(IRepositoryBase<T> repositoryBase) : IServiceBase<T> where T : class, IEntity
{
    public async Task<bool> CreateAsync(T entity)
    {
        return await repositoryBase.CreateAsync(entity);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        return await repositoryBase.UpdateAsync(entity);
    }

    public Task<bool> DeleteAsync(T entity)
    {
        return repositoryBase.DeleteAsync(entity);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await repositoryBase.GetByIdAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var allAsync = await repositoryBase.GetAllAsync();
        var allAsyncWithStatusDefaultActive = allAsync.Where(x => x.StatusDefault == StatusDefault.Active);

        return allAsyncWithStatusDefaultActive;
    }

    public async Task<IEnumerable<TX>> GetManyAsDynamicTypeByFilterAsync<TX>(Func<T, bool> filter, Func<T, TX> selector)
    {
        return await repositoryBase.GetManyAsDynamicTypeByFilterAsync(filter, selector);
    }

    public Task<IEnumerable<TX>> GetManyAsDynamicTypeWithPaginationAsync<TX>(Func<T, bool> filter, Func<T, TX> selector, int skip, int take)
    {
        throw new NotImplementedException();
    }
}