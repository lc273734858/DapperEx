using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[BidOrderRecord]")]
    public partial class BidOrderRecordEntity : BaseEntity
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
        /// 
        /// </summary>
        public static readonly Field F_BidTimeID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ProductID;
        /// <summary>
        /// 推广费
        /// </summary>
        public static readonly Field F_Commission;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Status;
        /// <summary>
        /// 0-创建 1-删除
        /// </summary>
        public static readonly Field F_OrderCreationDate;
        /// <summary>
        /// 0-创建 1-删除
        /// </summary>
        public static readonly Field F_OrderFinishDate;
        /// <summary>
        /// 创建日期
        /// </summary>
        public static readonly Field F_CreationDate;
        #endregion
        
        #region Constructor
        static BidOrderRecordEntity()
        {
            tableName="[dbo].[BidOrderRecord]";
            F_OrderID = new Field("OrderID", tableName);
            F_BidID = new Field("BidID", tableName);
            F_BidTimeID = new Field("BidTimeID", tableName);
            F_ProductID = new Field("ProductID", tableName);
            F_Commission = new Field("Commission", tableName);
            F_Status = new Field("Status", tableName);
            F_OrderCreationDate = new Field("OrderCreationDate", tableName);
            F_OrderFinishDate = new Field("OrderFinishDate", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
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
        /// 
        /// </summary>
        public String BidTimeID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ProductID{get;set;}
        
        /// <summary>
        /// 推广费
        /// </summary>
        public Decimal? Commission{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public Byte? Status{get;set;}
        
        /// <summary>
        /// 0-创建 1-删除
        /// </summary>
        public DateTime? OrderCreationDate{get;set;}
        
        /// <summary>
        /// 0-创建 1-删除
        /// </summary>
        public DateTime? OrderFinishDate{get;set;}
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreationDate{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("OrderID={0},BidID={1},BidTimeID={2},ProductID={3},Commission={4},Status={5},OrderCreationDate={6},OrderFinishDate={7},CreationDate={8}",OrderID,BidID,BidTimeID,ProductID,Commission,Status,OrderCreationDate,OrderFinishDate,CreationDate);
        }
        #endregion
    }
}