using DAL.Entities.Good;
using DAL.Interfaces;
using DTOs;
using Npgsql;

namespace DAL.Mappers.Database;

public class GoodDbDataMapper : IDataMapper<GoodDto>
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