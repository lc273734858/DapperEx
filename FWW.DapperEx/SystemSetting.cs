using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// 重新加载接口
    /// </summary>
    public interface IReload
    {
        /// <summary>
        /// 重新加载
        /// </summary>
        void Reload();
        /// <summary>
        /// 从数据库加载
        /// </summary>
        void LoadFromDataBase();
    }
    /// <summary>
    /// Class SystemSetting.
    /// </summary>
    public static class SystemSetting
    {
        #region Fields
        /// <summary>
        /// 需要重新加载的实体
        /// </summary>
        public static SystemTasks<IReload> LoadTasks; 
        #endregion

        #region Constructor
        static SystemSetting()
        {
            LoadTasks = new SystemTasks<IReload>();
        } 
        #endregion

        #region Method
        /// <summary>
        /// 重新加载设置
        /// </summary>
        public static void ReLaodSet()
        {
            lock (new object())
            {
                LoadTasks.Sort();
                foreach (var item in LoadTasks)
                {
                    ((IReload)item.Task).Reload();
                }
            }
        }
        /// <summary>
        /// 从数据库加载设置
        /// </summary>
        public static void LoadTaskFromDataBase()
        {
            lock (new object())
            {
                LoadTasks.Sort();
                foreach (var item in LoadTasks)
                {
                    ((IReload)item.Task).LoadFromDataBase();
                }
            }
        }
        #endregion
    }
}
