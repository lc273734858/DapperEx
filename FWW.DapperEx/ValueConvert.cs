using FWW.Framework.Common.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    public static class ValueConvert
    {
        static public int ConvertToInt(string errormessage, string V)
        {
            int count;
            if (!int.TryParse(V, out count))
            {
                throw new FriendlyException(errormessage);
            }
            return count;
        }
        static public decimal ConvertToDecimal(string errormessage, string V)
        {
            decimal count;
            if (!decimal.TryParse(V, out count))
            {
                throw new FriendlyException(errormessage);
            }
            return count;
        }
        static public DateTime ConvertToDate(string errormessage, string V)
        {
            DateTime date;
            if (!DateTime.TryParse(V, out date))
            {
                throw new FriendlyException(errormessage);
            }
            return date;
        }
        static public string ConvertToLike(string V)
        {
            return string.Format("%{0}%", V);
        }
    }
}
