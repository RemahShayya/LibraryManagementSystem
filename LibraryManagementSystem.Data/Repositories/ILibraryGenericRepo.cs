using System.Linq.Expressions;

namespace LibraryManagementSystem.Data.Data.Repositories
{
    public interface ILibraryGenericRepo<T> where T : class
    {
        Task<T> Get(object id);
        Task<List<T>> GetAll();
        T Update(T model);
        Task<T> Add(T model);
        void Delete(T model);
        Task SaveAsync();
        Task<bool> Exists(object id);
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    }
}
