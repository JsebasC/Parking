using Microsoft.EntityFrameworkCore;
using Parking.API.Domain.Ports;
using Parking.API.Infrastructure.Data;
using System.Linq.Expressions;

namespace Parking.API.Infrastructure.Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {        
        private readonly ParkingContext _context;
        private readonly DbSet<T> EntitySet;

        public Repository(ParkingContext context)
        {
            _context = context;
            EntitySet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeStringProperties = "", bool isTracking = false)
        {
            IQueryable<T> query = EntitySet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeStringProperties))
            {
                foreach (var includeProperty in includeStringProperties.Split
                    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isTracking = false, params Expression<Func<T, object>>[] includeObjectProperties)
        {
            IQueryable<T> query = EntitySet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties != null)
            {
                foreach (Expression<Func<T, object>> include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }
        public async Task<T> GetById(Guid id)
        {
            var result = await EntitySet.FindAsync(id)!;
            return result!;
        }

        public async Task Insert(T entity)
        {            
            await EntitySet.AddAsync(entity);
            await Save();
        }

        public async Task Update(T entity)
        {
            EntitySet.Update(entity);
            await Save();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            EntitySet.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }     
    }
}
