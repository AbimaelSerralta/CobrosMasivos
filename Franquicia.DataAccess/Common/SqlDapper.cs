using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Common
{
    public static class SqlDapper
    {
        private const string _SqlConnection = @"Data Source=DESKTOP-F06LGUD\SQLEXPRESS;Initial Catalog=franquicia;User ID=sa;Password=admin123";

        public static async Task<IEnumerable<T>> ReadAllAsync<T>(string query)
        {
            IEnumerable<T> results = null;
            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                results = await connection.QueryAsync<T>(query);
            }
            return results;
        }

        public static IEnumerable<T> ReadAll<T>(string query, DynamicParameters parameters = null)
        {
            IEnumerable<T> results = null;
            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                results = connection.Query<T>(query, parameters);
            }
            return results;
        }

        public static int Create<T>(T newObject)
        {
            int? result = null;

            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                result = connection.Insert<T>(newObject);
            }

            return result == null ? -1 : result.Value;
        }

        public static async Task<int> CreateAsync<T>(T newObject)
        {
            int? result = null;

            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                result = await connection.InsertAsync<T>(newObject);
            }

            return result == null ? -1 : result.Value;
        }

        public static int Update<T>(T toUpdate)
        {
            int result = 0;
            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                result = connection.Update<T>(toUpdate);
            }
            return result;
        }

        public static async Task<int> UpdateAsync<T>(T toUpdate)
        {
            int? result = null;
            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                result = await connection.UpdateAsync<T>(toUpdate);
            }
            return result == null ? -1 : result.Value;
        }

        public static bool Delete<T>(T toDelete)
        {
            try
            {
                int result = 0;
                using (IDbConnection connection = new SqlConnection(_SqlConnection))
                {
                    result = connection.Delete<T>(toDelete);
                }
                return result == 0 ? false : true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static T GetById<T>(string query)
        {
            T result;

            using (IDbConnection connection = new SqlConnection(_SqlConnection))
            {
                result = connection.QuerySingle<T>(query);
            }

            return result;
        }
    }
}
