using FWW.Framework.DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityCacheInfo
    {
        private Type _tableType;
        public Type TableType { get { return _tableType; } }
        public EntityCacheInfo(Type tableType)
        {
            _fields = new List<Field>();
            _primarykeys = new List<string>();
            _tableType = tableType;
        }
        #region Fields
        /// <summary>
        /// 自动增长字段
        /// </summary>
        public string IdentityField { get; set; }
        private List<Field> _fields;
        /// <summary>
        /// 字段
        /// </summary>
        public List<Field> Fields { get { return _fields; } }
        /// <summary>
        /// 返回第一个字段
        /// </summary>
        /// <returns></returns>
        public Field GetFirstField()
        {
            return _fields.FirstOrDefault();
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimiaryKey { get; set; }
        private List<string> _primarykeys;
        public List<string> PrimaryKeys { get { return _primarykeys; } }
        /// <summary>
        /// 主键作为条件
        /// </summary>
        public Func<object, string> ParimaryKeyWhere { get; set; }
        /// <summary>
        /// 新增
        /// </summary>
        public Func<object, string> InsertDeserializer { get; set; }
        /// <summary>
        /// 更新
        /// </summary>
        public Func<object, string> GetUpdateByPrimaryKeysFunc { get; set; }
        /// <summary>
        /// 设置更新字段
        /// </summary>
        public Action<object, string> SetUpdateInfo { get; set; }
        /// <summary>
        /// 设置创建信息字段
        /// </summary>
        public Action<object, string> SetCreateInfo { get; set; }

        /// <summary>
        /// 更新
        /// </summary>
        public Func<object, SqlDataParameter, string> GetUpdateByWhereFunc { get; set; }

        /// <summary>
        /// 简单条件语句，全部是等于
        /// </summary>
        public Func<object, string> SimpleWhereDeserializer { get; set; }
        #endregion

        #region Update
        /// <summary>
        /// 获取要更新的字段
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetUpdateFieldsSql(object obj)
        {
            var upsql = GetUpdateByPrimaryKeysFunc(obj);

            return upsql;
        }
        /// <summary>
        /// 根据主键更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetUpdateByPrimaryKey(object obj, string extendup = "")
        {
            var primaryKeyWhere = ParimaryKeyWhere(obj);
            if (string.IsNullOrEmpty(primaryKeyWhere) == false)
            {
                var upsql = GetUpdateFieldsSql(obj);
                if (string.IsNullOrEmpty(upsql))
                {
                    if (!string.IsNullOrEmpty(extendup))
                    {
                        upsql += extendup;
                    }
                    else
                        throw new Exception("没有可以更新的值");
                }
                else
                {
                    if (!string.IsNullOrEmpty(extendup))
                    {
                        upsql += "," + extendup;
                    }
                }
                return EntityCache.CreateUpdateSQL(TableName, upsql, primaryKeyWhere);
            }
            else
                throw new Exception(string.Format("表{0}主键{1}在做根据主键更新时必须赋值", TableName, PrimiaryKey));
        }
        /// <summary>
        /// Gets the update by where.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="where">The where.</param>
        /// <param name="sqldataparameters"></param>
        /// <param name="extendup">The extendup.</param>
        /// <returns>System.String.</returns>
        public string GetUpdateByWhere(object obj, string where,SqlDataParameter sqldataparameters, string extendup = "")
        {
            var upsql =GetUpdateByWhereFunc(obj, sqldataparameters);
            if (string.IsNullOrEmpty(upsql))
            {
                if (!string.IsNullOrEmpty(extendup))
                {
                    upsql += extendup;
                }
                else
                    throw new Exception("没有可以更新的值");
            }
            else
            {
                if (!string.IsNullOrEmpty(extendup))
                {
                    upsql += "," + extendup;
                }
            }
            return EntityCache.CreateUpdateSQL(TableName, upsql, where);
        }
        /// <summary>
        /// Gets the update by where.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="where">The where.</param>
        /// <param name="extendup">The extendup.</param>
        /// <returns>System.String.</returns>
        public string GetUpdateByWhere(object obj, string where, string extendup = "")
        {
            var upsql = GetUpdateFieldsSql(obj);
            if (string.IsNullOrEmpty(upsql))
            {
                if (!string.IsNullOrEmpty(extendup))
                {
                    upsql += extendup;
                }
                else
                    throw new Exception("没有可以更新的值");
            }
            else
            {
                if (!string.IsNullOrEmpty(extendup))
                {
                    upsql += "," + extendup;
                }
            }
            return EntityCache.CreateUpdateSQL(TableName, upsql, where);
        }
        #endregion

        #region Insert

        /// <summary>
        /// Gets the insert SQL.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public string GetInsertSql(object obj)
        {
            var insertsql = InsertDeserializer(obj);
            return EntityCache.CreateInsertSQL(TableName, insertsql, IdentityField);
        }
        public string GetInsertNoReturnAutoIDSql(object obj)
        {
            var insertsql = InsertDeserializer(obj);
            return EntityCache.CreateInsertSQLWithNoReturn(TableName, insertsql, IdentityField);
        }
        #endregion

        #region Select
        public string GeneratorSelectByPrimaryKey(object obj, string displayfields = "*", bool withnolock = false)
        {
            var primaryKeyWhere = ParimaryKeyWhere(obj);
            if (string.IsNullOrEmpty(primaryKeyWhere))
            {
                throw new Exception("根据主键查询时必须为主键赋值");
            }
            return EntityCache.CreateSelectSQL(TableName, displayfields, primaryKeyWhere, "", 0, false, withnolock);
        }
        /// <summary>
        /// Generators the simple select SQL.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="displayfields">The displayfields.</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        public string GeneratorSimpleSelectSql(object model, string displayfields = "*", string sort = "", int count = 0, bool withnolock = false)
        {
            var where = SimpleWhereDeserializer(model);
            return EntityCache.CreateSelectSQL(TableName, displayfields, where, sort, count, false, withnolock);
        }
        #endregion
    }
}
