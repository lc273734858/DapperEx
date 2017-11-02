using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 条件类
    /// </summary>
    [Serializable]
    public class WhereObject
    {
        #region Fields
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; set; }
        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public WhereOperation Operation { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }
        /// <summary>
        /// 条件数据类型
        /// </summary>
        public DbType? FieldDbType { get; set; }
        #endregion

        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="WhereObject"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public WhereObject(string fieldName)
        {
            FieldName = fieldName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WhereObject"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        public WhereObject(string fieldName, WhereOperation operation, object value)
        {
            FieldName = fieldName;
            Operation = operation;
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WhereObject" /> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        /// <param name="dbType">Type of the database.</param>
        public WhereObject(string fieldName, WhereOperation operation, object value, DbType? dbType)
            : this(fieldName, operation, value)
        {
            if (dbType != null)
            {
                FieldDbType = dbType;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.FieldName,GetOperationStr(Operation), this.Value);
        }
        /// <summary>
        /// 获取操作符
        /// </summary>
        /// <param name="operation">操作符</param>
        /// <returns></returns>
        public static string GetOperationStr(WhereOperation operation)
        {
            string result = string.Empty;
            switch (operation)
            {
                case WhereOperation.Equal:
                    result = "=";
                    break;
                case WhereOperation.NotEqual:
                    result = "<>";
                    break;
                case WhereOperation.GreaterThan:
                    result = ">";
                    break;
                case WhereOperation.GreaterEqual:
                    result = ">=";
                    break;
                case WhereOperation.LessThan:
                    result = "<";
                    break;
                case WhereOperation.LessEqual:
                    result = "<=";
                    break;
                case WhereOperation.IsNull:
                    result = "Is Null";
                    break;
                case WhereOperation.InClude:
                    result = "In";
                    break;
                case WhereOperation.NotInClude:
                    result = "Not In";
                    break;
                case WhereOperation.Like:
                    result = "like";
                    break;
                case WhereOperation.Self:
                    result = "";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;

        }

        #endregion
    }
}
