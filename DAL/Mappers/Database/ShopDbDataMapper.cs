using DAL.Interfaces;
using Npgsql;
using DAL.Entities.Shop;
using DTOs;

namespace DAL.Mappers.Database;

public class ShopDbDataMapper : IShopDataMapper
{
    private const string TableName = "shops";
    public IEnumerable<ShopDto> GetAll(string statement="")
    {
        List<ShopDto> shops = new List<ShopDto>();
        var con = DbConnection.GetConnection();
        string insertQuery = $"SELECT * FROM {TableName} {statement}";
        var insertCmd = new NpgsqlCommand(insertQuery, con);

        con.Open();
        using (var reader = insertCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                int idIndex = reader.GetOrdinal("id");
                int nameIndex = reader.GetOrdinal("shopname");
                int addressIndex = reader.GetOrdinal("address");

                while (reader.Read())
                {
                    var shop = new Shop
                    {
                        Id = reader.GetInt32(idIndex),
                        Name = reader.GetString(nameIndex),
                        Address = reader.GetString(addressIndex)
                    };
                    shops.Add(ToDto(shop));
                }
            }
        }
        con.Close();
        
        return shops;
    }

    public IEnumerable<ShopDto> GetGoodsFromShop(int shopId)
    {
        throw new NotImplementedException();
    }

    public void Update(ShopDto entity)
    {
        var shop = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        int num = 0;
        const string insertQuery = $"UPDATE {TableName} SET good_name = @name WHERE good_id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        
        insertCmd.Parameters.AddWithValue("@name", shop.Name);
        insertCmd.Parameters.AddWithValue("@id", shop.Id);

        num = insertCmd.ExecuteNonQuery();
        
        con.Close();
        
        Console.WriteLine($"Обновлено {num} товаров");
    }

    public ShopDto? GetById(int id)
    {
        Shop shop = new Shop();
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
                int nameIndex = reader.GetOrdinal("shopname");
                int addressIndex = reader.GetOrdinal("address");

                reader.Read();
                shop.Id = reader.GetInt32(idIndex);
                shop.Name = reader.GetString(nameIndex);
                shop.Address = reader.GetString(addressIndex);
            }
            else
                return null;
        }
        con.Close();
        return ToDto(shop);
    }

    public void Delete(ShopDto entity)
    {
        var shop = FromDto(entity);

        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = $"DELETE FROM {TableName} WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", shop.Id);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
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

    public int? Save(ShopDto entity)
    {
        var shop = FromDto(entity);
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = $"INSERT INTO {TableName}(shopname,address) values(@name,@address) RETURNING id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@name", shop.Name);
        insertCmd.Parameters.AddWithValue("@address", shop.Address);
        shop.Id = insertCmd.ExecuteScalar() as int?;
        Console.WriteLine($"Товар вставлен под id {shop.Id}");
        /*else
        {
            const string updateQuery = $"UPDATE {TableName} SET shopname = @name, address = @address WHERE id = @id";
            var updateCmd = new NpgsqlCommand(updateQuery, con);
            updateCmd.Parameters.AddWithValue("@name", shop.Name);
            updateCmd.Parameters.AddWithValue("@address", shop.Address);
            updateCmd.ExecuteNonQuery();
            Console.WriteLine($"Товар обновлен под id {shop.Id}");
        }*/
        con.Close();
        
        return shop.Id;
    }
}