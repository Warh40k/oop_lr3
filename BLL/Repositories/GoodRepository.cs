using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Repositories;

public class GoodRepository : IRepository<DAL.Entities.Good.Good>
{
    private IDataMapper<DAL.Entities.Good.Good> _mapper;

    public IEnumerable<DAL.Entities.Good.Good> GetAll()
    {
        throw new NotImplementedException();
    }

    public DAL.Entities.Good.Good GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(DAL.Entities.Good.Good entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(DAL.Entities.Good.Good entity)
    {
        throw new NotImplementedException();
    }

    public void Update(DAL.Entities.Good.Good entity)
    {
        throw new NotImplementedException();
    }

    public void Insert(DAL.Entities.Good.Good entity)
    {
        throw new NotImplementedException();
    }
}