using DTOs;

namespace DAL.Interfaces;

public interface IDataMapper<T>
{
    IEnumerable<T> GetAll(string statement = "");
    T? GetById(int id);
    int? Save(T dto);
    void Delete(T dto);
    

}