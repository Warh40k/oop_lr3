using DAL.Entities.ShopGoods;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Database;

public class ShopGoodsDataMapper : IDataMapper<ShopGoodDto>
{
    private const string TableName = "goods-shops";
    
    public IEnumerable<ShopGoodDto> GetAll(string statement="")
    {
        List<ShopGoodDto> shopgoods = new List<ShopGoodDto>();
        var con = DbConnection.GetConnection();
        const string insertQuery = $"SELECT * FROM {TableName}";
        var insertCmd = new NpgsqlCommand(insertQuery, con);

        con.Open();
        using (var reader = insertCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                int idIndex = reader.GetOrdinal("id");
                int idShopIndex = reader.GetOrdinal("id_shop");
                int idGoodIndex = reader.GetOrdinal("id_good");
                int inStockIndex = reader.GetOrdinal("in_stock");
                int priceIndex = reader.GetOrdinal("price");

                while (reader.Read())
                {
                    var shopgood = new ShopGood
                    {
                        Id = reader.GetInt32(idIndex),
                        GoodId = reader.GetInt32(idGoodIndex),
                        ShopId = reader.GetInt32(idShopIndex),
                        Price = reader.GetDecimal(priceIndex),
                        InStock = reader.GetInt32(inStockIndex)
                    };
                    shopgoods.Add(ToDto(shopgood));
                }
            }
        }
        con.Close();
        
        return shopgoods;
    }

    public ShopGoodDto? GetById(int id)
    {
        ShopGood shopgood;
        var con = DbConnection.GetConnection();
        const string insertQuery = $"SELECT * FROM {TableName} WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", id);

        con.Open();
        using (var reader = insertCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                int idIndex = reader.GetOrdinal("id");
                int idShopIndex = reader.GetOrdinal("id_shop");
                int idGoodIndex = reader.GetOrdinal("id_good");
                int inStockIndex = reader.GetOrdinal("in_stock");
                int priceIndex = reader.GetOrdinal("price");

                reader.Read();
                shopgood = new ShopGood
                {
                    Id = reader.GetInt32(idIndex),
                    GoodId = reader.GetInt32(idGoodIndex),
                    ShopId = reader.GetInt32(idShopIndex),
                    Price = reader.GetDecimal(priceIndex),
                    InStock = reader.GetInt32(inStockIndex)
                };
            }
            else
                return null;
        }
        con.Close();
        return ToDto(shopgood);
    }

    public int? Save(ShopGoodDto entity)
    {
        ShopGood shopgood = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        const string selectQuery = $"SELECT * FROM {TableName} WHERE shopname = @name";
        const string insertQuery = $"INSERT INTO {TableName}(id_good,id_shop,price,in_stock) values(@idGood,@idShop,@price,@in_stock) RETURNING id";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        selectCmd.Parameters.AddWithValue("@idGood", shopgood.GoodId);
        
        insertCmd.Parameters.AddWithValue("@idShop", shopgood.ShopId);
        insertCmd.Parameters.AddWithValue("@price", shopgood.Price);
        insertCmd.Parameters.AddWithValue("@in_stock", shopgood.InStock);
        int num = 0;
        bool isExist = selectCmd.ExecuteScalar() != null;
        if (!isExist)
            shopgood.Id = insertCmd.ExecuteScalar() as int?;
        con.Close();
        
        Console.WriteLine($"Записан {shopgood.Id} товаров");
        return shopgood.Id;
    }

    public void Delete(ShopGoodDto entity)
    {
        ShopGood shopgood = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = $"DELETE FROM {TableName} WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", shopgood.Id);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
    }

    public ShopGood FromDto(ShopGoodDto dto)
    {
        return new ShopGood
        {
            Id = dto.Id,
            GoodId = dto.GoodId,
            ShopId = dto.ShopId,
            InStock = dto.InStock,
            Price = dto.Price,
        };
    }

    public ShopGoodDto ToDto(ShopGood entity)
    {
        return new ShopGoodDto
        {
            Id = entity.Id,
            GoodId = entity.GoodId,
            ShopId = entity.ShopId,
            InStock = entity.InStock,
            Price = entity.Price,
        };
    }
}