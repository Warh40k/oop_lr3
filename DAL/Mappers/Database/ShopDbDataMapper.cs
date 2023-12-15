using DAL.Interfaces;
using Npgsql;
using DAL.Entities.Shop;

namespace DAL.Mappers.Database;

public class ShopDbDataMapper : IDataMapper<Shop>
{
    public IEnumerable<Shop> GetAll()
    {
        List<Shop> shops = new List<Shop>();
        var con = DbConnection.GetConnection();
        const string insertQuery = "SELECT * FROM shops";
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
                    shops.Add(shop);
                }
            }
            else
                return null;
        }
        con.Close();
        
        return shops;
    }

    public Shop GetById(int id)
    {
        Shop shop = new Shop();
        var con = DbConnection.GetConnection();
        const string insertQuery = "SELECT * FROM shops WHERE id = @id";
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
        return shop;
    }

    public void Delete(Shop entity)
    {
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = "DELETE FROM shops WHERE id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@id", entity.Id);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
    }

    public void Save(Shop entity)
    {
        var con = DbConnection.GetConnection();
        con.Open();
        const string selectQuery = "SELECT * FROM shops WHERE shopname = @name";
        const string insertQuery = "INSERT INTO shops(shopname,address) values(@name,@address) RETURNING id";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        selectCmd.Parameters.AddWithValue("@name", entity.Name);
        
        insertCmd.Parameters.AddWithValue("@name", entity.Name);
        insertCmd.Parameters.AddWithValue("@address", entity.Address);
        int num = 0;
        bool isExist = selectCmd.ExecuteScalar() != null;
        if (!isExist)
            entity.Id = insertCmd.ExecuteScalar() as int?;
        con.Close();
        
        Console.WriteLine($"Записан {entity.Id} товаров");
    }
}