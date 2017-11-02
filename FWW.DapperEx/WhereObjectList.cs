using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// Where条件
    /// </summary>
    [Serializable]
    public class WhereObjectList : List<WhereObject>, ICloneable
    {
        #region Fields
        private List<WhereObjectList> _ORList;
        private List<WhereObject> _ORListObject;

        /// <summary>
        /// OR条件
        /// </summary>
        public List<WhereObject> ORListObject
        {
            get { return _ORListObject; }
            set { _ORListObject = value; }
        }
        private List<WhereObjectList> _AndList;
        /// <summary>
        /// AndWhereList
        /// </summary>
        public List<WhereObjectList> AndList
        {
            get { return _AndList; }
            set { _AndList = value; }
        }
        /// <summary>
        /// OrWhereList
        /// </summary>
        public List<WhereObjectList> ORList
        {
            get { return _ORList; }
            set { _ORList = value; }
        }
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        public WhereObjectList()
            : base()
        {
            _ORList = new List<WhereObjectList>();
            _AndList = new List<WhereObjectList>();
            _ORListObject = new List<WhereObject>();
        }
        /// <summary>
        /// 默认添加And条件的实例
        /// </summary>
        /// <param name="fieldname">The fieldname.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="value">The value.</param>
        public WhereObjectList(string fieldname, WhereOperation operation, object value)
            : this()
        {
            this.Add(fieldname, operation, value);
        }
        #endregion

        #region Method
        /// <summary>
        /// 添加And条件
        /// </summary>
        /// <param name="fieldname">字段名</param>
        /// <param name="operation">条件</param>
        /// <param name="value">值</param>
        public void Add(string fieldname, WhereOperation operation, object value)
        {
            this.Add(fieldname, operation, value, null);
        }
        /// <summary>
        /// 添加And条件
        /// </summary>
        /// <param name="fieldname">字段名</param>
        /// <param name="operation">条件</param>
        /// <param name="value">值</param>
        /// <param name="dbType">参数数据类型</param>
        public void Add(string fieldname, WhereOperation operation, object value, DbType? dbType)
        {
            this.Add(new WhereObject(fieldname, operation, value, dbType));
        }

        /// <summary>
        /// 添加字段和字段关系条件
        /// </summary>
        /// <param name="fieldname1">字段1</param>
        /// <param name="operation">条件</param>
        /// <param name="fieldname2">字段2</param>
        public void AddFieldToField(string fieldname1, WhereOperation operation, string fieldname2)
        {
            if (string.IsNullOrEmpty(fieldname1) || string.IsNullOrEmpty(fieldname2))
            {
                throw new Exception("fieldname1 or fieldname2 不能为空");
            }
            this.Add(new WhereObject("", WhereOperation.Self, string.Format("{0} {1} {2}", fieldname1, WhereObject.GetOperationStr(operation), fieldname2)));
        }
        /// <summary>
        /// 添加或者字段和字段关系条件
        /// </summary>
        /// <param name="fieldname1">字段1</param>
        /// <param name="operation">条件</param>
        /// <param name="fieldname2">字段2</param>
        public void AddORFieldToField(string fieldname1, WhereOperation operation, string fieldname2)
        {
            if (string.IsNullOrEmpty(fieldname1) || string.IsNullOrEmpty(fieldname2))
            {
                throw new Exception("fieldname1 or fieldname2 不能为空");
            }
            _ORList.Add(new WhereObjectList(new WhereObject("", WhereOperation.Self, string.Format("{0} {1} {2}", fieldname1, WhereObject.GetOperationStr(operation), fieldname2))));
        }

        /// <summary>
        /// 添加自定义条件
        /// </summary>
        /// <param name="where">where</param>
        public void AddCustom(string where)
        {
            this.Add(new WhereObject("", WhereOperation.Self, where));
        }
        /// <summary>
        /// 添加自定义条件
        /// </summary>
        /// <param name="where">where</param>
        public void AddORCustom(string where)
        {
            _ORListObject.Add(new WhereObject("", WhereOperation.Self, where));
        }
        /// <summary>
        /// 默认添加And条件
        /// </summary>
        /// <param name="where"></param>
        public WhereObjectList(WhereObject where)
            : this()
        {
            this.Add(where);
        }
        /// <summary>
        /// 添加And条件，被添加的条件会用括号包裹
        /// </summary>
        /// <param name="wherelist"></param>
        public void Add(WhereObjectList wherelist)
        {
            _AndList.Add(wherelist);
        }
        /// <summary>
        /// 添加或者条件
        /// </summary>
        /// <param name="fieldname">字段名</param>
        /// <param name="operation">条件</param>
        /// <param name="value">值</param>
        public void AddOR(string fieldname, WhereOperation operation, object value)
        {
            this.AddOR(fieldname, operation, value, null);
        }
        /// <summary>
        /// 添加或者条件
        /// </summary>
        /// <param name="fieldname">字段名</param>
        /// <param name="operation">条件</param>
        /// <param name="value">值</param>
        /// <param name="dbType">参数数据类型</param>
        public void AddOR(string fieldname, WhereOperation operation, object value, DbType? dbType)
        {
            _ORListObject.Add(new WhereObject(fieldname, operation, value, dbType));
        }
        /// <summary>
        /// 添加或者条件，被添加的条件会用括号包裹
        /// </summary>
        /// <param name="wherelist">WhereObjectList</param>
        public void AddOR(WhereObjectList wherelist)
        {
            _ORList.Add(wherelist);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        public void AddOR(WhereObject where)
        {
            _ORListObject.Add(where);
        }

        /// <summary>
        /// 所有条件的总数
        /// </summary>
        public int AllCount()
        {
            return Count + _ORList.Count + _AndList.Count + _ORListObject.Count;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 深度复制
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            WhereObjectList where = new WhereObjectList();
            if (this.AndList.Count > 0)
            {
                foreach (var item in this.AndList)
                {
                    where.AndList.Add((WhereObjectList)item.Clone());
                }
            }
            if (this.ORList.Count > 0)
            {
                foreach (var item in this.ORList)
                {
                    where.ORList.Add((WhereObjectList)item.Clone());
                }
            }
            foreach (var item in this)
            {
                where.Add(item);
            }
            return where;
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.Generic.List`1" />.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            ORList.Clear();
            AndList.Clear();
        }
        #endregion


    }
}
