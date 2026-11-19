using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LocalDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(LocalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // la table correspond à T
        }
        public async Task<List<T>> GetAllAsync()    // SELECT * FROM T
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)  // SELECT * FROM T WHERE clé = id
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> AddAsync(T entity) // INSERT INTO T VALUES
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(T entity) // UPDATE
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity) // DELETE
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
