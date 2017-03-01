using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using static PPOK.Domain.Utility.Config;

namespace PPOK.Domain.Service
{
    internal class DBConnection : IDisposable
    {
        private static readonly System.Timers.Timer timer = new System.Timers.Timer(1000);
        private static Stack<DBConnection> connectionPool = new Stack<DBConnection>();
        private static ThreadLocal<DBConnection> thread_conns = new ThreadLocal<DBConnection>();

        static DBConnection()
        {
            timer.Elapsed += (sender, e) =>
            {
                if (connectionPool.Count > 0)
                {
                    DBConnection conn = connectionPool.Pop();
                    conn.Dispose();
                }
            };
            timer.Start();
        }

        private static DBConnection GrabFromStack()
        {
            if (connectionPool.Count == 0)
                return new DBConnection(DBConnectionString);
            else
                return connectionPool.Pop();
        }

        private static void ReleaseToStack(DBConnection conn)
        {
            connectionPool.Push(conn);
        }

        public static IDbConnection Grab()
        {
            var container = thread_conns.Value;
            if (container == null)
                container = thread_conns.Value = GrabFromStack();
            container.subscribers++;
            return container.conn;
        }

        public static void Release(IDbConnection conn)
        {
            var container = thread_conns.Value;
            if (container != null && conn == container.conn)
            {
                container.subscribers--;
                if (container.subscribers == 0)
                {
                    thread_conns.Value = null;
                    ReleaseToStack(container);
                }
            }
        }

        private int subscribers = 0;
        private IDbConnection conn;

        private DBConnection(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            if (conn == null)
                throw new ObjectDisposedException($"[{this.GetType().Name}] Service already disposed.");
            conn.Dispose();
            conn = null;
        }
    }

    public abstract class DatabaseService : IDisposable
    {
        public IDbConnection conn;

        public bool IsDisposed
        {
            get { return conn == null; }
        }

        public DatabaseService()
        {
            conn = DBConnection.Grab();
        }

        public virtual void Dispose()
        {
            if (conn != null)
            {
                DBConnection.Release(conn);
                conn = null;
            }
        }
    }
}
