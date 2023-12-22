using BLL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using DTOs;

namespace BLL.Repositories;

public class ShopRepository : IRepository<Shop>
{
    private IDataMapper<ShopDto> _mapper;

    public ShopRepository(IDataMapper<ShopDto> mapper)
    {
        _mapper = mapper;
    }
    public IEnumerable<Shop> GetAll(string statement="")
    {
        var dtos = _mapper.GetAll();
        List<Shop> shops = new List<Shop>();

        foreach (var dto in dtos)
        {
            shops.Add(FromDto(dto));
        }

        return shops;
    }
    
    private Shop? FromDto(ShopDto? dto)
    {
        return new Shop
        {
            Id = dto.Id,
            Name = dto.Name,
            Address = dto.Address
        };
    }

    private ShopDto ToDto(Shop entity)
    {
        return new ShopDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address
        };
    }

    public Shop? GetById(int id)
    {
        return FromDto(_mapper.GetById(id));
    }

    public void Save(Shop entity)
    {
        _mapper.Save(ToDto(entity));
    }

    public void Delete(Shop entity)
    {
        _mapper.Delete(ToDto(entity));
    }
}