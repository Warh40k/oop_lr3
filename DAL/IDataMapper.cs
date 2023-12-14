namespace DAL;

public interface IDataMapper<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Save(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Insert(T entity);
}