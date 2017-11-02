using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "CreditSocreHistory")]
    public partial class CreditSocreHistoryEntity : BaseEntity
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
        public static readonly Field F_UserId;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_LoginId;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_CreditSource;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Score;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UpdateDate;
        #endregion
        
        #region Constructor
        static CreditSocreHistoryEntity()
        {
            tableName="CreditSocreHistory";
            F_Id = new Field("Id", tableName);
            F_UserId = new Field("UserId", tableName);
            F_LoginId = new Field("LoginId", tableName);
            F_CreditSource = new Field("CreditSource", tableName);
            F_Score = new Field("Score", tableName);
            F_UpdateDate = new Field("UpdateDate", tableName);
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
        public String UserId{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String LoginId{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String CreditSource{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Decimal? Score{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateDate{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("Id={0},UserId={1},LoginId={2},CreditSource={3},Score={4},UpdateDate={5}",Id,UserId,LoginId,CreditSource,Score,UpdateDate);
        }
        #endregion
    }
}