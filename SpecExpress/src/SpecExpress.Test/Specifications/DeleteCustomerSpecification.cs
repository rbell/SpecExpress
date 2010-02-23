using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.Specifications
{
    public class DeleteCustomerSpecification : Validates<Customer>
    {
        public DeleteCustomerSpecification()
        {
            Check(c => c.Active).Optional().IsFalse();
            Check(c => c.Employees).Optional().ForEachSpecification<Contact>(); //Default specification
        }

    }
}
