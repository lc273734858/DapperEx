using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "userinfoex")]
    public partial class userinfoexEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        protected static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_CreationDate;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_tag;
        #endregion
        
        #region Constructor
        static userinfoexEntity()
        {
            tableName="userinfoex";
            F_ID = new Field("ID", tableName);
            F_UserID = new Field("UserID", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_tag = new Field("tag", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String ID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String UserID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreationDate{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Int32? tag{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("ID={0},UserID={1},CreationDate={2},tag={3}",ID,UserID,CreationDate,tag);
        }
        #endregion
    }
}