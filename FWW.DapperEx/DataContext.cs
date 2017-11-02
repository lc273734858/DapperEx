using FWW.Framework.DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public class DataContext
    {
        #region Field
        /// <summary>
        /// 数据库
        /// </summary>
        private Database db;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionstringKey"></param>
        public DataContext(string connectionstringKey)
        {
            this.db = new Database(ProviderFactory.CreateDbProvider(connectionstringKey));
        }
        #endregion

        #region Method
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TableEntity"></typeparam>
        /// <returns></returns>
        public FromSection<TableEntity> From<TableEntity>(bool withNolock = false) where TableEntity : BaseEntity
        {
            return new FromSection<TableEntity>(db, withNolock);
        }
        public string LastSqlExpress { get { return db.LastSqlExpress; } }
        public object LastSqlParameters { get { return db.LastSqlParameters; } }
        #endregion

        #region Update
        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <param name="entity"></param>
        public int UpdateByPrimaryKey(object entity, string extendup = "")
        {
            var sql = EntityCache.CreateUpdateByPrimaryKey(entity, extendup);
            return db.GetConnection().Execute(sql, entity);
        }
        /// <summary>
        /// 根据条件更新实体中的字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(object entity, WhereClip where, string extendup = "")
        {
            if (entity == null)
                return 0;
            var sqldataparameters = new SqlDataParameter();
            var wheresql=where.GenerateExpress(sqldataparameters);
            var sql = EntityCache.CreateUpdateByWhere(entity, wheresql,sqldataparameters, extendup);
            return db.GetConnection().Execute(sql, sqldataparameters);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere, string extendup = "")
            where TEntity : BaseEntity
        {
            var whereclip = ExpressionToClip<TEntity>.ToWhereClip(lambdaWhere);
            return Update(entity, whereclip, extendup);
        }
        #endregion

        #region Insert
        public int Insert(object entity)
        {
            var sql = EntityCache.CreateInsertSQL(entity);
            db.LastSqlExpress = sql;
            db.LastSqlParameters = entity;
            return db.GetConnection().Execute(sql, entity);
        }
        public void InsertNoReturnAutoID(object entity)
        {
            var sql = EntityCache.CreateInsertNoReturnAutoIDSQL(entity);
            db.LastSqlExpress = sql;
            db.LastSqlParameters = entity;
            db.GetConnection().Execute(sql, entity);
        }
        #endregion

        #region Delete

        public int Delete<TEntity>(WhereClip where)
        {
            var sqldataparameters = new SqlDataParameter();
            var wheresql=where.GenerateExpress(sqldataparameters);
            var tablename = EntityCache.GetTableName<TEntity>();
            var sql = EntityCache.CreateDeleteSQL(tablename, wheresql);
            db.LastSqlExpress = sql;
            db.LastSqlParameters = sqldataparameters;
            return db.GetConnection().Execute(sql, sqldataparameters);
        }
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
        {
            var whereclip = ExpressionToClip<TEntity>.ToWhereClip(lambdaWhere);
            return Delete<TEntity>(whereclip);
        }
        public int Delete(object entity)
        {
            var sql = EntityCache.CreateDeleteByPrimaryKey(entity);
            db.LastSqlExpress = sql;
            db.LastSqlParameters = entity;
            return db.GetConnection().Execute(sql, entity);
        }
        #endregion

        #region DapperEx
        public IEnumerable<T> Excute<T>(string sql, object paramter)
        {
            return db.GetConnection().Query<T>(sql, paramter);
        }
        public IEnumerable<T> Excute<T>(string sql, Parameters paramter)
        {
            return db.GetConnection().Query<T>(sql, paramter);
        }
        public object ExecuteScalar(string sql, object paramter)
        {
            return db.GetConnection().ExecuteScalar(sql, paramter, null, null, null);
        }
        public T ExecuteScalar<T>(string sql, object paramter)
        {
            return db.GetConnection().ExecuteScalar<T>(sql, paramter, null, null, null);
        }
        #endregion

        #region 存储过程

        /// <summary>
        /// 存储过程查询
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public ProcedureSection Procedure(string procName)
        {
            return new ProcedureSection(db, procName);
        }

        #endregion
    }
}
