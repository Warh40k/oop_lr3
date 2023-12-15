using DAL.Interfaces;
using Npgsql;
using DAL.Entities.Shop;
using DTOs;

namespace DAL.Mappers.Database;

public class ShopDbDataMapper : IDataMapper<ShopDto>
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

    public Shop FromDto(ShopDto dto)
    {
        return new Shop
        {
            Id = dto.Id,
            Name = dto.Name,
            Address = dto.Address
        };
    }

    public ShopDto ToDto(Shop entity)
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
        const string selectQuery = $"SELECT * FROM {TableName} WHERE shopname = @name";
        const string insertQuery = $"INSERT INTO {TableName}(shopname,address) values(@name,@address) RETURNING id";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        selectCmd.Parameters.AddWithValue("@name", shop.Name);
        
        insertCmd.Parameters.AddWithValue("@name", shop.Name);
        insertCmd.Parameters.AddWithValue("@address", shop.Address);
        int num = 0;
        bool isExist = selectCmd.ExecuteScalar() != null;
        if (!isExist)
            shop.Id = insertCmd.ExecuteScalar() as int?;
        con.Close();
        
        Console.WriteLine($"Записан {shop.Id} товаров");
        return shop.Id;
    }
}