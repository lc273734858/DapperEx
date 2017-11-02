using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// Type of a database.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// SQL Server 2000
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// MsAccess
        /// </summary>
        MsAccess = 1,
        /// <summary>
        /// SQL Server 2005
        /// </summary>
        SqlServer9 = 2,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 3,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite3 = 4,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 5
    }

}
