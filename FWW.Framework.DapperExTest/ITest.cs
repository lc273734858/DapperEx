using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public interface ITest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="times">执行次数</param>
        /// <returns></returns>
        object Excute(int times);
        long ExcuteTime { get; }
    }
}
