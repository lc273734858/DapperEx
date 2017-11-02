using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[ADPositionPriceSet]")]
    public partial class ADPositionPriceSetEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        protected static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ADPositionPriceSetID;
        /// <summary>
        /// 游戏编号
        /// </summary>
        public static readonly Field F_GameID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_GameName;
        /// <summary>
        /// 商品类型(equipment-装备 gamemoney-游戏币 account-游戏账号)
        /// </summary>
        public static readonly Field F_ProductType;
        /// <summary>
        /// 广告位价格
        /// </summary>
        public static readonly Field F_Position1Price;
        /// <summary>
        /// 广告位价格
        /// </summary>
        public static readonly Field F_Position2Price;
        /// <summary>
        /// 广告位价格
        /// </summary>
        public static readonly Field F_Position3Price;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BidDescription;
        /// <summary>
        /// 失效时间（小时）
        /// </summary>
        public static readonly Field F_Expirehour;
        /// <summary>
        /// 状态 1-启用 2-关闭 3-删除
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
        /// <summary>
        /// 类型名称
        /// </summary>
        public static readonly Field F_ProductTypeName;
        #endregion
        
        #region Constructor
        static ADPositionPriceSetEntity()
        {
            tableName="[dbo].[ADPositionPriceSet]";
            F_ADPositionPriceSetID = new Field("ADPositionPriceSetID", tableName);
            F_GameID = new Field("GameID", tableName);
            F_GameName = new Field("GameName", tableName);
            F_ProductType = new Field("ProductType", tableName);
            F_Position1Price = new Field("Position1Price", tableName);
            F_Position2Price = new Field("Position2Price", tableName);
            F_Position3Price = new Field("Position3Price", tableName);
            F_BidDescription = new Field("BidDescription", tableName);
            F_Expirehour = new Field("Expirehour", tableName);
            F_Status = new Field("Status", tableName);
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
        public String ADPositionPriceSetID{get;set;}
        /// <summary>
        /// 游戏编号
        /// </summary>
        public String GameID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String GameName{get;set;}
        
        /// <summary>
        /// 商品类型(equipment-装备 gamemoney-游戏币 account-游戏账号)
        /// </summary>
        public String ProductType{get;set;}
        
        /// <summary>
        /// 广告位价格
        /// </summary>
        public Decimal? Position1Price{get;set;}
        
        /// <summary>
        /// 广告位价格
        /// </summary>
        public Decimal? Position2Price{get;set;}
        
        /// <summary>
        /// 广告位价格
        /// </summary>
        public Decimal? Position3Price{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String BidDescription{get;set;}
        
        /// <summary>
        /// 失效时间（小时）
        /// </summary>
        public Int32? Expirehour{get;set;}
        
        /// <summary>
        /// 状态 1-启用 2-关闭 3-删除
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
        
        /// <summary>
        /// 类型名称
        /// </summary>
        public String ProductTypeName{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("ADPositionPriceSetID={0},GameID={1},GameName={2},ProductType={3},Position1Price={4},Position2Price={5},Position3Price={6},BidDescription={7},Expirehour={8},Status={9},CreationDate={10},ModifyDate={11},ProductTypeName={12}",ADPositionPriceSetID,GameID,GameName,ProductType,Position1Price,Position2Price,Position3Price,BidDescription,Expirehour,Status,CreationDate,ModifyDate,ProductTypeName);
        }
        #endregion
    }
}