namespace Parking.API.Domain.DataAccess
{

    public interface IRepository<T> where T : class
    {
        //IQueryable<T> GetAll();        
        Task<IEnumerable<T>> GetAll();        
        Task<T> GetById(Guid id);
        Task Insert(T entity);        
        Task Update(T entity);
        Task Delete(Guid id);        
    }
}
