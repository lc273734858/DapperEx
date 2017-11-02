using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "OperateAction")]
    public partial class OperateActionEntity : BaseEntity
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
        public static readonly Field F_OperateCode;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OperateName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OperateDesc;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BusinessType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_NeedCreditScore;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Status;
        #endregion
        
        #region Constructor
        static OperateActionEntity()
        {
            tableName="OperateAction";
            F_Id = new Field("Id", tableName);
            F_OperateCode = new Field("OperateCode", tableName);
            F_OperateName = new Field("OperateName", tableName);
            F_OperateDesc = new Field("OperateDesc", tableName);
            F_BusinessType = new Field("BusinessType", tableName);
            F_NeedCreditScore = new Field("NeedCreditScore", tableName);
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
        public String OperateCode{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String OperateName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String OperateDesc{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String BusinessType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Decimal? NeedCreditScore{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public UInt16? Status{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("Id={0},OperateCode={1},OperateName={2},OperateDesc={3},BusinessType={4},NeedCreditScore={5},Status={6}",Id,OperateCode,OperateName,OperateDesc,BusinessType,NeedCreditScore,Status);
        }
        #endregion
    }
}