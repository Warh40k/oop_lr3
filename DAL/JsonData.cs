using System.Text.Json;
using DAL.Entities.Good;
using DAL.Entities.Shop;
using DAL.Entities.ShopGoods;

namespace DAL;

public static class JsonData
{
    public static List<Good> Goods { get; set; } = new();
    public static List<Shop> Shops { get; set; } = new();
    public static List<ShopGood> ShopGoods { get; set; } = new();

    private static string _goodsFile = "goods.json";
    private static string _shopsFile = "shops.json";
    private static string _shopsGoodsFile = "shops-goods.json";
    
    public static void Load()
    {
        if (File.Exists(_goodsFile))
            Goods = JsonSerializer.Deserialize<List<Good>>(File.ReadAllText(_goodsFile));
        if (File.Exists(_shopsFile))
            Shops = JsonSerializer.Deserialize<List<Shop>>(File.ReadAllText(_shopsFile));
        if (File.Exists(_shopsGoodsFile))
            ShopGoods = JsonSerializer.Deserialize<List<ShopGood>>(File.ReadAllText(_shopsGoodsFile));
    }

    public static void Save()
    {
        string goods = JsonSerializer.Serialize(Goods);
        string shops = JsonSerializer.Serialize(Shops);
        string shopGoods = JsonSerializer.Serialize(ShopGoods);
        
        File.WriteAllText(_goodsFile, goods);
        File.WriteAllText(_shopsFile, shops);
        File.WriteAllText(_shopsGoodsFile, shopGoods);
    }
}