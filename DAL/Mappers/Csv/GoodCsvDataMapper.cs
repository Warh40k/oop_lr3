using System.Data.Common;
using DAL.Entities.Good;
using DAL.Entities.Shop;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Csv;

public class GoodCsvDataMapper : IGoodDataMapper
{
    public IEnumerable<GoodDto> GetAll(string statement="")
    {
        throw new NotImplementedException();
    }

    public GoodDto GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int? Save(GoodDto entity)
    {
        throw new NotImplementedException();
    }

    public void Update(GoodDto dto)
    {
        throw new NotImplementedException();
    }

    public void Delete(GoodDto entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GoodDto> GetGoodsFromShop(int? shopId)
    {
        throw new NotImplementedException();
    }

    public void AddGoodToShop(int? shopId, GoodDto goodDto)
    {
        throw new NotImplementedException();
    }

    public void DeleteGoodFromShop(int shopId, GoodDto goodDto)
    {
        throw new NotImplementedException();
    }

    public int FindCheapestShop(GoodDto goodDto)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<GoodDto> GetGoodsForBudget(int? shopId, int budget)
    {
        throw new NotImplementedException();
    }

    public Good FromDto(GoodDto dto)
    {
        throw new NotImplementedException();
    }

    public GoodDto ToDto(Good entity)
    {
        throw new NotImplementedException();
    }
}