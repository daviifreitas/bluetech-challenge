namespace Schedule.Domain.Interfaces.Service;

public interface IServiceBase<T> where T : class, IEntity
{
    Task<bool> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<TX>> GetManyAsDynamicTypeByFilterAsync<TX>(Func<T, bool> filter, Func<T, TX> selector);

    Task<IEnumerable<TX>> GetManyAsDynamicTypeWithPaginationAsync<TX>(Func<T, bool> filter, Func<T, TX> selector,
        int skip, int take);
}