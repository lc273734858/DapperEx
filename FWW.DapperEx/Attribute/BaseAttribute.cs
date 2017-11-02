using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    public class BaseAttribute : Attribute
    {
        /// <summary>
        /// 对应数据字段/表名
        /// </summary>
        public string Name { get; set; }
    }
}
