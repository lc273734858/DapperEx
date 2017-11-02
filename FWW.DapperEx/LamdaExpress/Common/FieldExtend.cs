using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    public static class FieldExtend
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Tips = "该方法({0})只能用于Dos.ORM lambda表达式！";

        /// <summary>
        /// 
        /// </summary>
        //public static object All(this object key)
        //{
        //    throw new Exception(Tips);
        //}

        /// <summary>
        /// like '%value%' 模糊查询，同Contains。
        /// </summary>
        public static bool Like(this object key, object values)
        {
            throw new Exception(string.Format(Tips, "Like"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// IS NULL
        /// </summary>
        public static bool IsNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNull"));
        }
        /// <summary>
        /// IS NOT NULL
        /// </summary>
        public static bool IsNotNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNotNull"));
        }
        /// <summary>
        /// As
        /// </summary>
        public static bool As(this object key, string values)
        {
            throw new Exception(string.Format(Tips, "As"));
        }
        /// <summary>
        /// Sum
        /// </summary>
        public static decimal Sum(this object key)
        {
            throw new Exception(string.Format(Tips, "Sum"));
        }
        /// <summary>
        /// Count
        /// </summary>
        public static int Count(this object key)
        {
            throw new Exception(string.Format(Tips, "Count"));
        }
        /// <summary>
        /// Avg
        /// </summary>
        public static decimal Avg(this object key)
        {
            throw new Exception(string.Format(Tips, "Avg"));
        }
        /// <summary>
        /// Len
        /// </summary>
        public static int Len(this object key)
        {
            throw new Exception(string.Format(Tips, "Len"));
        }
    }
}