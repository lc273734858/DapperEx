using FWW.Framework.DapperEx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperExTest
{
    public class HardCodeTest : BaseTest
    {
        public override void ExcuteMain()
        {
            var db = new Database(ProviderFactory.CreateDbProvider("DosConnLocal"));
            var cmd=db.GetConnection().CreateCommand();
            cmd.CommandText = "select top 5000 * from TestTable where id>@id";
            var paramter=cmd.CreateParameter();
            paramter.ParameterName = "@id";
            paramter.Value = 5000;
            cmd.Parameters.Add(paramter);
            var set = new DataSet();
            using (DbDataAdapter adapter =db.DbProviderFactory.CreateDataAdapter())
            {
                ((IDbDataAdapter)adapter).SelectCommand = cmd;
                adapter.Fill(set);
            }
        }
        
    }
}
