using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager
    {
        #region Fields
        /// <summary>
        /// 连接字符串信息
        /// </summary>
        public ConnectionStringSettings ConnectionInfo { get; set; }
        protected IDbConnection _Connection;
        /// <summary>
        /// Sql执行加锁
        /// </summary>
        public bool WithNoLock { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public IDbConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private IDbTransaction _Transaction;
        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction Transaction { get { return _Transaction; } }
        /// <summary>
        /// 连接状态
        /// </summary>
        protected MyConnectionStates _myconnectionState = MyConnectionStates.Closed;
        /// <summary>
        /// 获取数据库连接状态
        /// </summary>
        public MyConnectionStates MyConnectionState
        {
            get { return _myconnectionState; }
            set { _myconnectionState = value; }
        }
        private string _LastSqlExpress;
        /// <summary>
        /// 最后执行的Sql语句
        /// </summary>
        public string LastSqlExpress
        {
            get
            {
                return _LastSqlExpress;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public EntityManager()
            : this("defaultDbProvider")
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider">连接名称</param>
        public EntityManager(string provider)
        {
            ConnectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings[provider];
            _Connection = new SqlConnection(ConnectionInfo.ConnectionString);
        }
        #endregion

        #region Connection
        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConnection(MyConnectionStates connectionStates = MyConnectionStates.NeedClose)
        {
            if ((_myconnectionState == MyConnectionStates.Closed || this._Connection.State == ConnectionState.Closed) && this._Connection != null)
            {
                this._Connection.Open();
                _myconnectionState = connectionStates;
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            if (_myconnectionState == MyConnectionStates.NeedClose && this._Connection != null)
            {
                this._Connection.Close();
                //this._Connection.Dispose();
                //this._Connection = CreateNewConnection();
                _myconnectionState = MyConnectionStates.Closed;
            }
        }
        #endregion

        #region Transaction
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_Transaction == null)
            {
                if (this._Connection.State != ConnectionState.Open)
                {
                    this._Connection.Open();
                }
                _Transaction = this._Connection.BeginTransaction();
                _myconnectionState = MyConnectionStates.NoNeedClose;
            }
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="leve">事务级别</param>
        public void BeginTransaction(IsolationLevel leve)
        {
            if (_Transaction == null)
            {
                if (this._Connection.State != ConnectionState.Open)
                {
                    this._Connection.Open();
                }
                _Transaction = this._Connection.BeginTransaction(leve);
                _myconnectionState = MyConnectionStates.NoNeedClose;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Rollback();
                _Transaction = null;
                _myconnectionState = MyConnectionStates.NeedClose;
                this.CloseConnection();
            }
        }
        /// <summary>
        /// 确认事务
        /// </summary>
        public void CommitTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Commit();
                _Transaction = null;
                _myconnectionState = MyConnectionStates.NeedClose;
                this.CloseConnection();
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        public virtual void Insert(object model)
        {
            var sql = EntityCache.CreateInsertSQL(model);
            _Connection.Execute(sql, model, _Transaction);
        }
        /// <summary>
        /// 带返回自增字段的插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int InsertReturnKey(object model)
        {
            string sql = string.Empty;
            EntityCacheInfo sqlinfo;
            if (EntityCacheManager.TryGetValue(model.GetType(), out sqlinfo))
            {
                sql=sqlinfo.GetInsertSql(model);
            }
            else throw new Exception("未找到实体对象的匹配项");            
            return _Connection.ExecuteScalar<int>(sql, model, _Transaction, null, null);
        }
        #endregion

        #region Update
        /// <summary>
        /// 根据主键更新
        /// </summary>
        /// <param name="model">主键已经赋值的实体</param>
        /// <param name="extendUpSql">自定义的更新</param>
        /// <returns>受影响的行数</returns>
        public virtual int UpdateByPrimaryKey(object model, string extendUpSql = "")
        {
            return _Connection.UpdateByPrimaryKey(model, extendUpSql, _Transaction);
        }
        /// <summary>
        /// 如果已经存在则更新，否则新增
        /// </summary>
        /// <param name="model">主键已经赋值的实体</param>
        /// <returns>受影响的行数</returns>
        public virtual int InsertUpdate(object model)
        {
            if (ContainsKey(model))
            {
                return UpdateByPrimaryKey(model, "");
            }
            else
            {
                Insert(model);
                return 1;
            }
        }
        /// <summary>
        /// 根据条件更新实体内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="extendUpSql">自定义更新</param>
        /// <returns>受影响的行数</returns>
        public virtual int Update(object model, WhereClip where, string extendUpSql = "")
        {
            return _Connection.UpdateByWhere(model, where, extendUpSql, _Transaction);
        }
        /// <summary>
        /// 根据条件更新实体内容
        /// </summary>
        /// <param name="model">要更新的实体</param>
        /// <param name="where"></param>
        /// <param name="extendUpSql">自定义更新</param>
        /// <returns></returns>
        public virtual int Update(object model, string where, string extendUpSql = "")
        {
            return _Connection.UpdateByWhere(model, where, extendUpSql, _Transaction);
        }
        #endregion

        #region Contains
        /// <summary>
        /// 只以实体主键作为查询依据，查询是否存在数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bool</returns>
        public virtual bool ContainsKey(object model)
        {
            var sql = EntityCache.CreateSelectByPrimaryKey(model, "*", WithNoLock);
            var result = _Connection.Query(sql, model, _Transaction);
            return result.Count() > 0;
        }
        /// <summary>
        /// 以实体所有值为条件查询，查询是否存在数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Contains(object model)
        {
            var sql = EntityCache.CreateSelectSimple(model, "*", "", 1, WithNoLock);
            var result = _Connection.Query(sql, model, _Transaction);
            return result.Count() > 0;
        }
        /// <summary>
        /// 根据查询条件确认是否存在数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual bool Contains<T>(WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var sql = EntityCache.CreateSelectSql(typeof(T), "", where, ref p, "", 1, WithNoLock);
            var result = _Connection.Query(sql, p, _Transaction);
            return result.Count() > 0;
        }
        #endregion

        #region Select
        #region DataTable
        /// <summary>
        /// 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">要显示的字段，空或者Null代表全部字段</param>
        /// <param name="where">条件</param>
        /// <param name="whereParamters">条件中变量</param>
        /// <param name="sort">排序</param>
        /// <param name="count">返回数据量</param>
        /// <param name="withnoloc">是否解除事务锁定</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual DataTable QueryTable<T>(string field, string where, object whereParamters, string sort = "", int count = 0, bool withnoloc = true)
        {
            var sql = EntityCache.CreateSelectSql(typeof(T), field, where, sort, count, withnoloc);
            _LastSqlExpress = sql;
            return _Connection.QueryDataTable(sql, whereParamters,typeof(T), _Transaction);
        }
        /// <summary>
        /// 根据实体中的值作为简单条件 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">带条件值的实体</param>
        /// <param name="field">字段</param>
        /// <param name="sort">排序</param>
        /// <param name="count">返回数据量</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual DataTable QueryTable<T>(T model, string field, string sort = "", int count = 0)
        {
            var sql = EntityCache.CreateSelectSimple(model, field, sort, count, WithNoLock);
            _LastSqlExpress = sql;
            return _Connection.QueryDataTable(sql, model, typeof(T), _Transaction);
        }
        /// <summary>
        /// 根据条件作为简单条件 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where条件</param>
        /// <param name="displayfields">查询结果字段</param>
        /// <param name="sort">排序</param>
        /// <param name="count">查询结果最大记录数</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual DataTable QueryTable<T>(WhereObjectList where, string displayfields, string sort = "", int count = 0)
        {
            var p = new SqlDataParamter();
            var sql = EntityCache.CreateSelectSql(typeof(T), displayfields, where, ref p, sort, count, WithNoLock);
            _LastSqlExpress = sql;
            return _Connection.QueryDataTable(sql, p, typeof(T), _Transaction);
        }
        /// <summary>
        /// 根据自定义表Sql查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablesql">自定义表Sql</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual DataTable QueryTable<T>(string tablesql, WhereObjectList where)
        {
            string wherestr = "";
            var p = new SqlDataParamter();
            if (where != null)
            {
                wherestr = " where ";
                var wheresql = EntityCache.BuildWhereByWhereList(ref p, where);
                wherestr += wheresql;
            }
            var sql = tablesql + wherestr;
            _LastSqlExpress = sql;
            return _Connection.QueryDataTable(sql, p, typeof(T), _Transaction);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">条件数据</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual DataTable GetDataTableByPage<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                throw new Exception("自定义分页必须定义排序字段");
            }
            var sql = EntityCache.CreatePageSQLByCustomSql(tableSql, fieldsName, where, pageNum, pageCount, "", orderBy);
            _LastSqlExpress = sql;
            return _Connection.QueryDataTable(sql, whereobject, typeof(T),_Transaction);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">where条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual PageTableResult GetDataTableByPageHasTotal<T>(string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            var result = new PageTableResult();
            result.total = Count<T>(where, whereobject);
            var sql = EntityCache.CreatePageSQL(typeof(T), fieldsName, where, pageNum, pageCount, orderBy);
            _LastSqlExpress = sql;
            result.SetDatas(_Connection.QueryDataTable(sql, whereobject, typeof(T),_Transaction));
            return result;
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段.</param>
        /// <param name="orderBy">排序.</param>
        /// <param name="pageNum">页码.</param>
        /// <param name="pageCount">每页数据量.</param>
        /// <param name="where">条件.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual PageTableResult GetDataTableByPageHasTotal<T>(string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            return GetDataTableByPageHasTotal<T>(fieldsName, orderBy, pageNum, pageCount, wheresrt, p);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">条件数据</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual PageTableResult GetDataTableByPageHasTotal<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                throw new Exception("自定义分页必须定义排序字段");
            }
            var result = new PageTableResult();
            result.total = CountByCustomTable(tableSql, where, whereobject);
            result.SetDatas(GetDataTableByPage<T>(tableSql, fieldsName, orderBy, pageNum, pageCount, where, whereobject));
            return result;
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual PageTableResult GetDataTableByPageHasTotal<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            return GetDataTableByPageHasTotal<T>(tableSql, fieldsName, orderBy, pageNum, pageCount, wheresrt, p);
        }
        #endregion

        #region Object
        /// <summary>
        /// 根据实体有值字段作为查询条件查询单条数据，如果有数据返回True和module
        /// </summary>
        /// <typeparam name="T">表实体类型</typeparam>
        /// <param name="module">表实体实例</param>
        /// <param name="displayfields">要显示的字段</param>
        /// <returns></returns>
        public virtual bool TryGetData<T>(ref T module, string displayfields = "", string sort = "")
        {
            return _Connection.TryGetEntity(ref module, displayfields, WithNoLock, _Transaction, sort);
        }
        /// <summary>
        /// 根据Where条件查询数据，如果有数据返回True和module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="module">返回实体类型</param>
        /// <param name="where">条件</param>
        /// <param name="displayfields">要显示的字段</param>
        /// <param name="sort">排序字段</param>
        /// <returns>返回T module</returns>
        public virtual bool TryGetData<T>(out T module, WhereObjectList where, string displayfields = null, string sort = "")
        {
            var p = new SqlDataParamter();
            string wherestr = "";
            if (where != null)
            {
                wherestr = EntityCache.BuildWhereByWhereList(ref p, where);
            }
            var sql = EntityCache.CreateSelectSql(typeof(T), displayfields, wherestr, sort, 1, WithNoLock);
            var list = _Connection.Query<T>(sql, p, _Transaction);
            if (list.Any())
            {
                module = list.FirstOrDefault();
                return true;
            }
            else
            {
                module = default(T);
                return false;
            }
        }
        /// <summary>
        /// 以实体主键值为查询条件，查询数据，如果有数据返回True和module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="module">The module.</param>
        /// <param name="displayfields">要显示的字段</param>
        /// <returns>bool</returns>
        public virtual bool TryGetDataByPrimaryKey<T>(ref T module, string displayfields = "")
        {
            return _Connection.TryGetEntityByPrimaryKey(ref module, displayfields, WithNoLock, _Transaction);
        }
        /// <summary>
        /// 返回单个值的查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string sql, object parm)
        {
            return this._Connection.ExecuteScalar<T>(sql, parm, _Transaction, null, null);
        }
        #endregion

        #region Page
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">where条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual IEnumerable<T> GetDataByPage<T>(string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            var sql = EntityCache.CreatePageSQL(typeof(T), fieldsName, where, pageNum, pageCount, orderBy);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, whereobject, _Transaction);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段.</param>
        /// <param name="orderBy">排序.</param>
        /// <param name="pageNum">页码.</param>
        /// <param name="pageCount">每页数据量.</param>
        /// <param name="where">条件.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual IEnumerable<T> GetDataByPage<T>(string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            var sql = EntityCache.CreatePageSQL(typeof(T), fieldsName, wheresrt, pageNum, pageCount, orderBy);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, p, _Transaction);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">条件数据</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual IEnumerable<T> GetDataByPage<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                throw new Exception("自定义分页必须定义排序字段");
            }
            var sql = EntityCache.CreatePageSQLByCustomSql(tableSql, fieldsName, where, pageNum, pageCount, "", orderBy);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, whereobject, _Transaction);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual IEnumerable<T> GetDataByPage<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                throw new Exception("自定义分页必须定义排序字段");
            }
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            var sql = EntityCache.CreatePageSQLByCustomSql(tableSql, fieldsName, wheresrt, pageNum, pageCount, "", orderBy);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, p, _Transaction);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">where条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual PageResult<T> GetDataByPageHasTotal<T>(string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            var result = new PageResult<T>();
            result.total = Count<T>(where, whereobject);
            var sql = EntityCache.CreatePageSQL(typeof(T), fieldsName, where, pageNum, pageCount, orderBy);
            _LastSqlExpress = sql;
            result.datas = _Connection.Query<T>(sql, whereobject, _Transaction);
            return result;
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldsName">字段.</param>
        /// <param name="orderBy">排序.</param>
        /// <param name="pageNum">页码.</param>
        /// <param name="pageCount">每页数据量.</param>
        /// <param name="where">条件.</param>
        /// <returns>IEnumerable{``0}.</returns>
        public virtual PageResult<T> GetDataByPageHasTotal<T>(string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            return GetDataByPageHasTotal<T>(fieldsName, orderBy, pageNum, pageCount, wheresrt, p);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <param name="whereobject">条件数据</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual PageResult<T> GetDataByPageHasTotal<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, string where, object whereobject)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                throw new Exception("自定义分页必须定义排序字段");
            }
            PageResult<T> result = new PageResult<T>();
            result.total = CountByCustomTable(tableSql, where, whereobject);
            result.datas = GetDataByPage<T>(tableSql, fieldsName, orderBy, pageNum, pageCount, where, whereobject);
            return result;
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableSql">表Sql</param>
        /// <param name="fieldsName">要显示的字段名</param>
        /// <param name="orderBy">排序字段必填</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数据量</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable{``0}.</returns>
        /// <exception cref="System.Exception">自定义分页必须定义排序字段</exception>
        public virtual PageResult<T> GetDataByPageHasTotal<T>(string tableSql, string fieldsName, string orderBy, int pageNum, int pageCount, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var wheresrt = EntityCache.BuildWhereByWhereList(ref p, where);
            return GetDataByPageHasTotal<T>(tableSql,fieldsName,orderBy,pageNum,pageCount,wheresrt,p);
        }
        /// <summary>
        /// 自定义表语句分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablesql">表Sql</param>
        /// <param name="where">where条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageNum">页码</param>
        /// <param name="pageCount">每页数量</param>
        /// <param name="fields">字段</param>
        /// <returns>PageResult{``0}.</returns>
        [Obsolete]
        public virtual PageResult<T> GetDataByPage<T>(string tablesql, WhereObjectList where, string orderBy, int pageNum, int pageCount, string fields = "")
        {
            PageResult<T> result = new PageResult<T>();
            var p = new SqlDataParamter();
            var wherestr = EntityCache.BuildWhereByWhereList(ref p, where);
            result.total = CountByCustomTable(tablesql, wherestr, p);
            result.datas = GetDataByPage<T>(tablesql, fields, orderBy, pageNum, pageCount, wherestr, p);
            return result;
        }
        #endregion

        #region Ienumerable<T>
        /// <summary>
        /// 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">要显示的字段，空或者Null代表全部字段</param>
        /// <param name="where">条件</param>
        /// <param name="whereParamters">条件中变量</param>
        /// <param name="sort">排序</param>
        /// <param name="count">返回数据量</param>
        /// <param name="withnoloc">是否解除事务锁定</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual IEnumerable<T> Query<T>(string field, string where, object whereParamters, string sort = "", int count = 0, bool withnoloc = true)
        {
            var sql = EntityCache.CreateSelectSql(typeof(T), field, where, sort, count, withnoloc);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, whereParamters, _Transaction);
        }
        /// <summary>
        /// 根据实体中的值作为简单条件 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">带条件值的实体</param>
        /// <param name="field">字段</param>
        /// <param name="sort">排序</param>
        /// <param name="count">返回数据量</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual IEnumerable<T> Query<T>(T model, string field, string sort = "", int count = 0)
        {
            var sql = EntityCache.CreateSelectSimple(model, field, sort, count, WithNoLock);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, model, _Transaction);
        }
        /// <summary>
        /// 根据条件作为简单条件 查询单表数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where条件</param>
        /// <param name="displayfields">查询结果字段</param>
        /// <param name="sort">排序</param>
        /// <param name="count">查询结果最大记录数</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual IEnumerable<T> Query<T>(WhereObjectList where, string displayfields, string sort = "", int count = 0)
        {
            var p = new SqlDataParamter();
            var sql = EntityCache.CreateSelectSql(typeof(T), displayfields, where, ref p, sort, count, WithNoLock);
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, p, _Transaction);
        }
        /// <summary>
        /// 根据自定义表Sql查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tablesql">自定义表Sql</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public virtual IEnumerable<T> Query<T>(string tablesql, WhereObjectList where)
        {
            string wherestr = "";
            var p = new SqlDataParamter();
            if (where != null)
            {
                wherestr = " where ";
                var wheresql = EntityCache.BuildWhereByWhereList(ref p, where);
                wherestr += wheresql;
            }
            var sql = tablesql + wherestr;
            _LastSqlExpress = sql;
            return _Connection.Query<T>(sql, p,_Transaction);
        }
        /// <summary>
        /// 根据自定义表语句查询
        /// </summary>
        /// <param name="tablesql">自定义表Sql</param>
        /// <param name="where">条件</param>
        /// <returns>IEnumerable&lt;IDictionary&lt;string,object&gt;&gt;</returns>
        public virtual IEnumerable<IDictionary<string, object>> Query(string tablesql, WhereObjectList where)
        {
            _LastSqlExpress = tablesql;
            return Query<IDictionary<string, object>>(tablesql, where);
        }
        #endregion
        #endregion

        #region Delete
        /// <summary>
        /// 根据where条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int DeleteByWhere(Type model, WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var sql = EntityCache.CreateDeleteSQL(model, where, ref p);
            _LastSqlExpress = sql;
            return _Connection.Execute(sql, p, _Transaction);
        }
        public virtual int DeleteByWhere(Type model, string where, object whereobject)
        {
            var sql = EntityCache.CreateDeleteSQL(model, where);
            _LastSqlExpress = sql;
            return _Connection.Execute(sql, whereobject, _Transaction);
        }
        /// <summary>
        /// 根据主键作为条件删除数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int DeleteByPrimaryKey(object model)
        {
            var sql = EntityCache.CreateDeleteByPrimaryKey(model);
            _LastSqlExpress = sql;
            return _Connection.Execute(sql, model, _Transaction);
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereobject"></param>
        /// <returns></returns>
        public virtual int DeleteByMultiKey<T>(IEnumerable<T> whereobject)
        {
            var where = EntityCache.BuildPrimaryKeyWhere(typeof(T));
            return DeleteByWhere(typeof(T), where, whereobject);
        }
        #endregion

        #region Count
        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="where"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int Count<T>(WhereObjectList where)
        {
            var p = new SqlDataParamter();
            var sql = EntityCache.CreateCountSQL(typeof(T), where, ref p);
            _LastSqlExpress = sql;
            return _Connection.ExecuteScalar<int>(sql, p, _Transaction, null, null);
        }
        /// <summary>
        /// 根据实体中有值字段作为条件统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int Count(object model)
        {
            var sql = EntityCache.CreateCountSQL(model);
            _LastSqlExpress = sql;
            return _Connection.ExecuteScalar<int>(sql, model, _Transaction, null, null);
        }
        /// <summary>
        /// 根据主键作为条件作为条件统计
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual int CountByPriamaryKey(object model)
        {
            var sql = EntityCache.CreateCountSQLByPrimary(model);
            _LastSqlExpress = sql;
            return _Connection.ExecuteScalar<int>(sql, model, _Transaction, null, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">where语句</param>
        /// <param name="whereobject">条件数据</param>
        /// <returns></returns>
        public virtual int Count<T>(string where, object whereobject)
        {
            var sql = EntityCache.CreateCountSQL(typeof(T), where);
            _LastSqlExpress = sql;
            return _Connection.ExecuteScalar<int>(sql, whereobject, _Transaction, null, null);
        }
        /// <summary>
        /// 根据自定义表和where条件统计
        /// </summary>
        /// <param name="tablename">自定义表Sql</param>
        /// <param name="where">where</param>
        /// <param name="whereParamObj">where条件的值</param>
        /// <returns>Int</returns>
        public virtual int Count(string tablename, string where, object whereParamObj)
        {
            var sql = string.Format("select count(1) from {0} {1}", tablename, string.IsNullOrEmpty(where) ? "" : ("where " + where));
            _LastSqlExpress = sql;
            return _Connection.ExecuteScalar<int>(sql, whereParamObj, _Transaction, null, null);
        }
        /// <summary>
        /// 根据自定义表语句统计
        /// </summary>
        /// <param name="tableSql">自定义表Sql</param>
        /// <param name="where">where</param>
        /// <param name="whereParamObj">where条件的值</param>
        /// <returns></returns>
        public virtual int CountByCustomTable(string tableSql, string where, object whereParamObj)
        {
            Regex regex = new Regex(@"\sfrom\s", RegexOptions.IgnoreCase);
            var match = regex.Match(tableSql);
            if (match.Success)
            {
                var index = match.Index;
                if (index > 0)
                {
                    tableSql = tableSql.Substring(index + 6);
                }
            }
            _LastSqlExpress = tableSql;
            return Count(tableSql, where, whereParamObj);
        }
        #endregion

        #region OtherMethod
        private string GetDisplay(IEnumerable<string> displayfields)
        {
            return displayfields == null ? "" : displayfields.ToStringSeparatedByComma();
        }
        #endregion

    }
}
