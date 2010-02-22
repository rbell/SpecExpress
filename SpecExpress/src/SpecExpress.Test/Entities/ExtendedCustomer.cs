using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Test.Entities;

namespace SpecExpressTest.Entities
{
    public class ExtendedCustomer : Customer 
    {
        public string SpecialGreeting { get; set; }
    }
}
