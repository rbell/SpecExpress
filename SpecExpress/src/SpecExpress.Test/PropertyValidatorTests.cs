using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress.Rules.StringValidators;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class PropertyValidatorTests
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
        public void Validate_OptionalProperty_WithNoValue_IsValid()
        {
            var emptyContact = new Contact();
            emptyContact.FirstName = string.Empty;
            emptyContact.LastName = string.Empty;

            var propertyValidator =
                new PropertyValidator<Contact, string>(contact => contact.LastName);

            propertyValidator.PropertyValueRequired = false; //Optional

            //add a single rule
            var lengthValidator = new LengthBetween<Contact>(1, 5);
            propertyValidator.AddRule(lengthValidator); //.Rules.Add(lengthValidator);

            //Validate
            List<ValidationResult> result = propertyValidator.Validate(emptyContact, null);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Validate_OptionalNestedProperty_WithNullValue_IsValid()
        {
            var customer = new Customer();

            ValidationCatalog.AddSpecification<Customer>( spec => spec.Check(cust => cust.Address.Street).Optional()
                .MaxLength(255));

            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors, Is.Empty);

        }


        [Test]
        public void Validate_OptionalCollection_Using_Registered_Specification()
        {
            //Build test data
            var customer = new Customer() { Name = "TestCustomer"};
            var validContact = new Contact() {FirstName = "Johnny B", LastName = "Good"};
            var invalidContact = new Contact() { FirstName = "Baddy"};

            customer.Contacts = new List<Contact>() {validContact, invalidContact};

            //Build specifications
            ValidationCatalog.AddSpecification<Customer>(spec =>
                                                               {
                                                                   spec.Check(cust => cust.Name).Required();
                                                                   spec.Check(cust => cust.Contacts).Required();
                                                               });

            ValidationCatalog.AddSpecification<Contact>(spec =>
                                                              {
                                                                  spec.Check(c => c.FirstName).Required();
                                                                  spec.Check(c => c.LastName).Required();
                                                              });


            ValidationCatalog.ValidateObjectGraph = true;

            //Validate
            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors.Count, Is.AtLeast(1));

        }

        [Test]
        public void Validate_Property_With_PropertyNameOverrideExpression_IsValid()
        {
            var emptyContact = new Contact();
            emptyContact.FirstName = "George's last name";
            emptyContact.LastName = string.Empty;

            var propertyValidator =
                new PropertyValidator<Contact, string>(contact => contact.LastName);

            propertyValidator.PropertyValueRequired = true;
            propertyValidator.PropertyNameOverrideExpression = new Func<Contact, string>( o => o.FirstName);

            //add a single rule
            var lengthValidator = new LengthBetween<Contact>(1, 5);
            propertyValidator.AddRule(lengthValidator); //.Rules.Add(lengthValidator);

            //Validate
            List<ValidationResult> result = propertyValidator.Validate(emptyContact, null);

            Assert.That(result, Is.Not.Empty);
        }



    }
}