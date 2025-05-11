using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        // Get methods
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // Add methods
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update methods
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        // Remove methods
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        // Count methods
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Any methods
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        // Include related entities
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}