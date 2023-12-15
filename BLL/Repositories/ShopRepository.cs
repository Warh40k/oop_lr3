using BLL.Interfaces;

namespace BLL.Repositories;

public class ShopRepository : IRepository<DAL.Entities.Shop.Shop>
{
    public IEnumerable<DAL.Entities.Shop.Shop> GetAll()
    {
        throw new NotImplementedException();
    }

    public DAL.Entities.Shop.Shop GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Save(DAL.Entities.Shop.Shop entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(DAL.Entities.Shop.Shop entity)
    {
        throw new NotImplementedException();
    }

    public void Update(DAL.Entities.Shop.Shop entity)
    {
        throw new NotImplementedException();
    }

    public void Insert(DAL.Entities.Shop.Shop entity)
    {
        throw new NotImplementedException();
    }
}