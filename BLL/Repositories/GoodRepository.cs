using System.Xml;
using BLL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using DTOs;

namespace BLL.Repositories;

public class GoodRepository : IRepository<Good>
{
    private IGoodDataMapper _mapper;
    private IShopGoodDataMapper _shopGoodMap;

    public GoodRepository(IGoodDataMapper mapper, IShopGoodDataMapper shopGoodMap)
    {
        _mapper = mapper;
        _shopGoodMap = shopGoodMap;
    }

    public IEnumerable<Good> GetAll(string statement = "")
    {
        var dtos = _mapper.GetAll(statement);
        List<Good> goods = new List<Good>();

        foreach (var dto in dtos)
        {
            goods.Add(FromDto(dto));
        }

        return goods;
    }

    public IEnumerable<Good> GetGoodsFromShop(int? shopId)
    { 
        var goodDtos = _mapper.GetGoodsFromShop(shopId);
        var goods = new List<Good>();
        foreach (var dto in goodDtos)
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
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity
        };
    }

    private GoodDto ToDto(Good entity)
    {
        return new GoodDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity
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

    public void DeleteFromShop(int shopId, Good entity)
    {
        _mapper.DeleteGoodFromShop(shopId, ToDto(entity));
    }

    public void AddToShop(int shopId, Good entity)
    {
        _mapper.AddGoodToShop(shopId, ToDto(entity));
    }
}