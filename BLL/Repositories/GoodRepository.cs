using System.Collections.ObjectModel;
using BLL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using DTOs;

namespace BLL.Repositories;

public class GoodRepository : IRepository<Good>
{
    private IGoodDataMapper _mapper;

    public GoodRepository(IGoodDataMapper mapper)
    {
        _mapper = mapper;
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

    public int FindCheapestShop(Good good)
    {
        return _mapper.FindCheapestShop(ToDto(good));
    }

    public IEnumerable<Good> CountGoodCountForBudget(int? shopId, int budget)
    {
        var dtos = _mapper.GetGoodsForBudget(shopId, budget);
        var goods = new ObservableCollection<Good>();
        foreach (var dto in dtos)
        {
            goods.Add(FromDto(dto));
        }

        return goods;
    }
    
    public Good GetById(int id)
    {
        return FromDto(_mapper.GetById(id));
    }

    private Good FromDto(GoodDto? dto)
    {
        return new Good
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Quantity = dto.Quantity,
            AffordCount = dto.AffordCount
        };
    }

    private GoodDto ToDto(Good entity)
    {
        return new GoodDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            AffordCount = entity.AffordCount
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

    public void AddToShop(int? shopId, Good good)
    {
        _mapper.AddGoodToShop(shopId, ToDto(good));
    }
}