using BlazorApp1.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace BlazorApp1.Service
{
    public class StatisticsService
    {
        private string _dbConnection = "Data Source=db\\test.db";

        /// <summary>
        /// 获取每月的积分数
        /// </summary>
        /// <returns></returns>
        public List<double> GetBarData()
        {
            var connection = new SqliteConnection(_dbConnection);
            var sql = @"select * from score";
            var data = connection.Query<UserIntergral>(sql).ToList();
            data.ForEach(item => item.DisplayCreateTime = DateTimeOffset.FromUnixTimeSeconds(item.UpdateTime).DateTime);
            var mounthData = data.GroupBy(item => item.DisplayCreateTime.Month).Select(item => (item.Key, item.ToList().Sum(a => a.Intergral))).ToDictionary(a=>a.Item1);

            var result = new List<double>();
            for (int i = 1; i <= 12; i++)
            {
                if (mounthData.ContainsKey(i))
                {
                    result.Add((double)mounthData[i].Item2);
                }
                else
                {
                    result.Add(0);
                }
            }

            return result;
        } 
    }
}
