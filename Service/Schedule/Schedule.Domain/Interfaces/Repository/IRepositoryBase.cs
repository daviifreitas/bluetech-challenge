namespace Schedule.Domain.Interfaces.Repository;

public interface IRepositoryBase<T> where T : class, IEntity
{
    Task<bool> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<TX>> GetManyAsDynamicTypeByFilterAsync<TX>(Func<T,bool> filter, Func<T, TX> selector);
    Task<T?> GetOneByFunctionAsync(Func<T, bool> func);
}