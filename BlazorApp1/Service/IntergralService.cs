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
            var sql = @"INSERT INTO score (name,phone,intergral,createtime,updatetime) VALUES(@name,@phone,@intergral,@createTime,@updateTime)";

            connection.Execute(sql, new { name = userIntergral.Name, phone = userIntergral.Phone, intergral = userIntergral.Intergral, createTime = userIntergral.CreateTime, updateTime = userIntergral.UpdateTime });

        }

        public void Update(UserIntergral userIntergral)
        {
            using var connection = new SqliteConnection(_dbConnection);
            connection.Open();
            using var trans = connection.BeginTransaction();

            var sql = @"
                    INSERT into history (name,phone,intergral,createtime,userid)
                    SELECT name,phone,intergral,updatetime,id FROM score
                    WHERE id = @id";
            connection.Execute(sql, new { id = userIntergral.Id });

            sql = @"update score set name=@name, phone= @phone, intergral = @intergral, updateTime=@updateTime where id =@id";
            connection.Execute(sql, new { name = userIntergral.Name, phone = userIntergral.Phone, intergral = userIntergral.Intergral, updateTime = userIntergral.UpdateTime, id = userIntergral.Id });
            
            trans.Commit();
        }

        public List<UserIntergral> Get(int pageIndex, int pageSize = 10)
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"SELECT * FROM score LIMIT @limit OFFSET @offset";
            var result = connection.Query<UserIntergral>(sql, new { limit = pageSize, offset = (pageIndex - 1) * pageSize });
            return result.ToList();
        }

        public int GetAllCount()
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"SELECT count(*) FROM score";
            var result = connection.ExecuteScalar<int>(sql);
            return result;
        }

        public List<UserIntergral> GetAll()
        {
            using var connection = new SqliteConnection(_dbConnection);
            var sql = @"SELECT * FROM score order by updatetime desc";
            var result = connection.Query<UserIntergral>(sql);
            return result.ToList();
        }

    }
}
