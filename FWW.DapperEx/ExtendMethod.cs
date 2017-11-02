using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public static class ExtendMethod
    {
        /// <summary>
        /// 将IEnumerable的对象转化为以逗号隔开的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">The values.</param>
        /// <returns>System.String.</returns>
        public static string ToStringSeparatedByComma<T>(this IEnumerable<T> values)
        {
            string result = "";
            foreach (var item in values)
            {
                result += item.ToString() + ",";
            }
            return result.TrimEnd(',');
        }
        /// <summary>
        /// 将DataTable转换为List{Object}
        /// </summary>
        /// <param name="tb">The tb.</param>
        /// <returns>IEnumerable{System.Object}.</returns>
        public static IEnumerable<object> ToObjectList(this System.Data.DataTable tb)
        {
            var arrayList = new List<object>();
            foreach (System.Data.DataRow dataRow in tb.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (System.Data.DataColumn dataColumn in tb.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }
            return arrayList;
        }
    }
}
