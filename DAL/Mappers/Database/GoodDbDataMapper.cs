using DAL.Entities.Good;
using DAL.Interfaces;
using Npgsql;

namespace DAL.Mappers.Database;

public class GoodDbDataMapper : IDataMapper<Good>
{
    public IEnumerable<Good>? GetAll()
    {
        List<Good> goods = new List<Good>();
        var con = DbConnection.GetConnection();
        const string insertQuery = "SELECT * FROM goods";
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
                    goods.Add(good);
                }
            }
            else
                return null;
        }
        con.Close();
        
        return goods;
    }

    public Good? GetById(int id)
    {
        Good good = new Good();
        var con = DbConnection.GetConnection();
        const string insertQuery = "SELECT * FROM goods WHERE good_id = @id";
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
        return good;
    }

    public void Update(Good entity)
    {
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = "UPDATE goods SET good_name = @name WHERE good_id = @id";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@name", entity.Name);
        insertCmd.Parameters.AddWithValue("@id", entity.Id);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Обновлено {num} товаров");
    }

    public void Delete(Good entity)
    {
        var con = DbConnection.GetConnection();
        con.Open();
        const string insertQuery = "DELETE FROM goods WHERE good_name = @name";
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        insertCmd.Parameters.AddWithValue("@name", entity.Name);
        int num = insertCmd.ExecuteNonQuery();
        con.Close();
        
        Console.WriteLine($"Удалено {num} товаров");
    }

    public void Save(Good entity)
    {
        var con = DbConnection.GetConnection();
        con.Open();
        const string selectQuery = "SELECT * FROM goods WHERE good_name = @name";
        const string insertQuery = "INSERT INTO goods(good_name) values(@name) RETURNING good_id";
        var selectCmd = new NpgsqlCommand(selectQuery, con);
        var insertCmd = new NpgsqlCommand(insertQuery, con);
        selectCmd.Parameters.AddWithValue("@name", entity.Name);
        insertCmd.Parameters.AddWithValue("@name", entity.Name);
        int num = 0;
        bool isExist = selectCmd.ExecuteScalar() != null;
        if (!isExist)
            entity.Id = insertCmd.ExecuteScalar() as int?;
        con.Close();
        
        Console.WriteLine($"Записан {entity.Id} товаров");
    }
}