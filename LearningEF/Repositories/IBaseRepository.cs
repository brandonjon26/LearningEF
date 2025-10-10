using System.Threading.Tasks;

namespace LearningEF.Repositories
{
    public interface IBaseRepository<T, TId> where T : class
    {
        Task<int> SaveChangesAsync();
        Task<T?> GetByIdAsync(TId id);

    }
}
