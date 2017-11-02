using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public abstract class BaseTest : ITest
    {
        protected Stopwatch stop;
        public BaseTest() {
            stop = new Stopwatch();
        }
        long ITest.ExcuteTime
        {
            get
            {
                return stop.ElapsedMilliseconds;
            }
        }
        object ITest.Excute(int times)
        {
            stop.Reset();
            stop.Start();
            for (int i = 0; i < times; i++)
            {
                ExcuteMain();
            }
            stop.Stop();
            return null;
        }
        public abstract void ExcuteMain();
    }
}
