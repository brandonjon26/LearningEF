using LearningEF.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningEF.Repositories
{
    public abstract class BaseRepository<T, TId> : IBaseRepository<T, TId> where T : class
    {
        // DbContext is protected so derived classes can access it
        protected readonly DbContext _context;
        // Define DbSet<T> using generic type parameter T
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            // Get the correct DbSet from the context based on the Type T
            _dbSet = context.Set<T>();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            // Write changes to the database
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(TId id)
        {
            // Get the specific entity
            return await _dbSet.FindAsync(id);
        }
    }
}
