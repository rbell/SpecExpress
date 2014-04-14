using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    using SpecExpress.Test.Domain.Entities;

    public class CustomerExpectNullReferenceExceptionSpecification : Validates<Customer>
    {
        public CustomerExpectNullReferenceExceptionSpecification()
        {
            this.Check(c => c.Employees).Required().Expect(ThrowHereNullReferenceException,"object not valid");
        }

        private bool ThrowHereNullReferenceException(Customer arg1, List<Contact> arg2)
        {
            throw new NullReferenceException("try_to_find_the_specification_causing_this_nullreferenceexception");
        }
    }
}
