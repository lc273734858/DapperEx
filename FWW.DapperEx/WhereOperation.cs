using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// where条件
    /// </summary>
    public enum WhereOperation
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual = 3,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual = 5,
        /// <summary>
        /// 为Null
        /// </summary>
        IsNull = 6,
        /// <summary>
        /// 包括
        /// </summary>
        InClude = 7,
        /// <summary>
        /// 不包括
        /// </summary>
        NotInClude=8,
        /// <summary>
        /// 相似
        /// </summary>
        Like = 9,
        /// <summary>
        /// 自定义条件
        /// </summary>
        Self=10
    }
}
