using DAL.Entities.Shop;
using DTOs;

namespace DAL.Interfaces;

public interface IGoodDataMapper : IDataMapper<GoodDto>
{
    IEnumerable<GoodDto> GetGoodsFromShop(int? shopId, string where = "", int? value = -1);
    void AddGoodToShop(int? shopId, GoodDto goodDto);
    void DeleteGoodFromShop(int shopId, GoodDto goodDto);
    public int FindCheapestShop(GoodDto goodDto);
    IEnumerable<GoodDto> GetGoodsForBudget(int? shopId, int budget);
    public bool BuyGoods(int? shopId, GoodDto goodDto, int quantity);
}