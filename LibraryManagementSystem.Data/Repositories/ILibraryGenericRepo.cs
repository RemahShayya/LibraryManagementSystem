namespace LibraryManagementSystem.Data.Data.Repositories
{
    public interface ILibraryGenericRepo<T> where T : class
    {
        Task<T> Get(Guid id);
        Task<List<T>> GetAll();
        T Update(T model);
        Task<T> Add(T model);
        void Delete(T model);
        Task SaveAsync();
        Task<bool> Exists(Guid id);
    }
}
