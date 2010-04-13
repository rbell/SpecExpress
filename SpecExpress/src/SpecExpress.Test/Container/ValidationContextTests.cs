using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ValidationContextTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            ValidationCatalog.Reset();
        }

        [TearDown]
        public void Teardown()
        {
            //Reset Catalog
            ValidationCatalog.Reset();
        }

        #endregion

        //Validate with Delete Context

        [Test]
        public void Validate_ContextUsesDefinedSpecifications()
        {
            //Scan all 
            ValidationCatalog.Scan(x => x.TheCallingAssembly());

            //Setup entity
            var customer = new Customer()
                               {
                                   Active = true,
                                   Employees = new List<Contact>() {new Contact() {Active = true}}
                               };

            //Validate
            var results = ValidationCatalog<DeleteValidationContext>.Validate(customer);
            
            Assert.That(results.Errors.First().Message, Is.EqualTo("Active must be false."));
            Assert.That(results.Errors[1].NestedValidationResults.First().Message, Is.EqualTo("Contact 1 in Employees is invalid."));
            Assert.That(results.Errors[1].NestedValidationResults.First().NestedValidationResults.First().Message, Is.EqualTo("Active must be false."));
        }

    }
}