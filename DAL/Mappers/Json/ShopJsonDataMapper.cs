using DAL.Entities.Shop;
using DAL.Interfaces;
using DTOs;

namespace DAL.Mappers.Json;

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
        var shop = FromDto(entity);
        Shop existedGood = null;
        int? lastId = 0;
        if (JsonData.Goods.Count != 0)
        {
            var last = JsonData.Shops.Last();
            lastId = last.Id;
        }
        shop.Id = lastId + 1;
        JsonData.Shops.Add(shop);
        JsonData.Save();
        return shop.Id;
    }

    public void Delete(ShopDto entity)
    {
        var good = FromDto(entity);
        var search = JsonData.Goods.FirstOrDefault(obj => obj.Id == entity.Id);
        if (search != null)
            JsonData.Goods.Remove(search);
    }

    public static Shop FromDto(ShopDto dto)
    {
        return new Shop
        {
            Id = dto.Id,
            Name = dto.Name,
            Address = dto.Address
        };
    }

    public static ShopDto ToDto(Shop entity)
    {
        return new ShopDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Address = entity.Address
        };
    }
}