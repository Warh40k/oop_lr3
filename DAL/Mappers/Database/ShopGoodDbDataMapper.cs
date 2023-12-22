using DAL.Entities.ShopGoods;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Database;

public class ShopGoodDbDataMapper : IShopGoodDataMapper
{
    private const string TableName = "goods-shops";
    
    public IEnumerable<ShopGoodDto> GetAll(string statement="")
    {
        List<ShopGoodDto> shopgoods = new List<ShopGoodDto>();
        var con = DbConnection.GetConnection();
        const string insertQuery = $"SELECT * FROM \"{TableName}\"";
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

    public IEnumerable<ShopGoodDto> GetGoodsFromShop(ShopGoodDto Shop)
    {
        throw new NotImplementedException();
    }

    public void Update(ShopGoodDto entity)
    {
        var shopgood = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        int num = 0;
        const string insertQuery = $"UPDATE goods-shops SET id_good = @good, " +
                                   $"id_shop = @shop, " +
                                   $"price = @price, " +
                                   $"in_stock = @in_stock " +
                                   $"WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        
        insertCmd.Parameters.AddWithValue("@good", shopgood.GoodId);
        insertCmd.Parameters.AddWithValue("@id_shop", shopgood.ShopId);
        insertCmd.Parameters.AddWithValue("@price", shopgood.Price);
        insertCmd.Parameters.AddWithValue("@in_stock", shopgood.InStock);

        num = insertCmd.ExecuteNonQuery();
        
        con.Close();
        
        Console.WriteLine($"Обновлено {num} товаров");
    }

    public ShopGoodDto? GetById(int id)
    {
        ShopGood shopgood;
        var con = DbConnection.GetConnection();
        const string insertQuery = $"SELECT * FROM \"{TableName}\" WHERE id = @id";
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
        const string selectQuery = $"SELECT * FROM \"{TableName}\" WHERE id_shop = @idShop and id_good = @idGood";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        selectCmd.Parameters.AddWithValue("@idShop", shopgood.ShopId);
        selectCmd.Parameters.AddWithValue("@idGood", shopgood.GoodId);
        
        int num = 0;
        shopgood.Id = selectCmd.ExecuteScalar() as int?;
        if (shopgood.Id == null)
        {
            const string insertQuery = $"INSERT INTO \"{TableName}\" (id_good,id_shop,price,in_stock) values(@idGood,@idShop,@price,@in_stock) RETURNING id";
            var insertCmd = new NpgsqlCommand(insertQuery, con);

            insertCmd.Parameters.AddWithValue("@idShop", shopgood.ShopId);
            insertCmd.Parameters.AddWithValue("@idGood", shopgood.GoodId);
            insertCmd.Parameters.AddWithValue("@price", shopgood.Price);
            insertCmd.Parameters.AddWithValue("@in_stock", shopgood.InStock);
            
            shopgood.Id = insertCmd.ExecuteScalar() as int?;
            Console.WriteLine($"Создана связь под id {shopgood.Id}");
        }
        else
        {
            const string updateQuery =
                $"""
                UPDATE "{TableName}" 
                SET price = @price, 
                    in_stock = @in_stock 
                WHERE id = @id;
                """;
            var insertCmd = new NpgsqlCommand(updateQuery, con);
        
            insertCmd.Parameters.AddWithValue("@price", shopgood.Price);
            insertCmd.Parameters.AddWithValue("@in_stock", shopgood.InStock);
            insertCmd.Parameters.AddWithValue("@id", shopgood.Id);

            insertCmd.ExecuteNonQuery();
            Console.WriteLine($"Обновлена связь под id {shopgood.Id}");
        }
        con.Close();
        
        return shopgood.Id;
    }

    public void Delete(ShopGoodDto entity)
    {
        ShopGood shopgood = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = $"DELETE FROM \"{TableName}\" WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", shopgood.Id);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
    }

    public static ShopGood FromDto(ShopGoodDto dto)
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

    public static ShopGoodDto ToDto(ShopGood entity)
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