using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PPOK.Domain.Service
{
    public static class DatabaseConnectionPool
    {
        //Override this to redirect the database for testing
        public static Func<IDatabaseInterface> InterfaceFactory = () => new DefaultDatabaseInterface();

        public class DatabaseConnectionPoolEntry : IDisposable
        {
            internal int subscribers = 0;
            internal IDatabaseInterface conn;

            internal DatabaseConnectionPoolEntry(IDatabaseInterface conn)
            {
                this.conn = conn;
            }

            public void Dispose()
            {
                if (conn == null)
                    throw new ObjectDisposedException($"[{this.GetType().Name}] Service already disposed.");
                conn.Dispose();
                conn = null;
            }
        }

        private static readonly System.Timers.Timer timer = new System.Timers.Timer(1000);
        private static Stack<DatabaseConnectionPoolEntry> connectionPool = new Stack<DatabaseConnectionPoolEntry>();
        private static ThreadLocal<DatabaseConnectionPoolEntry> thread_conns = new ThreadLocal<DatabaseConnectionPoolEntry>();

        static DatabaseConnectionPool()
        {
            timer.Elapsed += (sender, e) =>
            {
                if (connectionPool.Count > 0)
                {
                    DatabaseConnectionPoolEntry conn = connectionPool.Pop();
                    conn.Dispose();
                }
            };
            timer.Start();
        }

        private static DatabaseConnectionPoolEntry GrabFromStack()
        {
            if (connectionPool.Count == 0)
                return new DatabaseConnectionPoolEntry(InterfaceFactory());
            else
                return connectionPool.Pop();
        }

        private static void ReleaseToStack(DatabaseConnectionPoolEntry conn)
        {
            connectionPool.Push(conn);
        }

        public static IDatabaseInterface Grab()
        {
            var container = thread_conns.Value;
            if (container == null)
                container = thread_conns.Value = GrabFromStack();
            container.subscribers++;
            return container.conn;
        }

        public static void Release(IDatabaseInterface conn)
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
    }
}
