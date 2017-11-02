using FWW.Framework.DapperExTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public class SelectTest : BaseTest
    {
        public override void ExcuteMain()
        {
            var list = Program.Context.From<TestTableEntity>().Top(5000).Where(p => (p.ID > 5000||p.Name=="11"||p.Name1=="22")&&p.ID>5000)
                .ToEnumerable();
            //Console.WriteLine(Program.Context.LastSqlExpress);
        }
    }
}
