namespace BLL.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Delete(T entity);
    void Update(T entity);
    void Insert(T entity);
}