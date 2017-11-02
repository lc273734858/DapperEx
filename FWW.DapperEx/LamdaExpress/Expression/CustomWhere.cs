using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperEx
{
    public class CustomWhere : IWhere
    {
        private string _expression;
        public CustomWhere(string expression)
        {
            _expression = expression;
        }
        public string GenerateExpress(SqlDataParameter sqldataparamters)
        {
            throw new NotImplementedException();
        }
    }
}
