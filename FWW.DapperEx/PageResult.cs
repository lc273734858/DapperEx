using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    [DataContract]
    public class PageResult<T>
    {
        [DataMember]
        public IEnumerable<T> datas{get;set;}
        [DataMember]
        public int total { get; set; } 
    }
    /// <summary>
    /// DataTable 直接转换的结果实体
    /// </summary>
    public class PageTableResult
    {
        public IEnumerable<object> datas { get; set; }
        public int total { get; set; }
        public void SetDatas(System.Data.DataTable tb)
        {            
            datas = tb.ToObjectList();
        }
    }
}
