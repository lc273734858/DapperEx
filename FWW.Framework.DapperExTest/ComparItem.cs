using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FWW.Framework.DapperExTest
{
    public class ComparItem
    {
        public ComparItem(ITest item,string name)
        {
            TestFactory = item;
            Name = name;
        }
        public ITest TestFactory;
        public string Name { get; set; }
    }
}
