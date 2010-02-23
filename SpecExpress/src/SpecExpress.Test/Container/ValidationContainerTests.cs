using System;
using System.Reflection;
using NUnit.Framework;
using SpecExpress.MessageStore;
using SpecExpress.Test.Domain.Specifications;
using SpecExpress.Test.Entities;
using System.Collections.Generic;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ValidationContainerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            //Set Assemblies to scan for Specifications
            //ValidationContainer.Scan(x => x.TheCallingAssembly());
            ValidationCatalog.Reset();
        }

        [TearDown]
        public void Teardown()
        {
            //Reset Catalog
            ValidationCatalog.Reset();
        }

        #endregion

        [Test]
        public void ValidationContainer_Initialize()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Contact>(x =>
                                                              {
                                                                  x.Check(contact => contact.LastName).Required();
                                                                  x.Check(contact => contact.FirstName).Required();
                                                                  x.Check(contact => contact.DateOfBirth).Optional()
                                                                      .GreaterThan(
                                                                      new DateTime(1950, 1, 1));
                                                              });

            //Dummy Contact
            var emptyContact = new Contact();
            emptyContact.FirstName = null;
            emptyContact.LastName = null;

            //Validate
            ValidationNotification notification = ValidationCatalog.Validate(emptyContact);

            Assert.That(notification.Errors, Is.Not.Empty);
        }

        [Test]
        [Ignore]
        public void AssertConfigurationValid_IsInvalid()
        {
            //Set Assemblies to scan for Specifications
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));
            Assert.That(ValidationCatalog.SpecificationContainer.GetAllSpecifications(), Is.Not.Empty);

            Assert.Throws<SpecExpressConfigurationException>(
                () =>
                {
                    ValidationCatalog.AssertConfigurationIsValid();
                });
        }

        [Test]
        public void When_multiple_specifications_defined_with_default_spec_defined_return_default()
        {
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));

            var spec = ValidationCatalog.SpecificationContainer.GetSpecification<SpecExpress.Test.Domain.Entities.Contact>();
            Assert.That(spec.GetType(), Is.EqualTo(typeof(SpecExpress.Test.Domain.Specifications.ContactSpecification)));
        }


        [Test]
        public void When_configuring_Catalog_and_using_default_configuration()
        {
            Assert.That(ValidationCatalog.Configuration.DefaultMessageStore.GetType(), Is.EqualTo(typeof(ResourceMessageStore)));
            Assert.That(ValidationCatalog.Configuration.ValidateObjectGraph, Is.False);
        }

        [Test]
        public void Validate_Collection_Using_Specified_Specification_WithoutValidateObjectGraph()
        {
            //Build test data
            var validContact = new Contact() { FirstName = "Johnny B", LastName = "Good" };
            var invalidContact = new Contact() { FirstName = "Baddy" };

            var contacts = new List<Contact>() { validContact, invalidContact };

            //Create specification
            ValidationCatalog.AddSpecification<Contact>(spec =>
            {
                spec.Check(c => c.FirstName).Required();
                spec.Check(c => c.LastName).Required();
            });

            //Validate
            var results = ValidationCatalog.Validate(contacts);

            Assert.That(results.Errors.Count, Is.AtLeast(1));

        }

        [Test]
        public void ValidateProperty_SimpleProperty_ReturnsValidationNotification()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Contact>(x =>
            {
                x.Check(c => c.LastName).Required();
                x.Check(c => c.FirstName).Required();
            });

            var contact = new Contact();

            // Validating contact as a whole should result in two errors.
            var objectNotification = ValidationCatalog.Validate(contact);
            Assert.IsFalse(objectNotification.IsValid);
            Assert.AreNotEqual(2, objectNotification.Errors);

            // Validation contact.LastName should result with only one error.
            var propertyNotification = ValidationCatalog.ValidateProperty(contact, c => c.LastName);
            Assert.IsFalse(propertyNotification.IsValid);
            Assert.AreNotEqual(1, propertyNotification.Errors);
        }

        [Test]
        public void ValidatePropertyWithPropertyString_SimpleProperty_ReturnsValidationNotification()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Contact>(x =>
            {
                x.Check(c => c.LastName).Required();
                x.Check(c => c.FirstName).Required();
            });

            var contact = new Contact();

            // Validating contact as a whole should result in two errors.
            var objectNotification = ValidationCatalog.Validate(contact);
            Assert.IsFalse(objectNotification.IsValid);
            Assert.AreNotEqual(2, objectNotification.Errors);

            // Validation contact.LastName should result with only one error.
            var propertyNotification = ValidationCatalog.ValidateProperty(contact,"LastName");
            Assert.IsFalse(propertyNotification.IsValid);
            Assert.AreNotEqual(1, propertyNotification.Errors);
        }

        [Test]
        public void ValidateProperty_SimpleProperty_OverridePropertyNameReturnsValidationNotification()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Contact>(x =>
            {
                x.Check(c => c.LastName, "Contact Last Name").Required();
                x.Check(c => c.FirstName).Required();
            });

            var contact = new Contact();

            // Validating contact as a whole should result in two errors.
            var objectNotification = ValidationCatalog.Validate(contact);
            Assert.IsFalse(objectNotification.IsValid);
            Assert.AreNotEqual(2, objectNotification.Errors);

            // Validation contact.LastName should result with only one error.
            var propertyNotification = ValidationCatalog.ValidateProperty(contact, c => c.LastName);
            Assert.IsFalse(propertyNotification.IsValid);
            Assert.AreNotEqual(1, propertyNotification.Errors);
        }

        [Test]
        public void ValidateProperty_NoValidationForProperty_ThrowsArgumentException()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Contact>(x =>
            {
                x.Check(c => c.FirstName).Required();
            });

            var contact = new Contact();

            // Validation contact.LastName should result with only one error.


            Assert.Throws<ArgumentException>(
               () =>
               {
                   var propertyNotification = ValidationCatalog.ValidateProperty(contact, c => c.LastName);
               });

        }

        [Test]
        public void ValidateProperty_CollectionProperty_ReturnsValidationNotification()
        {
            //Create Rules Adhoc
            ValidationCatalog.AddSpecification<Address>(x =>
                                                            {
                                                                x.Check(a => a.Street)
                                                                    .Required()
                                                                    .MaxLength(50);
                                                            });

            ValidationCatalog.AddSpecification<Contact>(x =>
                                                            {
                                                                x.Check(c => c.Addresses).Required()
                                                                    .ForEachSpecification<Address>();
                                                                x.Check(c => c.FirstName).Required().MaxLength(100);
                                                            });

            var contact = new Contact();
            contact.Addresses = new List<Address>() {new Address()};

            // Validating contact as a whole should result in two errors.
            var objectNotification = ValidationCatalog.Validate(contact);
            Assert.IsFalse(objectNotification.IsValid);
            Assert.AreNotEqual(2, objectNotification.Errors);

            // Validation contact.LastName should result with only one error.
            var propertyNotification = ValidationCatalog.ValidateProperty(contact, c => c.Addresses);
            Assert.IsFalse(propertyNotification.IsValid);
            Assert.AreNotEqual(1, propertyNotification.Errors);
        }

    }
}