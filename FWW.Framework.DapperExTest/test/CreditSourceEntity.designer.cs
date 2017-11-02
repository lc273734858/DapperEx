using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "CreditSource")]
    public partial class CreditSourceEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        protected static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Id;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Code;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_AuthURL;
        #endregion
        
        #region Constructor
        static CreditSourceEntity()
        {
            tableName="CreditSource";
            F_Id = new Field("Id", tableName);
            F_Code = new Field("Code", tableName);
            F_Name = new Field("Name", tableName);
            F_AuthURL = new Field("AuthURL", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String Id{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String Code{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String AuthURL{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("Id={0},Code={1},Name={2},AuthURL={3}",Id,Code,Name,AuthURL);
        }
        #endregion
    }
}