using DAL.Entities.Good;
using DAL.Entities.ShopGoods;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Database;

public class GoodDbDataMapper : IGoodDataMapper
{
    private const string TableName = "goods";
    public IEnumerable<GoodDto> GetAll(string statement="")
    {
        List<GoodDto> goods = new List<GoodDto>();
        var con = DbConnection.GetConnection();
        string insertQuery = $"SELECT * FROM {TableName} {statement}";
        var insertCmd = new NpgsqlCommand(insertQuery, con);

        con.Open();
        using (var reader = insertCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                int idIndex = reader.GetOrdinal("good_id");
                int nameIndex = reader.GetOrdinal("good_name");

                while (reader.Read())
                {
                    var good = new Good
                    {
                        Id = reader.GetInt32(idIndex),
                        Name = reader.GetString(nameIndex)
                    };
                    goods.Add(ToDto(good));
                }
            }
        }
        con.Close();
        
        return goods;
    }

    public IEnumerable<GoodDto> GetGoodsFromShop(int? shopId)
    {
        var con = DbConnection.GetConnection();
        List<GoodDto> goods = new List<GoodDto>();
        string query = """
                       SELECT good_id, good_name, price, in_stock
                       FROM goods
                       INNER JOIN "goods-shops" as gs ON gs.id_shop = @id_shop
                       """;
        var cmd = new NpgsqlCommand(query, con);
        cmd.Parameters.AddWithValue("@id_shop", shopId);
        var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            int idIndex = reader.GetOrdinal("good_id");
            int nameIndex = reader.GetOrdinal("good_name");
            int priceIndex = reader.GetOrdinal("price");
            int inStockIndex = reader.GetOrdinal("in_stock");

            while (reader.Read())
            {
                Good good = new Good();
                good.Id = reader.GetInt32(idIndex);
                good.Name = reader.GetString(nameIndex);
                good.Price = reader.GetDecimal(priceIndex);
                good.Quantity = reader.GetInt32(inStockIndex);
                goods.Add(ToDto(good));
            }
        }

        return goods;
    }

    public void AddGoodToShop(int shopId, GoodDto goodDto)
    {
        var shopGoodMapper = new ShopGoodDbDataMapper();
        var good = FromDto(goodDto);
        var shopgood = new ShopGoodDto
        {
            GoodId = good.Id,
            ShopId = shopId,
            Price = good.Price,
            InStock = good.Quantity
        };
        shopGoodMapper.Save(shopgood);
        Save(ToDto(good));
    }

    public void DeleteGoodFromShop(int shopId, GoodDto goodDto)
    {
        var shopGoodMapper = new ShopGoodDbDataMapper();
        var shopGoodDto = new ShopGoodDto
        {
            GoodId = goodDto.Id,
            ShopId = shopId,
            Price = goodDto.Price,
            InStock = goodDto.Quantity
        };
        
        shopGoodMapper.Delete(shopGoodDto);
    }

    public GoodDto? GetById(int id)
    {
        Good good = new Good();
        var con = DbConnection.GetConnection();
        const string insertQuery = $"SELECT * FROM {TableName} WHERE good_id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", id);

        con.Open();
        using (var reader = insertCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                int idIndex = reader.GetOrdinal("good_id");
                int nameIndex = reader.GetOrdinal("good_name");

                reader.Read();
                good.Id = reader.GetInt32(idIndex);
                good.Name = reader.GetString(nameIndex);
            }
            else
                return null;
        }
        con.Close();
        return ToDto(good);
    }

    public void Update(GoodDto entity)
    {
        var good = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        var isExist = CheckRowExist(con, good);
        int num = 0;
        if (isExist == null)
        {
            const string insertQuery = $"UPDATE {TableName} SET good_name = @name WHERE good_id = @id";
            var insertCmd = new NpgsqlCommand(insertQuery, con);
            
            insertCmd.Parameters.AddWithValue("@name", good.Name);
            insertCmd.Parameters.AddWithValue("@id", good.Id);
    
            num = insertCmd.ExecuteNonQuery();
        }
        
        con.Close();
        
        Console.WriteLine($"Обновлено {num} товаров");
    }

    private static int? CheckRowExist(NpgsqlConnection con, Good good)
    {
        const string selectQuery = $"SELECT * FROM {TableName} WHERE good_name = @name";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        selectCmd.Parameters.AddWithValue("@name", good.Name);

        return selectCmd.ExecuteScalar() as int?;
    }

    public void Delete(GoodDto entity)
    {
        var good = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = $"DELETE FROM {TableName} WHERE good_name = @name";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@name", good.Name);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
    }

    public Good FromDto(GoodDto dto)
    {
        return new Good
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public GoodDto ToDto(Good entity)
    {
        return new GoodDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public int? Save(GoodDto entity)
    {
        Good good = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        good.Id = CheckRowExist(con, good);
        if (good.Id == null)
        {
            const string insertQuery = $"INSERT INTO {TableName}(good_name) values(@name) RETURNING good_id";
            var insertCmd = new NpgsqlCommand(insertQuery, con);
            insertCmd.Parameters.AddWithValue("@name", good.Name);
            good.Id = insertCmd.ExecuteScalar() as int?;
        }
        con.Close();
        
        Console.WriteLine($"Записан {good.Id} товаров");
        return good.Id;
    }
}