using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 
    /// </summary>
    public class Parameters : Dictionary<string, object>
    {
        /// <summary>
        /// 
        /// </summary>
        public Parameters()
        {
            items = new List<Parameter>();
        }
        private List<Parameter> items;
        public List<Parameter> Items
        {
            get { return items; }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parameters"></param>
        public void AddRange(IEnumerable<Parameter> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                foreach (var item in parameters)
                {
                    this.Add(item.ParameterName, item.ParameterValue);
                }
                this.items.AddRange(parameters);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public void AddRange(Parameters parameters)
        {
            if (parameters != null && parameters.Any())
            {
                foreach (var item in parameters.Items)
                {
                    this.Add(item.ParameterName, item.ParameterValue);
                }
                this.items.AddRange(parameters.Items);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void Add(Parameter p)
        {
            this.items.Add(p);
            base.Add(p.ParameterName, p.ParameterValue);
        }
        /// <summary>
        /// 自动添加@前缀
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AutoAddPreChar(string key, object value)
        {
            this.Add(string.Concat("@", key), value);
        }
    }
}
