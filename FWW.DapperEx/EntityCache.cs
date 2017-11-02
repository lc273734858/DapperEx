using FWW.Framework.DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// Sql生成工具
    /// </summary>
    public static class EntityCache
    {
        /// <summary>
        /// 
        /// </summary>
        public static string WithNoLockStr(bool _withnolock)
        {
            return _withnolock ? "WITH(NOLOCK)" : "";
        }

        #region Select
        #region Page
        static public string CreatePageSQL(Type model, IEnumerable<string> displayFields, WhereObjectList where, ref SqlDataParameter paramters, int pageNum, int pageCount, string sort = "", bool withnolock = true)
        {
            return CreatePageSQL(model, displayFields.ToStringSeparatedByComma(), BuildWhereByWhereList(ref paramters, where), pageNum, pageCount, sort, withnolock);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="displayFields"></param>
        /// <param name="where"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageCount"></param>
        /// <param name="sort"></param>
        /// <param name="withnolock"></param>
        /// <returns></returns>
        static public string CreatePageSQL(Type model, string displayFields, string where, int pageNum, int pageCount, string sort = "", bool withnolock = true)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model);
            return CreatePageSQL(sqlinfo.TableName, displayFields, where, pageNum, pageCount, sqlinfo.PrimiaryKey, sort, withnolock);

        }
        /// <summary>
        /// 创建分页SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="displayFields">显示字段</param>
        /// <param name="where">where条件</param>
        /// <param name="pageNum">当前页码</param>
        /// <param name="pageCount">每页记录数</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="sort">排序字段</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        static public string CreatePageSQL(string tableName, string displayFields, string where, int pageNum, int pageCount, string primaryKey, string sort = "", bool withnolock = false)
        {
            //显示字段
            string dis = string.IsNullOrEmpty(displayFields) ? "*" : displayFields;
            var tablesql = string.Format(",{0} FROM {1} {2}", dis, tableName, WithNoLockStr(withnolock));
            return CreatePageSQLByCustomSql(tablesql, displayFields, where, pageNum, pageCount, primaryKey, sort, withnolock);
        }
        /// <summary>
        /// Creates the page SQL by custom SQL.
        /// </summary>
        /// <param name="tableSql">表的Sql语句</param>
        /// <param name="displayFields">The display fields.</param>
        /// <param name="where">The where.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">表Sql不能为空</exception>
        static public string CreatePageSQLByCustomSql(string tableSql, string displayFields, string where, int pageNum, int pageCount, string primaryKey, string sort = "", bool withnolock = false)
        {
            if (string.IsNullOrEmpty(tableSql))
            {
                throw new Exception("表Sql不能为空");
            }
            if (pageCount <= 0)
            {
                pageCount = 10;
            }
            StringBuilder sb = new StringBuilder();
            //显示字段
            string dis = string.IsNullOrEmpty(displayFields) ? "*" : displayFields;
            sb.Append("Select ");
            sb.Append(string.Format(" TOP({0}) ", pageCount));
            sb.Append(dis);
            sb.Append(" from (SELECT ROW_NUMBER() OVER (ORDER BY");
            if (string.IsNullOrEmpty(sort))
            {
                sb.Append(string.Format(" {0}) AS RowNumber", primaryKey));
            }
            else
            {
                sb.Append(string.Format(" {0}) AS RowNumber", sort));
            }
            if (tableSql.ToLower().IndexOf("select") == 0)
            {
                tableSql = tableSql.Remove(0, 6);
                tableSql = "," + tableSql;
            }
            sb.Append(tableSql);

            if (string.IsNullOrEmpty(where) == false)
            {
                sb.Append(" where " + where);
            }
            sb.Append(string.Format(") A WHERE RowNumber > {0}*({1})", pageCount, pageNum - 1));
            return sb.ToString();
        }
        #endregion

        #region Select
        /// <summary>
        /// Creates the select SQL.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="displayFields">The display fields.</param>
        /// <param name="where">The where.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="Count">The count.</param>
        /// <param name="updatecheck">if set to <c>true</c> [updatecheck].</param>
        /// <returns>System.String.</returns>
        static public string CreateSelectSQL(string tableName, IEnumerable<string> displayFields, string where, string sort = "", int Count = 0, bool updatecheck = false)
        {
            StringBuilder sb = new StringBuilder();
            if (displayFields != null && displayFields.Any())
            {
                foreach (string item in displayFields)
                {
                    sb.Append("," + item);
                }
                sb.Remove(sb.ToString().IndexOf(","), 1);
            }
            return CreateSelectSQL(tableName, sb.ToString(), where, sort, Count, updatecheck);

        }
        /// <summary>
        /// 创建Select语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="displayFields">显示字段</param>
        /// <param name="where">where</param>
        /// <param name="sort">排序</param>
        /// <param name="Count">最大记录数目</param>
        /// <param name="updatecheck">是否获取检查更新标识</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        static public string CreateSelectSQL(string tableName, string displayFields, string where, string sort = "", int Count = 0, bool updatecheck = false, bool withnolock = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select ");
            if (Count > 0)
            {
                sb.Append(string.Format("top({0}) ", Count));
            }
            if (string.IsNullOrEmpty(displayFields)==false)
            {
                sb.Append(displayFields);
            }
            else
            {
                sb.Append("*");
            }
            if (updatecheck)
            {
                sb.Append(",CAST(BINARY_CHECKSUM(*) AS VARCHAR(100)) AS OldCheck");
            }
            sb.AppendFormat(" from {0} {1}", tableName, WithNoLockStr(withnolock));
            if (string.IsNullOrEmpty(where) == false)
            {
                sb.Append(" where " + where);
            }
            if (string.IsNullOrEmpty(sort) == false)
            {
                sb.Append(" order by " + sort);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Creates the select SQL.
        /// </summary>
        /// <param name="modeltype">The modeltype.</param>
        /// <param name="displayfields">The displayfields.</param>
        /// <param name="where">The where.</param>
        /// <param name="paramter">The paramter.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="count">The count.</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        static public string CreateSelectSql(Type modeltype, string displayfields, WhereObjectList where, ref SqlDataParameter paramter, string sort = "", int count = 0, bool withnolock = false)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(modeltype); ;
            var wherestr = BuildWhereByWhereList(ref paramter, where);
            return CreateSelectSQL(sqlinfo.TableName, displayfields, wherestr, sort, count, false, withnolock);

        }
        static public string CreateSelectSql(Type modeltype, string displayfields, string where, string sort = "", int count = 0, bool withnolock = false)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(modeltype);
            return CreateSelectSQL(sqlinfo.TableName, displayfields, where, sort, count, false, withnolock);
        }
        /// <summary>
        /// 根据主键
        /// </summary>
        /// <param name="model"></param>
        /// <param name="displayfields"></param>
        /// <param name="withnolock"></param>
        /// <returns></returns>
        static public string CreateSelectByPrimaryKey(object model, string displayfields, bool withnolock = false)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            return sqlinfo.GeneratorSelectByPrimaryKey(model, displayfields, withnolock);
        }
        /// <summary>
        /// 简单查询生成器
        /// </summary>
        /// <param name="model"></param>
        /// <param name="displayfields"></param>
        /// <param name="withnolock"></param>
        /// <returns></returns>
        static public string CreateSelectSimple(object model, string displayfields, string sort = "", int count = 0, bool withnolock = false)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            return sqlinfo.GeneratorSimpleSelectSql(model, displayfields,sort,count, withnolock);
        }

        #endregion

        #region Count
        static public string CreateCountSQLByPrimary(object model)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            return CreateCountSQL(sqlinfo.TableName, sqlinfo.ParimaryKeyWhere(model), true);
        }
        static public string CreateCountSQL(object model)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            return CreateCountSQL(sqlinfo.TableName, sqlinfo.SimpleWhereDeserializer(model), true);
        }

        static public string CreateCountSQL(Type modeltype, WhereObjectList where, ref SqlDataParameter paramter)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(modeltype);
            return CreateCountSQL(sqlinfo.TableName, BuildWhereByWhereList(ref paramter, where), true);
        }
        static public string CreateCountSQL(Type modeltype, string where)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(modeltype);
            return CreateCountSQL(sqlinfo.TableName, where, true);
        }
        /// <summary>
        /// 获取统计SQL
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">The where.</param>
        /// <param name="withnolock">if set to <c>true</c> [withnolock].</param>
        /// <returns>System.String.</returns>
        static public string CreateCountSQL(string tableName, string where, bool withnolock)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(where))
            {
                sb.Append(string.Format("Select count(1) from {0} {1}", tableName, WithNoLockStr(withnolock)));
            }
            else
            {
                sb.Append(string.Format("Select count(1) from {0} {2} where {1}", tableName, where, WithNoLockStr(withnolock)));
            }
            return sb.ToString();
        }
        #endregion
        #endregion

        #region OtherMethod
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            return GetSqlMethodInfo(typeof(T)).TableName;
        }
        private static EntityCacheInfo GetSqlMethodInfo(Type modeltype)
        {
            EntityCacheInfo sqlinfo;
            if (EntityCacheManager.TryGetValue(modeltype, out sqlinfo))
            {
                return sqlinfo;
            }
            else throw new Exception("未找到实体对象的匹配项");
        }
        #endregion

        #region Update
        /// <summary>
        /// Creates the update by primary key.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        static public string CreateUpdateByPrimaryKey(object obj, string extendup = "")
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(obj.GetType());
            return sqlinfo.GetUpdateByPrimaryKey(obj, extendup);
        }
        /// <summary>
        /// Creates the update by where.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="where">The where.</param>
        /// <param name="extendup">扩展的更新字段</param>
        /// <returns>System.String.</returns>
        static public string CreateUpdateByWhere(object obj, string where, string extendup = "")
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(obj.GetType());
            return sqlinfo.GetUpdateByWhere(obj, where, extendup);
        }
        static public string CreateUpdateByWhere(object obj, string where,SqlDataParameter dataparameters,  string extendUp = "")
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(obj.GetType());
            return sqlinfo.GetUpdateByWhere(obj, where, dataparameters, extendUp);

        }
        /// <summary>
        /// 创建更新SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="UpSQL"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        static public string CreateUpdateSQL(string tableName, string UpSQL, string where)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(UpSQL.Trim()) == false)
            {
                sb.Append(string.Format("update {0} set {1}", tableName, UpSQL));
                if (string.IsNullOrEmpty(where) == false)
                {
                    sb.Append(string.Concat(" where ",where));
                }
            }
            else
            {
                throw new Exception("没有需要更新的值");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 创建更新SQL，如果字典中含有主键值，此处需标记，会跳过此字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="updateFields">更新字段</param>
        /// <param name="where">条件</param>
        /// <param name="primarykey">如果字典中含有主键值，此处需标记，会跳过此字段</param>
        /// <returns>System.String.</returns>
        static public string CreateUpdateSQL(string tableName, Dictionary<string, object> updateFields, string where, string primarykey = "")
        {
            var UpSQL = BuildUpdateFields(updateFields, primarykey);
            return CreateUpdateSQL(tableName, UpSQL, where);
        }
        /// <summary>
        /// Builds the update from entity.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="expFields">扩展字段</param>
        /// <param name="updatePrimaryKey">是否更新主键</param>
        /// <returns></returns>
        static public string BuildUpdateFields(Dictionary<string, object> entity, string primarykey = "")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in entity.Keys.Where(p => p.Equals(primarykey, StringComparison.OrdinalIgnoreCase)))
            {
                sb.Append(string.Format(",{0}=@{1}", field));
            }
            return sb.ToString();
        }
        #endregion

        #region InsertSql

        /// <summary>
        /// Creates the insert SQL.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.String.</returns>
        static public string CreateInsertSQL(object model)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            return sqlinfo.GetInsertSql(model);
        }
        /// <summary>
        /// 返回不返回主键的SQL
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        static public string CreateInsertNoReturnAutoIDSQL(object model)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());

            return sqlinfo.GetInsertNoReturnAutoIDSql(model);
        }
        /// <summary>
        /// 创建插入SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="InSQL">插入的值</param>
        /// <param name="identityField">要返回的主键,不设置无返回值</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        static public string CreateInsertSQL(string tableName, string InSQL, string identityField = "")
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(InSQL.Trim()) == false)
            {
                sb.Append(string.Format("insert into {0} {1}", tableName, InSQL));
            }
            else
            {
                throw new Exception("没有需要插入的数据");
            }
            if (string.IsNullOrEmpty(identityField) == false)
            {
                sb.AppendLine(string.Format("SELECT @@IDENTITY AS '{0}'", identityField));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 创建插入SQL语句,不返回自增主键
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="InSQL"></param>
        /// <param name="identityField"></param>
        /// <returns></returns>
        static public string CreateInsertSQLWithNoReturn(string tableName, string InSQL, string identityField = "")
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(InSQL.Trim()) == false)
            {
                sb.Append(string.Format("insert into {0} {1}", tableName, InSQL));
            }
            else
            {
                throw new Exception("没有需要插入的数据");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 创建插入SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="insertvalues">插入的值</param>
        /// <param name="identityField">要返回的主键,不设置无返回值</param>
        /// <returns></returns>
        static public string CreateInsertSQL(string tableName, Dictionary<string, object> insertvalues, string identityField = "")
        {
            var InSQL = BuildInsertFields(insertvalues);
            return CreateInsertSQL(tableName, InSQL, identityField);
        }
        /// <summary>
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="entity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        static public string BuildInsertFields(Dictionary<string, object> entity)
        {
            var keys = new StringBuilder();
            var values = new StringBuilder();

            foreach (var key in entity.Keys)
            {
                keys.Append(string.Format(",{0}", key));
                values.Append(string.Format(",@{0}", key));
            }
            if (keys.Length > 0)
            {
                keys.Remove(0, 1);
                values.Remove(0, 1);
                keys.Insert(0, "(");
                keys.Append(string.Format(") values ({0})", values.ToString()));
            }
            return keys.ToString();
        }
        #endregion

        #region Delete
        /// <summary>
        /// 创建删除SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        static public string CreateDeleteSQL(string tableName, string where)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(where.Trim()))
            {
                sb.Append(string.Format("Delete from {0} ", tableName));
            }
            else
            {
                sb.Append(string.Format("Delete from {0} where {1}", tableName, where));
            }
            return sb.ToString();
        }
        static public string CreateDeleteSQL(Type model, string where)
        {
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model);
            return CreateDeleteSQL(sqlinfo.TableName, where);
        }
        static public string CreateDeleteSQL(Type model, WhereObjectList where, ref SqlDataParameter sqlparamter)
        {
            var wherestr = BuildWhereByWhereList(ref sqlparamter, where);
            return CreateDeleteSQL(model, wherestr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        static public string CreateDeleteByPrimaryKey(object model)
        {
            string where = string.Empty;
            EntityCacheInfo sqlinfo = GetSqlMethodInfo(model.GetType());
            where = sqlinfo.ParimaryKeyWhere(model);
            return CreateDeleteSQL(sqlinfo.TableName, where);
        }

        #endregion

        #region Where
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        static public string BuildAndWhere(Dictionary<string, object> entity)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var field in entity.Keys)
            {
                sb.Append(string.Format(" and {0}=@{1}", field));
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, 4);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="wherelist"></param>
        /// <returns></returns>
        static public string BuildWhereByWhereList(ref SqlDataParameter parameters, WhereObjectList wherelist)
        {
            if (wherelist == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            bool haveWhere = false;
            foreach (WhereObject where in wherelist)
            {
                string oper = BuildWhereByWhereObject(ref parameters, where);
                if (string.IsNullOrEmpty(oper) == false)
                {
                    sb.Append(string.Format(" and {0}", oper));
                }
            }
            if (sb.Length > 0)
            {
                haveWhere = true;
                sb.Remove(0, 5);
            }
            string orwhere = string.Empty;
            foreach (WhereObject where in wherelist.ORListObject)
            {
                string oper = BuildWhereByWhereObject(ref parameters, where);
                if (string.IsNullOrEmpty(oper) == false)
                {
                    orwhere += string.Format(" or {0}", oper);
                }
            }
            if (haveWhere == false && orwhere.Length > 0)
            {
                sb.Append(orwhere.TrimStart(" or".ToCharArray()));
            }
            else
            {
                sb.Append(orwhere);
            }
            if (wherelist.AndList.Count > 0)
            {
                foreach (WhereObjectList item in wherelist.AndList)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendFormat(" and ({0})", BuildWhereByWhereList(ref parameters, item));
                    }
                    else
                    {
                        sb.AppendFormat(" ({0})", BuildWhereByWhereList(ref parameters, item));
                    }
                }
            }
            if (wherelist.ORList.Count > 0)
            {
                foreach (WhereObjectList item in wherelist.ORList)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendFormat(" or ({0})", BuildWhereByWhereList(ref parameters, item));
                    }
                    else
                    {
                        sb.AppendFormat(" ({0})", BuildWhereByWhereList(ref parameters, item));
                    }
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        static public string BuildWhereByWhereObject(ref SqlDataParameter parameters, WhereObject where)
        {
            if (where.Value == null)
            {
                throw new Exception(string.Format("{0}字段的条件值为空", where.FieldName));
            }
            string result;
            string newkey;
            switch (where.Operation)
            {
                case WhereOperation.Equal:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}={1}", where.FieldName, newkey);
                    break;
                case WhereOperation.NotEqual:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}<>{1}", where.FieldName, newkey);
                    break;
                case WhereOperation.GreaterThan:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}>{1}", where.FieldName, newkey);
                    break;
                case WhereOperation.GreaterEqual:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}>={1}", where.FieldName, newkey);
                    break;
                case WhereOperation.LessThan:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}<{1}", where.FieldName, newkey);
                    break;
                case WhereOperation.LessEqual:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}<={1}", where.FieldName, newkey);
                    break;
                case WhereOperation.IsNull:
                    bool IsNull;
                    if (bool.TryParse(where.Value.ToString(), out IsNull) == true)
                    {
                        if (IsNull)
                        {
                            result = string.Format("{0} is null", where.FieldName);
                        }
                        else
                        {
                            result = string.Format("not {0}  is null", where.FieldName);
                        }
                    }
                    else
                    {
                        throw new Exception("条件为空");
                    }
                    break;
                case WhereOperation.InClude:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    if (where.Value.GetType().IsValueType)
                    {
                        result = string.Format("{0} in ({1})", where.FieldName, newkey);
                    }
                    else
                    {
                        result = string.Format("{0} in {1}", where.FieldName, newkey);
                    }
                    break;
                case WhereOperation.NotInClude:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    if (where.Value.GetType().IsValueType)
                    {
                        result = string.Format("{0} not in ({1})", where.FieldName, newkey);
                    }
                    else
                    {
                        result = string.Format("{0} not in {1}", where.FieldName, newkey);
                    }
                    
                    break;
                case WhereOperation.Like:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0} like {1}", where.FieldName, newkey);
                    break;
                case WhereOperation.Self:
                    result = where.Value.ToString();
                    break;
                default:
                    newkey = parameters.AddParamter(where.FieldName, where.Value, where.FieldDbType);
                    result = string.Format("{0}={1}", where.FieldName, newkey);
                    break;
            }
            return result;
        }
        static public string BuildPrimaryKeyWhere(Type modeltype)
        {
            var sqlinfo = GetSqlMethodInfo(modeltype);
            return string.Format("{0}=@{1}",sqlinfo.PrimiaryKey);
        }

        #endregion
    }
}
