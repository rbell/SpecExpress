using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test
{
    using NUnit.Framework;

    using SpecExpress.Test.Domain.Entities;
    using SpecExpress.Test.Domain.Specifications;

    /// <summary>
    /// These are test cases to illustrate a the NullReference Problem
    /// </summary>
    [TestFixture]
    
    public class ExceptionsTest
    {
        /// <summary>
        /// An null item in the list causes nullreferenceexception. Very hard to figure out what is going on
        /// without a clear exception message
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ListNullReferenceException()
        {   
            var customer = new Customer {Employees = new List<Contact> {null}};
            Specification.Assert(spec => spec.Check(c => customer.Employees).Optional().ForEachSpecification());
        }

        /// <summary>
        /// Very hard to figure out which specification caused the exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExpectNullReferenceException()
        {
            var customer = new Customer {Employees = new List<Contact> {null}};
            Specification.Assert(spec => spec.Check(c => customer.Employees).Required().Expect(ThrowHereNullReferenceException, "object not valid"));
        }

        private bool ThrowHereNullReferenceException(object arg1, List<Contact> arg2)
        {
            throw new NullReferenceException("try_to_find_the_specification_causing_this_nullreferenceexception");
        }
    }
}
