#region << 版 本 注 释 >>
/****************************************************
* 文 件 名：
* Copyright(c) ITdos
* CLR 版本: 4.0.30319.18408
* 创 建 人：steven hu
* 电子邮箱：
* 官方网站：www.ITdos.com
* 创建日期：2010/2/10
* 文件描述：
******************************************************
* 修 改 人：
* 修改日期：
* 备注描述：
*******************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data.Common;
using FWW.Framework.DapperEx;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Globalization;

namespace FWW.Framework.DapperEx
{

    /// <summary>
    /// Section
    /// </summary>
    public abstract class Section
    {

        #region Property
        protected Database database;
        protected DbCommand cmd;
        protected DbTransaction tran = null; 
        #endregion

        #region Constructor
        public Section(Database currentdatabase)
        {
            Check.NotNullOrEmpty(currentdatabase, "dbSession");
            this.database = currentdatabase;
        } 
        #endregion

        #region 执行

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public virtual object ToScalar()
        {
            return (tran == null ? this.database.ExecuteScalar(cmd) : this.database.ExecuteScalar(cmd, tran));
        }


        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return DataUtils.ConvertValue<TResult>(ToScalar());
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public virtual IDataReader ToDataReader()
        {
            return (tran == null ? this.database.ExecuteReader(cmd) : this.database.ExecuteReader(cmd, tran));
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public virtual DataSet ToDataSet()
        {
            return (tran == null ? this.database.ExecuteDataSet(cmd) : this.database.ExecuteDataSet(cmd, tran));
        }


        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return this.ToDataSet().Tables[0];
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public virtual int ExecuteNonQuery()
        {
            return (tran == null ? this.database.ExecuteNonQuery(cmd) : this.database.ExecuteNonQuery(cmd, tran));
        }
        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public TEntity First<TEntity>()
        {
            return ToFirst<TEntity>();
        }
        //private Func<IDataReader, object> GetDeserializer<T>(IDataReader reader,object parama)
        //{
        //    var effectiveType = parama.GetType();
        //    var identity = new SqlMapper.Identity(cmd.CommandText, cmd.CommandType, database.GetConnection(), effectiveType, cmd.GetType(), null);
        //    var info = SqlMapper.GetCacheInfo(identity, cmd.Parameters, true);
        //    int hash = SqlMapper.GetColumnHash(reader);
        //    if (info.Deserializer.Func == null || info.Deserializer.Hash != hash)
        //    {
        //        info.Deserializer = new SqlMapper.DeserializerState(hash,SqlMapper.GetDeserializer(effectiveType, reader, 0, -1, false));
        //        SqlMapper.SetQueryCache(identity, info);
        //    }
        //    return info.Deserializer.Func;
        //}
        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirst<TEntity>()
        {
            var list = ToList<TEntity>();
            return list.First();
        }

        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirstDefault<TEntity>()
        {
            var list = ToList<TEntity>();
            return list.FirstOrDefault();
        }
        /// <summary>
        /// 返回实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> ToList<TEntity>()
        {
            //List<TEntity> listT = new List<TEntity>();
            return ToEnumerable<TEntity>().ToList();
            //return listT;
        }
        public IEnumerable<T> ToEnumerable<T>()
        {
            var cmd1 = new CommandDefinition(cmd.CommandText, cmd.Parameters, null, null, CommandType.StoredProcedure, CommandFlags.None);
            return database.GetConnection().Query<T>(cmd1);
        }
        #endregion

    }
}

