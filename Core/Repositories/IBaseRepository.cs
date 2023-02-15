
namespace Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T Add(T entity);
        T GetById(long id);
    }
}
