using System.Linq;
using NUnit.Framework;
using SpecExpress.Test.Entities;
using SpecExpress.Test;
using System.Collections.Generic;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class SpecificationRuleTests
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
        public void When_validated_with_DefaultSpecification()
        {
            //Don't implicitly validate object graph
            ValidationCatalog.ValidateObjectGraph = false;

            var customer = new Customer {Name = "SampleCustomer", Address = new Address()};

            //Add Specification for Customer and Address
            ValidationCatalog.CatalogSpecificationContainer.Add(new AddressSpecification());
            ValidationCatalog.CatalogSpecificationContainer.Add(new CustomerAddressSpecification());

            //Validate Customer
            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors, Is.Not.Empty);

            Assert.That(results.Errors.First().NestedValidationResults, Is.Not.Empty);

        }

        [Test]
        public void When_validated_with_ExplicitSpecification()
        {
            //Don't implicitly validate object graph
            ValidationCatalog.ValidateObjectGraph = false;

            var customer = new Customer { Name = "SampleCustomer", Address = new Address() { Country = new Country() {Id = "DE", Name = "Germany"}, Street = "1234 Offenbacher Strasse"} };

            //Add Specification for Customer for international addresses
            ValidationCatalog.CatalogSpecificationContainer.Add(new InternationalAddressSpecification());
            ValidationCatalog.AddSpecification<Customer>(spec => spec.Check(c => c.Address).Required().Specification<InternationalAddressSpecification>());

            //Validate Customer
            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors, Is.Not.Empty);

            Assert.That(results.Errors.First().NestedValidationResults, Is.Not.Empty);

        }

        [Test]
        public void SpecificationExpression()
        {

            var customer = new Customer { 
                Name = "SampleCustomer", 
                Contacts = new List<Contact>() 
                { 
                    new Contact() {LastName = "Smith"}
                }, 
                Address = new Address() 
                { 
                    Country = new Country(){Id = "DE", Name = "Germany"}, Street = "1234 Offenbacher Strasse"} 
                };

            ValidationCatalog.CatalogSpecificationContainer.Add(new CustomerAddressSpecification());

            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors, Is.Not.Empty);


        }





    }
}