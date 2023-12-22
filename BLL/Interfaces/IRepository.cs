namespace BLL.Interfaces;

public interface IRepository<T>
{
    IEnumerable<T> GetAll(string statement);
    T? GetById(int id);
    void Save(T entity);
    void Delete(T entity);
}