namespace Dot.Net.WebApi.Repositories
{
    public interface IRepository<T> where T : class //"where T : class" impose que T soit une classe
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
