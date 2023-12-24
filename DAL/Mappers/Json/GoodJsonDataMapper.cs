using DAL.Entities.Good;
using DAL.Entities.Shop;
using DAL.Entities.ShopGoods;
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
        int? lastId = 0;
        if (JsonData.Goods.Count != 0)
        {
            var last = JsonData.Goods.Last();
            lastId = last.Id;
        }
        good.Id = lastId + 1;
        JsonData.Goods.Add(good);
        JsonData.Save();
        return good.Id;
    }

    public void Delete(GoodDto entity)
    {
        var good = FromDto(entity);
        var search = JsonData.Goods.FirstOrDefault(obj => obj.Id == good.Id);
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
        var good = FromDto(goodDto);
        var extShopGood = JsonData.ShopGoods.FirstOrDefault(obj => obj.GoodId == good.Id && obj.ShopId == shopId, null);
        if (extShopGood == null)
        {
            var shopGood = new ShopGood
            {
                GoodId = good.Id,
                ShopId = shopId,
                Price = good.Price,
                InStock = good.Quantity
            };
            
            int? lastId = 0;
            if (JsonData.ShopGoods.Count != 0)
            {
                var last = JsonData.ShopGoods.Last();
                lastId = last.Id;
            }
            shopGood.Id = lastId + 1;
            JsonData.ShopGoods.Add(shopGood);
            JsonData.Save();
        }
        else
        {
            extShopGood.Price = good.Price;
            extShopGood.InStock = good.Quantity;
            JsonData.Save();
        }
    }

    public void DeleteGoodFromShop(int shopId, GoodDto goodDto)
    {
        throw new NotImplementedException();
    }

    public int FindCheapestShop(GoodDto goodDto)
    {
        var good = FromDto(goodDto);
        var cheapest = from sg in JsonData.ShopGoods
            join s in JsonData.Shops on sg.ShopId equals s.Id
            where sg.GoodId == good.Id
            orderby sg.Price
            select new Shop
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address
            };
        return (int)cheapest.First().Id;
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