using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
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

            var addressValidationResult = validationResults.Where(vr => vr.Property.Name == "Address").First();
            var nameValidationResult = validationResults.Where(vr => vr.Property.Name == "Name").First();

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
