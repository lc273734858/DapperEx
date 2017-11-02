using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[Setting]")]
    public partial class SettingEntity : BaseEntity
    {
        #region StaticReadOnlyFields
        private static string tableName;
        /// <summary>
        /// 参数编号
        /// </summary>
        public static readonly Field F_SettingID;
        /// <summary>
        /// 参数描述
        /// </summary>
        public static readonly Field F_SettingDescription;
        /// <summary>
        /// 参数值
        /// </summary>
        public static readonly Field F_SettingValue;
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
        static SettingEntity()
        {
            tableName="[dbo].[Setting]";
            F_SettingID = new Field("SettingID", tableName);
            F_SettingDescription = new Field("SettingDescription", tableName);
            F_SettingValue = new Field("SettingValue", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
            F_ModifyDate = new Field("ModifyDate", tableName);
        }
        #endregion
        
        #region DataMember
        /// <summary>
        /// 参数编号
        /// </summary>
        [PrimaryKey]
        public String SettingID{get;set;}
        /// <summary>
        /// 参数描述
        /// </summary>
        public String SettingDescription{get;set;}
        
        /// <summary>
        /// 参数值
        /// </summary>
        public String SettingValue{get;set;}
        
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
            return string.Format("SettingID={0},SettingDescription={1},SettingValue={2},CreationDate={3},ModifyDate={4}",SettingID,SettingDescription,SettingValue,CreationDate,ModifyDate);
        }
        #endregion
    }
}