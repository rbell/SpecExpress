using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress.Enums;

namespace SpecExpress.Test
{
    using SpecExpress.Test.Domain.Entities;
    using SpecExpress.Test.Specifications;

    using Customer = SpecExpress.Test.Entities.Customer;

    [TestFixture]
    public class ValidationLevelTests
    {
        [SetUp]
        public void Setup()
        {
            ValidationCatalog.Reset();
        }

        [Test]
        public void Specification_WithWarn_ReturnsValidationResultAsWarn()
        {   
            var spec = new CustomerSpecification();

            spec.Check(c => c.Name).Required();
            spec.Warn(c => c.Address).Required().Specification<AddressSpecification>();

            var customer = new Customer();

            var validationResults = spec.Validate(customer);

            var addressValidationResult = validationResults.Errors.Where(vr => vr.Property.Name == "Address").First();
            var nameValidationResult = validationResults.Errors.Where(vr => vr.Property.Name == "Name").First();

            Assert.That(addressValidationResult.Level == ValidationLevelType.Warn);
            Assert.That(nameValidationResult.Level == ValidationLevelType.Error);
        }

        [Test]
        public void ValidationNotification_WithAllWarn_IsValid()
        {
            ValidationCatalog.AddSpecification<Customer>( spec =>
                                                    {
                                                        spec.Warn(c => c.Name).Required();
                                                        spec.Warn(c => c.Address).Required().Specification<AddressSpecification>();
                                                    });
         

            var customer = new Customer();

            var vn = ValidationCatalog.Validate(customer);

            Assert.That( vn.IsValid, Is.True);
        }

        [Test]
        public void ValidationNotification_WithAllWarnDeepTree_IsValid()
        {
           var customer = new SpecExpress.Test.Domain.Entities.Customer();
            customer.PrimaryContact = new Contact();

            var vn = ValidationCatalog.Validate<CustomerRequiredWarningSpecification>(customer);
            Assert.IsTrue(vn.Errors.Count == 1);
            Assert.That(vn.IsValid, Is.True);
        }

        [Test]
        public void ValidationNotification_WithOneError_IsValid()
        {
            ValidationCatalog.AddSpecification<Customer>(spec =>
            {
                spec.Check(c => c.Name).Required();
                spec.Warn(c => c.Address).Required().Specification<AddressSpecification>();
            });


            var customer = new Customer();

            var vn = ValidationCatalog.Validate(customer);

            Assert.That(vn.IsValid, Is.False);
        }

    }
}
