using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Test.Entities;
using SpecExpressTest.Entities;
using SpecExpressTest.Specifications;

namespace SpecExpressTest
{
    [TestFixture]
    public class InheritanceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            ValidationCatalog.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            ValidationCatalog.Reset();
        }

        #endregion

        [Test]
        public void SpecificationInheritance_OnObject_WithSpecification_IsValid()
        {
            //Base Class
            ValidationCatalog.SpecificationContainer.Add(new CustomerRequiredNameSpecification());
            //Inherited class
            ValidationCatalog.AddSpecification<ExtendedCustomer>(spec =>
                                                                      { 
                                                                          spec.Using<Customer, CustomerRequiredNameSpecification>();
                                                                          spec.Check(c => c.SpecialGreeting).Required();
                                                                      });

            var customer = new ExtendedCustomer();
            var results = ValidationCatalog.Validate(customer);

            
            Assert.That(results.Errors.Count, Is.EqualTo(2));
            var errorMessages = results.Errors.Select(x => x.ToString());
            Assert.That(errorMessages.Contains("Name is required."));
            Assert.That(errorMessages.Contains("Special Greeting is required."));
        }

    }
}
