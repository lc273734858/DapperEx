using FWW.Framework.DapperExTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public class SelectDosORM : BaseTest
    {
        public override void ExcuteMain()
        {
            var context = new Dos.ORM.DbSession("DosConnLocal");
            context.From("TestTable").Top(5000).ToDataTable();
            //context.From<TestTableEntity>().Top(500).ToList();
        }
    }
}
