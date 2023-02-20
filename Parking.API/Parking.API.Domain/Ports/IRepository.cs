using System.Linq.Expressions;

namespace Parking.API.Domain.Ports
{

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeStringProperties = "",
            bool isTracking = false);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             bool isTracking = false, params Expression<Func<T, object>>[] includeObjectProperties);

        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
    }
}
