using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test
{
    using NUnit.Framework;

    using SpecExpress.Test.Domain.Entities;
    using SpecExpress.Test.Domain.Specifications;

    [TestFixture]
    public class ExceptionsTest
    {
        /// <summary>
        /// An null item in the list causes nullreferenceexception. Very hard to figure out what is going on
        /// without a clear exception message
        /// </summary>
        [Test]
        public void ListNullReferenceException()
        {
            try
            {
                var customer = new Customer();
                customer.Employees = new List<Contact>();
                customer.Employees.Add(null);

                ValidationCatalog.Validate<CustomerNullReferenceSpecification>(customer);
            }
            catch (NullReferenceException exception)
            {
                Assert.IsTrue(exception.Message.Contains(typeof(Customer).FullName));
            }
        }

        /// <summary>
        /// Very hard to figure out which specification caused the exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(CustomRuleEvaluationException))]
        public void ExpectNullReferenceException()
        {
            var customer = new Customer();
            customer.Employees = new List<Contact>();
            customer.Employees.Add(null);

            ValidationCatalog.Validate<CustomerExpectNullReferenceExceptionSpecification>(customer);
        }
    }
}
