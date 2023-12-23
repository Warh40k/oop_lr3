using DAL.Entities.Good;
using DAL.Interfaces;
using DTOs;

namespace DAL.Mappers.Json;

public class GoodJsonDataMapper : IGoodDataMapper
{
    public IEnumerable<GoodDto> GetAll(string statement="")
    {
        var goods = new List<GoodDto>();
        
        foreach (var good in JsonData.Goods)
        {
            goods.Add(ToDto(good));
        }

        return goods;
    }

    public GoodDto GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int? Save(GoodDto entity)
    {
        var good = FromDto(entity);
        Good existedGood = null;
        int? lastId = 1;
        if (JsonData.Goods.Count != 0)
        {
            var last = JsonData.Goods.Last();
            lastId = last.Id;
        }
        good.Id = lastId;
        JsonData.Goods.Add(good);
        JsonData.Save();
        return lastId;
    }

    public void Delete(GoodDto entity)
    {
        var good = FromDto(entity);
        var search = JsonData.Goods.FirstOrDefault(obj => obj.Id == entity.Id);
        if (search != null)
            JsonData.Goods.Remove(search);
    }

    public IEnumerable<GoodDto> GetGoodsFromShop(int? shopId, string where = "", int? value = -1)
    {
        return from shopGood in JsonData.ShopGoods
            join good in JsonData.Goods on shopGood.GoodId equals good.Id
            where shopGood.ShopId == shopId
            select new GoodDto
            {
                Id = good.Id,
                Name = good.Name,
                Price = shopGood.Price,
                Quantity = shopGood.InStock
            };
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

    public bool BuyGoods(int? shopId, GoodDto good, int quantity)
    {
        throw new NotImplementedException();
    }

    public Good FromDto(GoodDto dto)
    {
        return new Good
        {
            Id = dto.Id,
            Name = dto.Name,
            Quantity = dto.Quantity,
            Price = dto.Price,
            AffordCount = dto.AffordCount
        };
    }

    public GoodDto ToDto(Good entity)
    {
        return new GoodDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Quantity = entity.Quantity,
            Price = entity.Price,
            AffordCount = entity.AffordCount
        };
    }
}