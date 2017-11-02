using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[BidRecord]")]
    public partial class BidRecordEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        private static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BidID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameArea;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameAreaName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameService;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameServiceName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ProductType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ProductID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ProductName;
        /// <summary>
        /// 状态（0-没有参加竞价 1-已经提交押金 2-竞价中 3-暂停排位  6-成功出售 10-取消排位）
        /// </summary>
        public static readonly Field F_Status;
        /// <summary>
        /// 是否激活
        /// </summary>
        public static readonly Field F_Active;
        /// <summary>
        /// 用户编号
        /// </summary>
        public static readonly Field F_UserID;
        /// <summary>
        /// 商户姓名
        /// </summary>
        public static readonly Field F_UserName;
        /// <summary>
        /// 产品价格
        /// </summary>
        public static readonly Field F_ProductPrice;
        /// <summary>
        /// 佣金比例
        /// </summary>
        public static readonly Field F_CommissionRate;
        /// <summary>
        /// 佣金
        /// </summary>
        public static readonly Field F_Commission;
        /// <summary>
        /// 占位总时间（小时）
        /// </summary>
        public static readonly Field F_ExpireHour;
        /// <summary>
        /// 剩余多少占位时间（分钟）
        /// </summary>
        public static readonly Field F_ExpireMinutes;
        /// <summary>
        /// 占位失效时间
        /// </summary>
        public static readonly Field F_ExpireDate;
        /// <summary>
        /// 最大失效时间
        /// </summary>
        public static readonly Field F_ExpireDateMax;
        /// <summary>
        /// 取消竞价原因(1-用户取消 2-规定时间到期 3-发布单下架 5-系统重启)
        /// </summary>
        public static readonly Field F_CancelReason;
        /// <summary>
        /// 结算状态 0-未结算 1-结算中 2-成功结算
        /// </summary>
        public static readonly Field F_CleareStatus;
        /// <summary>
        /// 创建日期
        /// </summary>
        public static readonly Field F_CreationDate;
        /// <summary>
        /// 修改日期
        /// </summary>
        public static readonly Field F_ModifyDate;
        /// <summary>
        /// 产品类型名称
        /// </summary>
        public static readonly Field F_ProductTypeName;
        #endregion
        
        #region Constructor
        static BidRecordEntity()
        {
            tableName="[dbo].[BidRecord]";
            F_BidID = new Field("BidID", tableName);
            F_GameID = new Field("GameID", tableName);
            F_GameName = new Field("GameName", tableName);
            F_GameArea = new Field("GameArea", tableName);
            F_GameAreaName = new Field("GameAreaName", tableName);
            F_GameService = new Field("GameService", tableName);
            F_GameServiceName = new Field("GameServiceName", tableName);
            F_ProductType = new Field("ProductType", tableName);
            F_ProductID = new Field("ProductID", tableName);
            F_ProductName = new Field("ProductName", tableName);
            F_Status = new Field("Status", tableName);
            F_Active = new Field("Active", tableName);
            F_UserID = new Field("UserID", tableName);
            F_UserName = new Field("UserName", tableName);
            F_ProductPrice = new Field("ProductPrice", tableName);
            F_CommissionRate = new Field("CommissionRate", tableName);
            F_Commission = new Field("Commission", tableName);
            F_ExpireHour = new Field("ExpireHour", tableName);
            F_ExpireMinutes = new Field("ExpireMinutes", tableName);
            F_ExpireDate = new Field("ExpireDate", tableName);
            F_ExpireDateMax = new Field("ExpireDateMax", tableName);
            F_CancelReason = new Field("CancelReason", tableName);
            F_CleareStatus = new Field("CleareStatus", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_ModifyDate = new Field("ModifyDate", tableName);
            F_ProductTypeName = new Field("ProductTypeName", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String BidID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String GameID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameArea{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameAreaName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameService{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameServiceName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ProductType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ProductID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ProductName{get;set;}
        
        /// <summary>
        /// 状态（0-没有参加竞价 1-已经提交押金 2-竞价中 3-暂停排位  6-成功出售 10-取消排位）
        /// </summary>
        public Byte? Status{get;set;}
        
        /// <summary>
        /// 是否激活
        /// </summary>
        public Boolean? Active{get;set;}
        
        /// <summary>
        /// 用户编号
        /// </summary>
        public String UserID{get;set;}
        
        /// <summary>
        /// 商户姓名
        /// </summary>
        public String UserName{get;set;}
        
        /// <summary>
        /// 产品价格
        /// </summary>
        public Decimal? ProductPrice{get;set;}
        
        /// <summary>
        /// 佣金比例
        /// </summary>
        public Decimal? CommissionRate{get;set;}
        
        /// <summary>
        /// 佣金
        /// </summary>
        public Decimal? Commission{get;set;}
        
        /// <summary>
        /// 占位总时间（小时）
        /// </summary>
        public Int32? ExpireHour{get;set;}
        
        /// <summary>
        /// 剩余多少占位时间（分钟）
        /// </summary>
        public Int32? ExpireMinutes{get;set;}
        
        /// <summary>
        /// 占位失效时间
        /// </summary>
        public DateTime? ExpireDate{get;set;}
        
        /// <summary>
        /// 最大失效时间
        /// </summary>
        public DateTime? ExpireDateMax{get;set;}
        
        /// <summary>
        /// 取消竞价原因(1-用户取消 2-规定时间到期 3-发布单下架 5-系统重启)
        /// </summary>
        public Byte? CancelReason{get;set;}
        
        /// <summary>
        /// 结算状态 0-未结算 1-结算中 2-成功结算
        /// </summary>
        public Byte? CleareStatus{get;set;}
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreationDate{get;set;}
        
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? ModifyDate{get;set;}
        
        /// <summary>
        /// 产品类型名称
        /// </summary>
        public String ProductTypeName{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("BidID={0},GameID={1},GameName={2},GameArea={3},GameAreaName={4},GameService={5},GameServiceName={6},ProductType={7},ProductID={8},ProductName={9},Status={10},Active={11},UserID={12},UserName={13},ProductPrice={14},CommissionRate={15},Commission={16},ExpireHour={17},ExpireMinutes={18},ExpireDate={19},ExpireDateMax={20},CancelReason={21},CleareStatus={22},CreationDate={23},ModifyDate={24},ProductTypeName={25}",BidID,GameID,GameName,GameArea,GameAreaName,GameService,GameServiceName,ProductType,ProductID,ProductName,Status,Active,UserID,UserName,ProductPrice,CommissionRate,Commission,ExpireHour,ExpireMinutes,ExpireDate,ExpireDateMax,CancelReason,CleareStatus,CreationDate,ModifyDate,ProductTypeName);
        }
        #endregion
    }
}