using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public sealed class SqlDataParameter : Dictionary<string, object>
    {
        private const string preparamer = "@";
        public string AddParamter(string key, object value)
        {
            return AddParamter(key, value, null);
        }
        public string AddParamter(string key, object value, DbType? FieldDbType=null)
        {
            key = key.Replace(".", "").Replace("[","").Replace("]","");
            if (ContainsKey(key))
            {
                key += Count;
            }
            base.Add(key, value);
            return preparamer+key;
        }
        public void Concat(Dictionary<string, object> paramters)
        {
            foreach (var item in paramters)
            {
                this.AddParamter(item.Key, item.Value);
            }
        }

        internal string AddFieldParamter(Field field,object value)
        {
            return AddParamter(field.FieldName, value);
        }

        internal void AddRange(SqlDataParameter parameters)
        {
            this.Concat(parameters);
        }
    }
}
