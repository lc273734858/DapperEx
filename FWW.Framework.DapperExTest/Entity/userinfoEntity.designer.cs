using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "userinfo")]
    public partial class userinfoEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        protected static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserName;
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
        static userinfoEntity()
        {
            tableName="userinfo";
            F_UserID = new Field("UserID", tableName);
            F_UserName = new Field("UserName", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_tag = new Field("tag", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String UserID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String UserName{get;set;}
        
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
            return string.Format("UserID={0},UserName={1},CreationDate={2},tag={3}",UserID,UserName,CreationDate,tag);
        }
        #endregion
    }
}