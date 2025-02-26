using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcoinverGMAO_api.Data
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(long id);
        Task HardDeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<int> BulkUpdateAsync(IEnumerable<T> entities);
        IQueryable<T> AsQueryable();


    }
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            // Ajustado para que filtre por DeletedAt == null y el Id coincida
            return await _dbSet
                .Where(t => t.DeletedAt == null && t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .Where(t => t.DeletedAt == null)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .ToArrayAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> BulkUpdateAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retorna un IQueryable para consultas personalizadas (Where, OrderBy, etc.).
        /// </summary>
        public IQueryable<T> AsQueryable()
        {
            // Si deseas filtrar siempre los registros "no eliminados", puedes hacer:
            return _dbSet.Where(t => t.DeletedAt == null).AsQueryable();

            // O, si prefieres que devuelva TODOS los registros, incluso los eliminados:
            // return _dbSet.AsQueryable();
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                foreach (var entry in _context.ChangeTracker.Entries<BaseModel>())
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("A concurrency conflict occurred.", ex);
            }
        }
    }
}
