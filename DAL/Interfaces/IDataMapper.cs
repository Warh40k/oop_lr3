namespace DAL.Interfaces;

public interface IDataMapper<T>
{
    IEnumerable<T>? GetAll();
    T? GetById(int id);
    void Save(T entity);
    void Delete(T entity);
}