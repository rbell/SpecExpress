using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    public class ExtendedCustomerSpecification : Validates<ExtendedCustomer>
    {
        public ExtendedCustomerSpecification()
        {
            Check(c => (Customer)c, "Customer").Required().Specification<CustomerSpecification>();
            Check(c => c.SpecialGreeting).Required();
        }
    }
}
