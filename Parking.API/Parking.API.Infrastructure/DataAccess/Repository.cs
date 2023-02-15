using Microsoft.EntityFrameworkCore;
using Parking.API.Domain.DataAccess;
using Parking.API.Infrastructure.Data;

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

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }
        public async Task<T> GetById(Guid id)
        {
            return await EntitySet.FindAsync(id);
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
