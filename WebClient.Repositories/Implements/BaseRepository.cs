using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Oracle.ManagedDataAccess.Client;
using WebClient.Core;
using WebClient.Core.Helper;
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

                var propertyId = typeof(TEntity).GetProperties().FirstOrDefault(x => Attribute.IsDefined(x, typeof(ExplicitKeyAttribute)));

                var query = string.Format("select * from {0} where {1} = :id", tableName, propertyId.Name);
                if (checkTinhTrang)
                {
                    query += " AND tinh_trang = 1";
                }

                return await this.dbContext.QueryFirstOrDefaultAsync<TEntity>(query, param: new
                {
                    id
                }, commandType: CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            return await AddObjectAsync(entity);
        }

        public async Task<T> AddObjectAsync<T>(T entity)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                var properties = typeof(T).GetProperties();
                var pars = new List<string>();
                var returns = new List<string>();
                foreach (var p in properties)
                {
                    if (Attribute.IsDefined(p, typeof(ComputedAttribute)))
                    {
                        continue;
                    }

                    if (Attribute.IsDefined(p, typeof(ExplicitKeyAttribute)) || Attribute.IsDefined(p, typeof(ReturningAttribute)))
                    {
                        returns.Add(p.Name);

                        var oracleType = GenerateOracleType(p);
                        if (oracleType == OracleDbType.Varchar2)
                        {
                            dyParam.Add(p.Name, oracleType, ParameterDirection.Output, size: 20);
                        }
                        else
                        {
                            dyParam.Add(p.Name, oracleType, ParameterDirection.Output);
                        }

                        continue;
                    }

                    var val = GetPropValue(entity, p.Name);
                    if (val != null)
                    {
                        pars.Add(p.Name);

                        dyParam.Add(p.Name, GenerateOracleType(p), ParameterDirection.Input, val);
                    }
                }

                var sql = @"insert into {0}({1})values({2}){3}";
                sql = string.Format(sql,
                    GetTableName<T>(),
                    string.Join(',', pars),
                    string.Join(',', pars.Select(x => ":" + x)),
                    returns.Count == 0 ? "" : " returning " + string.Join(", ", returns) + " into " + string.Join(", ", returns.Select(x => ":" + x)));

                await this.dbContext.ExecuteAsync(sql, param: dyParam, commandType: CommandType.Text);

                foreach (var i in returns)
                {
                    var oracleParam = dyParam.GetByName(i);
                    if (oracleParam.DbType == DbType.Int64 || oracleParam.DbType == DbType.Int32 || oracleParam.DbType == DbType.Int16)
                    {
                        typeof(T).GetProperty(i).SetValue(entity, int.Parse(oracleParam.Value.ToString()));
                    }
                    else
                    {
                        typeof(T).GetProperty(i).SetValue(entity, oracleParam.Value.ToString());
                    }
                }

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }   

        public async Task UpdateAsync(TEntity entity)
        {
            await UpdateObjectAsync(entity);
        }

        public async Task UpdateObjectAsync<T>(T entity)
        {
            try
            {
                var dyParam = new OracleDynamicParameters();
                var properties = typeof(T).GetProperties();
                var pars = new List<string>();
                var idName = "";
                foreach (var p in properties)
                {
                    if (Attribute.IsDefined(p, typeof(ComputedAttribute)))
                    {
                        continue;
                    }

                    var val = GetPropValue(entity, p.Name);
                    if (val != null)
                    {
                        if (!Attribute.IsDefined(p, typeof(ExplicitKeyAttribute)))
                        {
                            pars.Add(p.Name + "=:" + p.Name);
                        }
                        else
                        {
                            idName = p.Name;
                        }
                        dyParam.Add(p.Name, GenerateOracleType(p), ParameterDirection.Input, val);
                    }
                    else
                    {
                        pars.Add(p.Name + "=null");
                    }
                }

                var sql = "update {0} set {1} where {2}";
                sql = string.Format(sql, GetTableName<T>(), string.Join(',', pars), idName + "=:" + idName);

                await this.dbContext.ExecuteAsync(sql, param: dyParam, commandType: CommandType.Text);
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

        private static object GetPropValue<T>(T src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static OracleDbType GenerateOracleType(PropertyInfo p)
        {
            OracleDbType dbType = OracleDbType.Varchar2;

            if (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?)
                || p.PropertyType == typeof(long) || p.PropertyType == typeof(long?))
            {
                dbType = OracleDbType.Int64;
            }
            else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
            {
                dbType = OracleDbType.Date;
            }

            return dbType;
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
