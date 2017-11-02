using System;
using System.Runtime.Serialization;
using FWW.Framework.DapperEx;

namespace FWW.Framework.DapperExTest.Entity
{
    [Table(Name = "[dbo].[TestTable]")]
    public partial class TestTableEntity : BaseEntity
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
        public static readonly Field F_Name;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name1;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name2;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name3;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name4;
        /// <summary>
        /// 
        /// </summary>
        public static readonly Field F_Name5;
        /// <summary>
        /// 创建日期
        /// </summary>
        public static readonly Field F_CreationDate;
        #endregion
        
        #region Constructor
        static TestTableEntity()
        {
            tableName="[dbo].[TestTable]";
            F_ID = new Field("ID", tableName);
            F_Name = new Field("Name", tableName);
            F_Name1 = new Field("Name1", tableName);
            F_Name2 = new Field("Name2", tableName);
            F_Name3 = new Field("Name3", tableName);
            F_Name4 = new Field("Name4", tableName);
            F_Name5 = new Field("Name5", tableName);
            F_CreationDate = new Field("CreationDate", tableName);
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
        public String Name{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name1{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name2{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name3{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name4{get;set;}
        
        /// <summary>
        /// 
        /// </summary>
        public String Name5{get;set;}
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreationDate{get;set;}
        
        #endregion
        
        #region Method
        public new string ToString()
        {
            return string.Format("ID={0},Name={1},Name1={2},Name2={3},Name3={4},Name4={5},Name5={6},CreationDate={7}",ID,Name,Name1,Name2,Name3,Name4,Name5,CreationDate);
        }
        #endregion
    }
}