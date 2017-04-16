using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Dapper;
using PPOK.Domain.Utility;

namespace PPOK.Domain.Service
{
    public class DefaultDatabaseInterface : IDatabaseInterface
    {
        private IDbConnection conn;

        public DefaultDatabaseInterface()
        {
            conn = new SqlConnection(Config.DBConnectionString);
        }

        public void Dispose()
        {
            if(conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }

        public void Execute(string sql, object parameters)
        {
            conn.Execute(sql, parameters);
        }

        public IEnumerable<dynamic> Query(string sql, object parameters)
        {
            return conn.Query(sql, parameters);
        }
    }
}
