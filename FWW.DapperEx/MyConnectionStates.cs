using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum MyConnectionStates
    {
        /// <summary>
        /// 需要关闭连接
        /// </summary>
        NeedClose = 0,
        /// <summary>
        /// 不需要关闭连接
        /// </summary>
        NoNeedClose = 1,
        /// <summary>
        /// 关闭状态
        /// </summary>
        Closed = 2
    }
}
