#pragma warning disable CS1591

using System;
using System.Collections.Generic;
using SQLitePCL.pretty;

namespace Emby.Server.Implementations.Data
{
    public sealed class ManagedConnection : IDisposable
    {
        private readonly ConnectionPool _pool;

        private SQLiteDatabaseConnection _db;

        private bool _disposed = false;

        public ManagedConnection(SQLiteDatabaseConnection db, ConnectionPool pool)
        {
            _db = db;
            _pool = pool;
        }

        public IStatement PrepareStatement(string sql)
        {
            return _db.PrepareStatement(sql);
        }

        public IEnumerable<IStatement> PrepareAll(string sql)
        {
            return _db.PrepareAll(sql);
        }

        public void ExecuteAll(string sql)
        {
            _db.ExecuteAll(sql);
        }

        public void Execute(string sql, params object[] values)
        {
            _db.Execute(sql, values);
        }

        public void RunQueries(string[] sql)
        {
            _db.RunQueries(sql);
        }

        public void RunInTransaction(Action<IDatabaseConnection> action, TransactionMode mode)
        {
            _db.RunInTransaction(action, mode);
        }

        public T RunInTransaction<T>(Func<IDatabaseConnection, T> action, TransactionMode mode)
        {
            return _db.RunInTransaction(action, mode);
        }

        public IEnumerable<IReadOnlyList<ResultSetValue>> Query(string sql)
        {
            return _db.Query(sql);
        }

        public IEnumerable<IReadOnlyList<ResultSetValue>> Query(string sql, params object[] values)
        {
            return _db.Query(sql, values);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _pool.Return(_db);

            _db = null!; // Don't dispose it
            _disposed = true;
        }
    }
}
