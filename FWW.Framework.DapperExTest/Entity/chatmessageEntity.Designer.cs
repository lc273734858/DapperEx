using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace ChatMessage.Entity
{
    [Table(Name = "chatmessage")]
    public partial class chatmessageEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        protected static string tableName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_MessageID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_UserType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_WorkOrder;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OrderID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_OrderType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_SenderID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_SenderName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_SendTime;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ReciveID;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ReciveName;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ReciveTime;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_MsgType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Msg;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_BU;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ChannelType;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Status;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_CreateDate;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ModifiyDate;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_ClientType;
        #endregion
        
        #region Constructor
        static chatmessageEntity()
        {
            tableName="chatmessage";
            F_ID = new Field("ID", tableName);
            F_MessageID = new Field("MessageID", tableName);
            F_UserID = new Field("UserID", tableName);
            F_UserName = new Field("UserName", tableName);
            F_UserType = new Field("UserType", tableName);
            F_WorkOrder = new Field("WorkOrder", tableName);
            F_OrderID = new Field("OrderID", tableName);
            F_OrderType = new Field("OrderType", tableName);
            F_SenderID = new Field("SenderID", tableName);
            F_SenderName = new Field("SenderName", tableName);
            F_SendTime = new Field("SendTime", tableName);
            F_ReciveID = new Field("ReciveID", tableName);
            F_ReciveName = new Field("ReciveName", tableName);
            F_ReciveTime = new Field("ReciveTime", tableName);
            F_MsgType = new Field("MsgType", tableName);
            F_Msg = new Field("Msg", tableName);
            F_BU = new Field("BU", tableName);
            F_ChannelType = new Field("ChannelType", tableName);
            F_Status = new Field("Status", tableName);
            F_CreateDate = new Field("CreateDate", tableName);
            F_ModifiyDate = new Field("ModifiyDate", tableName);
            F_ClientType = new Field("ClientType", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public Int32? ID{get;set;}
        /// <summary>
        /// 
        /// </summary>
        public String MessageID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String UserID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String UserName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String UserType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String WorkOrder{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String OrderID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String OrderType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String SenderID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String SenderName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SendTime{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ReciveID{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ReciveName{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReciveTime{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String MsgType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Msg{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String BU{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ChannelType{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public SByte? Status{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiyDate{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String ClientType{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("ID={0},MessageID={1},UserID={2},UserName={3},UserType={4},WorkOrder={5},OrderID={6},OrderType={7},SenderID={8},SenderName={9},SendTime={10},ReciveID={11},ReciveName={12},ReciveTime={13},MsgType={14},Msg={15},BU={16},ChannelType={17},Status={18},CreateDate={19},ModifiyDate={20},ClientType={21}",ID,MessageID,UserID,UserName,UserType,WorkOrder,OrderID,OrderType,SenderID,SenderName,SendTime,ReciveID,ReciveName,ReciveTime,MsgType,Msg,BU,ChannelType,Status,CreateDate,ModifiyDate,ClientType);
        }
        #endregion
    }
}