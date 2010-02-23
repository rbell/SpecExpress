using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.Specifications
{
    public class CustomerRequiredNameSpecification : Validates<Customer>
    {
        public CustomerRequiredNameSpecification()
        {
            Check(x => x.Name).Required();
        }
    }
}
