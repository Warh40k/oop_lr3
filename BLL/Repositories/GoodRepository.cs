using BLL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using DTOs;

namespace BLL.Repositories;

public class GoodRepository : IRepository<Good>
{
    private IDataMapper<GoodDto> _mapper;

    public IEnumerable<Good> GetAll()
    {
        throw new NotImplementedException();
    }

    public Good GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Good entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Good entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Good entity)
    {
        throw new NotImplementedException();
    }

    public void Insert(Good entity)
    {
        throw new NotImplementedException();
    }
}