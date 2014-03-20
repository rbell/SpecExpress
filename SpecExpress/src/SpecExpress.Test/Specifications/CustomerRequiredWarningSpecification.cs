using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Specifications
{
    using SpecExpress.Test.Domain.Entities;

    public class CustomerRequiredWarningSpecification : Validates<Customer>
    {
        public CustomerRequiredWarningSpecification()
        {
            this.Check(c => c.PrimaryContact).Required().Specification<ContactFirstNameWarningRequiredSpecification>();
        }
    }
}
