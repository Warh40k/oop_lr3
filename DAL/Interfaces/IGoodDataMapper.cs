using DTOs;

namespace DAL.Interfaces;

public interface IGoodDataMapper : IDataMapper<GoodDto>
{
    IEnumerable<GoodDto> GetGoodsFromShop(int? shopId);
    void AddGoodToShop(int? shopId, GoodDto goodDto);
    void DeleteGoodFromShop(int shopId, GoodDto goodDto);
}