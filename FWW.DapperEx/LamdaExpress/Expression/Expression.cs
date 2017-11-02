#region << 版 本 注 释 >>
/****************************************************
* 文 件 名：
* Copyright(c) ITdos
* CLR 版本: 4.0.30319.18408
* 创 建 人：steven hu
* 电子邮箱：
* 官方网站：www.ITdos.com
* 创建日期：2010/4/8 19:55:23
* 文件描述：
******************************************************
* 修 改 人：
* 修改日期：
* 备注描述：
*******************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FWW.Framework.DapperEx;
using System.Data;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 条件类型
    /// </summary>
    public enum WhereType
    {
        /// <summary>
        /// join where
        /// </summary>
        JoinWhere,
        /// <summary>
        /// 常规Where
        /// </summary>
        Where
    }

    /// <summary>
    /// 比较类型
    /// </summary>
    public enum QueryOperator : byte
    {
        /// <summary>
        /// ==
        /// </summary>
        Equal,

        /// <summary>
        /// &lt;&gt; 、 !=、不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// >
        /// </summary>
        Greater,

        /// <summary>
        /// &lt; 小于
        /// </summary>
        Less,

        /// <summary>
        /// >=
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// &lt;= 小于等于
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// LIKE
        /// </summary>
        Like,

        /// <summary>
        /// &
        /// </summary>
        BitwiseAND,

        /// <summary>
        /// |
        /// </summary>
        BitwiseOR,

        /// <summary>
        /// ^
        /// </summary>
        BitwiseXOR,

        /// <summary>
        /// ~
        /// </summary>
        BitwiseNOT,

        /// <summary>
        /// IS NULL
        /// </summary>
        IsNULL,

        /// <summary>
        /// IS NOT NULL
        /// </summary>
        IsNotNULL,

        /// <summary>
        ///  +
        /// </summary>
        Add,

        /// <summary>
        /// -
        /// </summary>
        Subtract,


        /// <summary>
        /// *
        /// </summary>
        Multiply,

        /// <summary>
        /// /
        /// </summary>
        Divide,

        /// <summary>
        /// %
        /// </summary>
        Modulo,
        /// <summary>
        /// in
        /// </summary>
        In,
        /// <summary>
        /// not in
        /// </summary>
        NotIn
    }
    public interface IWhere
    {
        string GenerateExpress(SqlDataParameter sqldataparamters);
    }
    /// <summary>
    /// 表达式
    /// </summary>
    [Serializable]
    public class Expression: IWhere
    {
        protected Field _field;
        protected object _value;
        protected QueryOperator _oper;
        protected bool _isFieldBefore;
        protected WhereClip AndClip;
        protected WhereClip OrClip;
        /// <summary>
        /// 
        /// </summary>
        protected string expressionString = string.Empty;

        /// <summary>
        /// 参数
        /// </summary>
        protected SqlDataParameter parameters = new SqlDataParameter();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Expression()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expressionString"></param>
        public Expression(string expressionString)
        {
            this.expressionString = expressionString;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expressionString"></param>
        /// <param name="sqlparameters"></param>
        public Expression(string expressionString, SqlDataParameter sqlparameters)
        {
            if (!string.IsNullOrEmpty(expressionString))
            {
                this.expressionString = expressionString;

                if (null != sqlparameters && sqlparameters.Count > 0)
                    this.parameters.AddRange(sqlparameters);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="oper"></param>
        public Expression(Field field, object value, QueryOperator oper)
            : this(field, value, oper, true)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <param name="oper">操作符</param>
        /// <param name="isFieldBefore">字段放前面</param>
        public Expression(Field field, object value, QueryOperator oper, bool isFieldBefore)
        {
            _field = field;
            _value=value;
            _oper = oper;
            _isFieldBefore = isFieldBefore;
        }
        /// <summary>
        /// 生成表达式
        /// </summary>
        /// <param name="sqldataparamters"></param>
        /// <returns></returns>
        public string GenerateExpress(SqlDataParameter sqldataparamters)
        {
            if (Field.IsNullOrEmpty(_field)==false)
            {
                string valuestring = null;
                if (_value is Expression)
                {
                    Expression expression = (Expression)_value;
                    valuestring =string.Concat("(",expression.GenerateExpress(sqldataparamters),")");
                    //parameters.AddRange(expression.Parameters);
                }
                else if (_value is Field)
                {
                    Field fieldValue = (Field)_value;
                    valuestring = fieldValue.TableFieldName;
                }
                else
                {
                    //valuestring = DataUtils.MakeUniqueKey(_field);
                    ////valuestring = field.tableName + field.Name;
                    //Parameter p = new Parameter(valuestring, _value, _field.ParameterDbType, _field.ParameterSize);
                    //parameters.Add(p);
                    valuestring = sqldataparamters.AddFieldParamter(_field, _value);
                }

                if (_isFieldBefore)
                {
                    this.expressionString = string.Concat(_field.TableFieldName, DataUtils.ToString(_oper), valuestring);
                }
                else
                {
                    this.expressionString = string.Concat(valuestring, DataUtils.ToString(_oper), _field.TableFieldName);
                } 
            }
            if (AndClip != null)
            {
                this.expressionString = string.Concat("(", this.expressionString, ") and (", AndClip.GenerateExpress(sqldataparamters)+")");
            }
            if (OrClip!=null)
            {
                this.expressionString = string.Concat("(",this.expressionString, ") or (", OrClip.GenerateExpress(sqldataparamters)+")");
            }
            return this.expressionString;
        }

        /// <summary>
        /// 返回参数
        /// </summary>
        internal SqlDataParameter Parameters
        {
            get
            {
                return parameters;
            }
        }


        /// <summary>
        /// 返回组合字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            throw new Exception("次方法废弃不用");
            return expressionString;
        }

    }
}
