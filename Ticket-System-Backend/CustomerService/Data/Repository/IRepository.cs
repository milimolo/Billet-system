namespace CustomerApi.Data.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task EditAsync(T entity, CancellationToken cancellationToken = default);
        Task RemoveAsync(int id, CancellationToken cancellationToken = default);
    }
}
