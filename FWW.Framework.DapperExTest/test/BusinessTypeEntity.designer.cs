using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "BusinessType")]
    public partial class BusinessTypeEntity : BaseEntity
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
        public static readonly Field F_BusinessCode;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BusinessName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BusinessDesc;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Status;
        #endregion
        
        #region Constructor
        static BusinessTypeEntity()
        {
            tableName="BusinessType";
            F_Id = new Field("Id", tableName);
            F_BusinessCode = new Field("BusinessCode", tableName);
            F_BusinessName = new Field("BusinessName", tableName);
            F_BusinessDesc = new Field("BusinessDesc", tableName);
            F_Status = new Field("Status", tableName);
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
        public String BusinessCode{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String BusinessName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String BusinessDesc{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public UInt16? Status{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("Id={0},BusinessCode={1},BusinessName={2},BusinessDesc={3},Status={4}",Id,BusinessCode,BusinessName,BusinessDesc,Status);
        }
        #endregion
    }
}