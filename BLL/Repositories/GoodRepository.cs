using BLL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using DTOs;

namespace BLL.Repositories;

public class GoodRepository : IRepository<Good>
{
    private IDataMapper<GoodDto> _mapper;

    public GoodRepository(IDataMapper<GoodDto> mapper)
    {
        _mapper = mapper;
    }

    public IEnumerable<Good> GetAll()
    {
        var dtos = _mapper.GetAll();
        List<Good> goods = new List<Good>();

        foreach (var dto in dtos)
        {
            goods.Add(FromDto(dto));
        }

        return goods;
    }

    public Good? GetById(int id)
    {
        return FromDto(_mapper.GetById(id));
    }

    private Good? FromDto(GoodDto? dto)
    {
        return new Good
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    private GoodDto ToDto(Good entity)
    {
        return new GoodDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public void Save(Good entity)
    {
        _mapper.Save(ToDto(entity));
    }

    public void Delete(Good entity)
    {
        _mapper.Delete(ToDto(entity));
    }
}