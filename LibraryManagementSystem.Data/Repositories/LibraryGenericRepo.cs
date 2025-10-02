using LibraryManagmentSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Data.Data.Repositories
{
    public class LibraryGenericRepo<T> : ILibraryGenericRepo<T> where T : class
    {
        LibraryContext context;
        public LibraryGenericRepo(LibraryContext _context)
        {
            context = _context;
        }
        public async Task<T> Add(T model)
        {
            var entity=await context.Set<T>().AddAsync(model);
            return entity.Entity;

        }

        public void Delete(T model)
        {
            context.Remove(model);
        }

        public async Task<T> Get(object id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            var objects = await context.Set<T>().ToListAsync();
            return objects;
        }

        public T Update(T model)
        {
            context.Set<T>().Update(model);
            return model;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<bool> Exists(object id)
        {
            return await context.Set<T>().FindAsync(id) != null;
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }



    }
}
