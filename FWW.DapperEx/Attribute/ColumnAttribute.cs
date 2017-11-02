using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public class ColumnAttribute : BaseAttribute {      
    }
    /// <summary>
    /// Class PrimaryKeyAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PrimaryKeyAttribute : BaseAttribute {
        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }
    }
    /// <summary>
    /// 忽略字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreAttribute : BaseAttribute
    {
        /// <summary>
        /// 忽略所有，即不出现在查询，也不出现在更新
        /// </summary>
        public bool All { get; set; }
    }
    /// <summary>
    /// 数据库表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : BaseAttribute
    {
    }
    /// <summary>
    /// 生成SQL时参数里面的列名和对应值名称
    /// </summary>
    public class ParamColumnMapping
    {
        /// <summary>
        /// 数据库列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 对应类属性名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">实体对应名</param>
        /// <param name="colName">字段对应名</param>
        public ParamColumnMapping(string name, string colName) {
            FieldName = name;
            ColumnName = colName;
        }
    }
}
