using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Test.Entities;
using Address=SpecExpress.Test.Domain.Values.Address;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ContactSpecificationTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public void Advanced()
        {
            var contact1 = new Contact() { FirstName = "Something", LastName = "Else"};

            var results = ValidationCatalog.Validate<ContactSpecification>(contact1);

            Assert.That(results.IsValid, Is.False);
        }
    }
}   