using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[PrementPayOrder]")]
    public partial class PrementPayOrderEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        private static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OrderID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BidID;
        /// <summary>
        /// 发布单号
        /// </summary>
        public static readonly Field F_ProductID;
        /// <summary>
        /// 预付款
        /// </summary>
        public static readonly Field F_PrementMoney;
        /// <summary>
        /// 返还预付费
        /// </summary>
        public static readonly Field F_BackMoney;
        /// <summary>
        /// 占位费
        /// </summary>
        public static readonly Field F_ActualMoney;
        /// <summary>
        /// 状态 1-创建 2-成功支付预付款 3-退款中 5-订单完成 6-订单取消
        /// </summary>
        public static readonly Field F_Status;
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
        static PrementPayOrderEntity()
        {
            tableName="[dbo].[PrementPayOrder]";
            F_OrderID = new Field("OrderID", tableName);
            F_BidID = new Field("BidID", tableName);
            F_ProductID = new Field("ProductID", tableName);
            F_PrementMoney = new Field("PrementMoney", tableName);
            F_BackMoney = new Field("BackMoney", tableName);
            F_ActualMoney = new Field("ActualMoney", tableName);
            F_Status = new Field("Status", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_ModifyDate = new Field("ModifyDate", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public String OrderID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String BidID{get;set;}
        
        /// <summary>
        /// 发布单号
        /// </summary>
        public String ProductID{get;set;}
        
        /// <summary>
        /// 预付款
        /// </summary>
        public Decimal? PrementMoney{get;set;}
        
        /// <summary>
        /// 返还预付费
        /// </summary>
        public Decimal? BackMoney{get;set;}
        
        /// <summary>
        /// 占位费
        /// </summary>
        public Decimal? ActualMoney{get;set;}
        
        /// <summary>
        /// 状态 1-创建 2-成功支付预付款 3-退款中 5-订单完成 6-订单取消
        /// </summary>
        public Byte? Status{get;set;}
        
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
            return string.Format("OrderID={0},BidID={1},ProductID={2},PrementMoney={3},BackMoney={4},ActualMoney={5},Status={6},CreationDate={7},ModifyDate={8}",OrderID,BidID,ProductID,PrementMoney,BackMoney,ActualMoney,Status,CreationDate,ModifyDate);
        }
        #endregion
    }
}