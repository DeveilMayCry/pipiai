using BlazorApp1.Data;
using Microsoft.Data.Sqlite;
using Dapper;

namespace BlazorApp1.Service
{
    public class IntergralService
    {
        private string _dbConnection = "Data Source=db\\test.db";

        public void Insert(UserIntergral userIntergral)
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"INSERT INTO score (name,phone,intergral) VALUES(@name,@phone,@intergral)";

            connection.Execute(sql, userIntergral);
           
        }

        public void Update(UserIntergral userIntergral)
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"update score set name=@name, phone= @phone, intergral = @intergral where id =@id)";

            connection.Execute(sql, userIntergral);
        }

        public List<UserIntergral> Get(int pageIndex, int pageSize = 10)
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"SELECT * FROM score LIMIT @limit OFFSET @offset";
            var result = connection.Query<UserIntergral>(sql, new { limit = pageSize, offset = (pageIndex - 1) * pageSize });
            return result.ToList();
        }
    }
}
