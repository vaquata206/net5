using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using WebClient.Core.Helpers;
using WebClient.Repositories.Interfaces;

namespace WebClient.Repositories.Implements
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : class
    {
        protected readonly DbContext dbContext;
        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private bool isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<TEntity> GetByIdAsync(int id, bool checkTinhTrang = true)
        {
            try
            {
                var tableName = GetTableName<TEntity>();

                var sql = string.Format("select * from {0} where id = @id", tableName);
                if (checkTinhTrang)
                {
                    sql += " AND DaXoa = @daXoa";
                }

                return await this.dbContext.QueryFirstOrDefaultAsync<TEntity>(
                    sql: sql,
                    param: new
                    {
                        id = id,
                        daXoa = Constants.TrangThai.ChuaXoa
                    },
                    commandType: CommandType.Text
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            await this.dbContext.InsertAsync(entity);
            return entity;
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            await this.dbContext.UpdateAsync(entity);
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            await this.dbContext.DeleteAsync(entity);
        }

        public async Task<string> TaoMa(string baseCode)
        {
            try
            {
                var tableName = GetTableName<TEntity>();
                var yearNumber = (DateTime.Now.Year % 100).ToString();

                var sql = string.Format("select top(1) Ma from {0} where Ma like @baseCode + '%' order by NgayKhoiTao desc", tableName);

                var lastCode = await this.dbContext.QueryFirstOrDefaultAsync<string>(
                    sql: sql,
                    param: new
                    {
                        baseCode = baseCode + "_" + yearNumber + "_"
                    },
                    commandType: CommandType.Text
                );

                if (string.IsNullOrEmpty(lastCode))
                {
                    return baseCode + "_" + yearNumber + "_0001";
                }
                else
                {
                    return baseCode + "_" + yearNumber + "_" + (int.Parse(lastCode.Split("_").ElementAt(2)) + 1).ToString("D4");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetTableName<T>()
        {
            var table = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));
            return (table == null) ? typeof(T).Name : table.Name;
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                // free managed resources
                if (this.dbContext != null)
                {
                    this.dbContext.Dispose();
                }
            }

            isDisposed = true;
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't
        // own unmanaged resources, but leave the other methods
        // exactly as they are.
        ~BaseRepository()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
