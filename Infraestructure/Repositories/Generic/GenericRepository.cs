using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Generic
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected  AdventureWorks2017Context DbContext { get; set; }

        protected GenericRepository(AdventureWorks2017Context dbContext)
        {
            DbContext = dbContext;
        }

        public Task<List<T>> GetAllRecords()
        {
            return DbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await DbContext.FindAsync<T>(id);
        }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>();
        }


        public async Task InsertAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbContext.FindAsync<T>(id);
                DbContext.Remove(entity);
            }
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<T> SqlQuery(string query)
        {
            var result = DbContext.Set<T>().FromSql(query);
            return result;
        }

        public async Task ExecuteCommand(string query)
        {
            await DbContext.Database.ExecuteSqlCommandAsync(query);
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }



    }
}
