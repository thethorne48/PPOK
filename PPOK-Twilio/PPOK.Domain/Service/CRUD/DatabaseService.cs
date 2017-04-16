using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using static PPOK.Domain.Utility.Config;

namespace PPOK.Domain.Service
{
    public abstract class DatabaseService : IDisposable
    {
        //Override this to redirect the database for testing
        private static Func<IDatabaseInterface> InterfaceFactory = () => DatabaseConnectionPool.Grab();
        private static Action<IDatabaseInterface> InterfaceDisposer = conn => DatabaseConnectionPool.Release(conn);

        public static void RedirectDatabase(Func<IDatabaseInterface> factory, Action<IDatabaseInterface> disposer=null)
        {
            InterfaceFactory = factory;
            InterfaceDisposer = disposer;
        }

        public IDatabaseInterface conn;

        public bool IsDisposed
        {
            get { return conn == null; }
        }

        public DatabaseService()
        {
            conn = InterfaceFactory();
        }

        public virtual void Dispose()
        {
            if (conn != null)
            {
                if (InterfaceDisposer != null)
                    InterfaceDisposer(conn);
                else
                    conn.Dispose();
                conn = null;
            }
        }
    }
}
