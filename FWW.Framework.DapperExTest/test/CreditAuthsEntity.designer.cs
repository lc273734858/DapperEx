using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace CreditCenter.Domain
{
    [Table(Name = "CreditAuths")]
    public partial class CreditAuthsEntity : BaseEntity
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
        public static readonly Field F_UserName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_RealName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_IdNumber;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_MobileNumber;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_CreditSource;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_AuthStatus;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_StatusUpdateTime;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_CreditScore;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ScoreUpdateTime;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OpenId;
        #endregion
        
        #region Constructor
        static CreditAuthsEntity()
        {
            tableName="CreditAuths";
            F_Id = new Field("Id", tableName);
            F_UserId = new Field("UserId", tableName);
            F_UserName = new Field("UserName", tableName);
            F_RealName = new Field("RealName", tableName);
            F_IdNumber = new Field("IdNumber", tableName);
            F_MobileNumber = new Field("MobileNumber", tableName);
            F_CreditSource = new Field("CreditSource", tableName);
            F_AuthStatus = new Field("AuthStatus", tableName);
            F_StatusUpdateTime = new Field("StatusUpdateTime", tableName);
            F_CreditScore = new Field("CreditScore", tableName);
            F_ScoreUpdateTime = new Field("ScoreUpdateTime", tableName);
            F_OpenId = new Field("OpenId", tableName);
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
        public String UserName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String RealName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String IdNumber{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String MobileNumber{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String CreditSource{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Int32? AuthStatus{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StatusUpdateTime{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Decimal? CreditScore{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ScoreUpdateTime{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String OpenId{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("Id={0},UserId={1},UserName={2},RealName={3},IdNumber={4},MobileNumber={5},CreditSource={6},AuthStatus={7},StatusUpdateTime={8},CreditScore={9},ScoreUpdateTime={10},OpenId={11}",Id,UserId,UserName,RealName,IdNumber,MobileNumber,CreditSource,AuthStatus,StatusUpdateTime,CreditScore,ScoreUpdateTime,OpenId);
        }
        #endregion
    }
}