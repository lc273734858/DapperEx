using FWW.Framework.DapperExTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public class SelectHartCodeTest : BaseTest
    {
        public override void ExcuteMain()
        {
            var ss=Program.Context.Excute<TestTableEntity>("select top 5000 * from TestTable where id>@id", new { id=5000,name="11" });
        }
    }
}
