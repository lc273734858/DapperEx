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
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 执行存储过程
    /// </summary>
    public class ProcedureSection:Section
    {

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbSession"></param>
        /// <param name="procName"></param>
        public ProcedureSection(Database database, string procName) : base(database)
        {
            Check.NotNullOrEmpty(procName, "存储过程名称");
            this.cmd = database.GetStoredProcCommand(procName);
        } 
        #endregion

        #region Fields
        /// <summary>
        /// 返回的参数
        /// </summary>
        private List<string> outParameters = new List<string>();
        private DbTransaction tran; 
        #endregion

        #region Method
        /// <summary>
        /// 设置事务
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ProcedureSection SetDbTransaction(DbTransaction tran)
        {
            this.tran = tran;
            return this;
        }

        /// <summary>
        /// 存储过程参数不要加前缀
        /// </summary>
        protected bool isParameterSpecial
        {
            get
            {
                return !(database.DbProvider is SqlServer.SqlServerProvider
                           || database.DbProvider is SqlServer9.SqlServer9Provider
                           || database.DbProvider is MsAccess.MsAccessProvider);
            }
        }

        /// <summary>
        /// 获取参数名字
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string getParameterName(string parameterName)
        {
            Check.NotNullOrEmpty(parameterName, "parameterName");
            if (!isParameterSpecial)
            {
                return database.DbProvider.BuildParameterName(parameterName);
            }
            else
            {
                return parameterName.TrimStart(database.DbProvider.ParamPrefix);
            }


        }
        /// <summary>
        /// 返回存储过程返回值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetReturnValues()
        {
            Dictionary<string, object> returnValues = new Dictionary<string, object>();
            foreach (string outParameter in outParameters)
            {
                returnValues.Add(outParameter, cmd.Parameters[getParameterName(outParameter)].Value);
            }
            return returnValues;
        } 
        #endregion

        #region 添加参数


        /// <summary>
        /// 添加参数
        /// </summary>
        public ProcedureSection AddParameter(params DbParameter[] parameters)
        {
            database.AddParameter(this.cmd, parameters);
            return this;
        }


        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcedureSection AddInParameter(string parameterName, DbType dbType, object value)
        {
            return AddInParameter(parameterName, dbType, 0, value);
        }

        /// <summary>
        /// 添加输入参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcedureSection AddInParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.NotNullOrEmpty(parameterName, "parameterName");
            Check.NotNullOrEmpty(dbType, "dbType");

            database.AddInParameter(this.cmd, parameterName, dbType, size, value);
            return this;
        }

        /// <summary>
        /// 添加输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcedureSection AddOutParameter(string parameterName, DbType dbType)
        {
            return AddOutParameter(parameterName, dbType, 0);
        }

        /// <summary>
        /// 添加输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcedureSection AddOutParameter(string parameterName, DbType dbType, int size)
        {
            Check.NotNullOrEmpty(parameterName, "parameterName");
            Check.NotNullOrEmpty(dbType, "dbType");

            database.AddOutParameter(this.cmd, parameterName, dbType, size);

            outParameters.Add(parameterName);
            return this;
        }


        /// <summary>
        /// 添加输入输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ProcedureSection AddInputOutputParameter(string parameterName, DbType dbType, object value)
        {
            return AddInputOutputParameter(parameterName, dbType, 0, value);
        }


        /// <summary>
        /// 添加输入输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcedureSection AddInputOutputParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.NotNullOrEmpty(parameterName, "parameterName");
            Check.NotNullOrEmpty(dbType, "dbType");

            database.AddInputOutputParameter(this.cmd, parameterName, dbType, size, value);
            outParameters.Add(parameterName);

            return this;
        }

        /// <summary>
        /// 添加返回参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcedureSection AddReturnValueParameter(string parameterName, DbType dbType)
        {
            return AddReturnValueParameter(parameterName, dbType, 0);
        }

        /// <summary>
        /// 添加返回参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcedureSection AddReturnValueParameter(string parameterName, DbType dbType, int size)
        {
            Check.NotNullOrEmpty(parameterName, "parameterName");
            Check.NotNullOrEmpty(dbType, "dbType");

            database.AddReturnValueParameter(this.cmd, parameterName, dbType, size);
            outParameters.Add(parameterName);
            return this;
        }

        #endregion

        #region 执行

        /// <summary>
        /// 操作参数名称
        /// </summary>
        protected void executeBefore()
        {
            if (isParameterSpecial)
            {
                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    foreach (DbParameter dbpara in cmd.Parameters)
                    {
                        if (!string.IsNullOrEmpty(dbpara.ParameterName))
                        {
                            dbpara.ParameterName = dbpara.ParameterName.TrimStart(database.DbProvider.ParamPrefix);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public override object ToScalar()
        {
            executeBefore();

            return base.ToScalar();
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public override IDataReader ToDataReader()
        {
            executeBefore();

            return base.ToDataReader();
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public override DataSet ToDataSet()
        {
            executeBefore();

            return base.ToDataSet();
        }


        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public override int ExecuteNonQuery()
        {
            executeBefore();

            return base.ExecuteNonQuery();
        }

        #endregion
    }
}

