using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Oracle.ManagedDataAccess.Client;

namespace WebClient.Repositories
{
    public class DbContext : IDisposable
    {
        private readonly IDbConnection dbConnection;
        private IDbTransaction dbTransaction;
        public DbContext(string connectionString)
        {
            this.dbConnection = new OracleConnection(connectionString);
            this.dbConnection.Open();
        }

        public IDbTransaction DbTransaction => dbTransaction ??= dbConnection.BeginTransaction();

        public void Commit()
        {
            this.dbTransaction.Commit();
            this.Dispose();
        }

        public void Rollback()
        {
            this.dbTransaction.Rollback();
            this.Dispose();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await this.dbConnection.QueryFirstOrDefaultAsync<T>(sql: sql, param: param, transaction: dbTransaction, commandType: commandType ?? CommandType.Text);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null)
        {
            return await this.dbConnection.QueryAsync<T>(sql: sql, param: param, transaction: dbTransaction, commandType: commandType ?? CommandType.Text);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null)
        {
            return await this.dbConnection.ExecuteAsync(sql: sql, param: param, transaction: this.DbTransaction, commandType: commandType ?? CommandType.Text);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.dbConnection.Dispose();
                    if (this.dbTransaction != null)
                    {
                        this.dbTransaction.Dispose();
                    }
                }
                // Release unmanaged resources.
                // Set large fields to null.                
                disposed = true;
            }
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DbContext() // the finalizer
        {
            Dispose(false);
        }
    }
}
