using System.Data.Common;
using DAL.Entities.Shop;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Csv;

public class ShopJsonDataMapper : IShopDataMapper
{
    public IEnumerable<ShopDto> GetAll(string statement="")
    {
        var shops = new List<ShopDto>();
        foreach (var good in JsonData.Shops)
        {
            shops.Add(ToDto(good));
        }

        return shops;
    }

    public ShopDto GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int? Save(ShopDto entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(ShopDto entity)
    {
        throw new NotImplementedException();
    }

    public Shop FromDto(ShopDto dto)
    {
        throw new NotImplementedException();
    }

    public ShopDto ToDto(Shop entity)
    {
        throw new NotImplementedException();
    }
}