using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress;
using SpecExpressTest.Entities;


namespace SpecExpressTest
{
    [TestFixture]
    public class ExpressionValidationTests
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
        public void InvalidExpression_IsInvalid()
        {
            ValidationCatalog.AddSpecification<Contact>(x => x.Check(c => c.NumberOfDependents).Required().   
                                                                 GreaterThan( z=> new BadWolf().Max(z.NumberOfDependents)));

            var contact = new Contact() {LastName = "Bill"};

            var results = ValidationCatalog.Validate(contact);

            Assert.That(results.Errors, Is.Not.True);

           

        }
    }
}
