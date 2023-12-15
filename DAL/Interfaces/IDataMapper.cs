namespace DAL.Interfaces;

public interface IDataMapper<D>
{
    IEnumerable<D> GetAll(string statement = "");
    D? GetById(int id);
    int? Save(D dto);
    void Update(D dto);
    void Delete(D dto);

}