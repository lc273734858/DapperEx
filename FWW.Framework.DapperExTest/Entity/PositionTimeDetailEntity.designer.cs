using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[PositionTimeDetail]")]
    public partial class PositionTimeDetailEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        private static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BidTimeID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BidID;
        /// <summary>
        /// 商品编号
        /// </summary>
        public static readonly Field F_ProductID;
        /// <summary>
        /// 广告位
        /// </summary>
        public static readonly Field F_PositionID;
        /// <summary>
        /// 开始时间
        /// </summary>
        public static readonly Field F_BeginDateTime;
        /// <summary>
        /// 结束日期
        /// </summary>
        public static readonly Field F_EndDateTime;
        /// <summary>
        /// 在位时常（分钟）
        /// </summary>
        public static readonly Field F_PositionMinutes;
        /// <summary>
        /// 状态(1-开始 2-结束 3-系统作废)
        /// </summary>
        public static readonly Field F_Status;
        /// <summary>
        /// 是否已经结算
        /// </summary>
        public static readonly Field F_Closed;
        /// <summary>
        /// 创建日期
        /// </summary>
        public static readonly Field F_CreationDate;
        /// <summary>
        /// 修改日期
        /// </summary>
        public static readonly Field F_ModifyDate;
        #endregion
        
        #region Constructor
        static PositionTimeDetailEntity()
        {
            tableName="[dbo].[PositionTimeDetail]";
            F_BidTimeID = new Field("BidTimeID", tableName);
            F_BidID = new Field("BidID", tableName);
            F_ProductID = new Field("ProductID", tableName);
            F_PositionID = new Field("PositionID", tableName);
            F_BeginDateTime = new Field("BeginDateTime", tableName);
            F_EndDateTime = new Field("EndDateTime", tableName);
            F_PositionMinutes = new Field("PositionMinutes", tableName);
            F_Status = new Field("Status", tableName);
            F_Closed = new Field("Closed", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_ModifyDate = new Field("ModifyDate", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String BidTimeID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String BidID{get;set;}
        
        /// <summary>
        /// 商品编号
        /// </summary>
        public String ProductID{get;set;}
        
        /// <summary>
        /// 广告位
        /// </summary>
        public Int32? PositionID{get;set;}
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDateTime{get;set;}
        
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDateTime{get;set;}
        
        /// <summary>
        /// 在位时常（分钟）
        /// </summary>
        public Int32? PositionMinutes{get;set;}
        
        /// <summary>
        /// 状态(1-开始 2-结束 3-系统作废)
        /// </summary>
        public Byte? Status{get;set;}
        
        /// <summary>
        /// 是否已经结算
        /// </summary>
        public Boolean? Closed{get;set;}
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreationDate{get;set;}
        
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? ModifyDate{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("BidTimeID={0},BidID={1},ProductID={2},PositionID={3},BeginDateTime={4},EndDateTime={5},PositionMinutes={6},Status={7},Closed={8},CreationDate={9},ModifyDate={10}",BidTimeID,BidID,ProductID,PositionID,BeginDateTime,EndDateTime,PositionMinutes,Status,Closed,CreationDate,ModifyDate);
        }
        #endregion
    }
}