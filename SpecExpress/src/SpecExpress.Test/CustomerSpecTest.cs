using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Rules.DateValidators;
using SpecExpress.Test.Entities;
using SpecExpressTest.Entities;

namespace SpecExpressTest
{
    [TestFixture]
    public class CustomerSpecTest
    {

        [Test]
        public void CustomerName_Optional_IsValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Optional();

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsEmpty(notification);
        }

        [Test]
        public void CustomerName_OptionalAndLength_IsValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Optional().LengthBetween(0, 100);

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsEmpty(notification);
        }

        [Test]
        public void CustomerName_OptionalAndLength_IsNotValid()
        {
            var customer = new Customer() { Name = "A"};

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Optional().LengthBetween(2, 100);

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
            Assert.AreEqual(1, notification.Count);
        }

        [Test]
        public void CustomerName_Required_IsNotValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required();

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
        }

        [Test]
        public void CustomerName_RequiredAndLength_NullValue_IsNotValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().LengthBetween(2, 100);

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
            Assert.AreEqual(1, notification.Count);
        }

        [Test]
        public void CustomerName_RequiredAndLength_InvalidLength_IsNotValid()
        {
            var customer = new Customer { Name = "X" };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().LengthBetween(2, 100);

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
            Assert.AreEqual(1, notification.Count);
        }

        [Test]
        public void CustomerName_RequiredAndNotMinLength_InvalidLength_IsNotValid()
        {
            var customer = new Customer { Name = string.Empty.PadLeft(105, 'X') };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().Not.MinLength(100);

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
            Assert.AreEqual(1, notification.Count);
        }

        [Test]
        public void CustomerContacts_Lambda_IsNotValid()
        {
            var contact1 = new Contact() { DateOfBirth = DateTime.Now.AddYears(-19) };
            var contact2 = new Contact() { DateOfBirth = DateTime.Now.AddYears(-22) };
            var customer = new Customer() { Contacts = new List<Contact> { contact1, contact2 } };

            var spec = new CustomerSpecification();


            spec.Check(
                c => from contact in c.Contacts where contact.DateOfBirth < DateTime.Now.AddYears(-20) select contact)
                .Optional()
                .ForEach(c => ((Contact)c).Active, "All contacts under age of 20 must be active.");

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification);
            Assert.AreEqual(1, notification.Count);
        }

        [Test]
        public void CustomerAddressCountry_Required_IsValid()
        {
            var customer = new Customer() { Address = new Address() { Country = new Country() } };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Address.Country.Name).Required();

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.That(notification, Is.Not.Empty);
            Assert.That(notification.Count, Is.EqualTo(1));
            Assert.That(notification[0].Message, Is.EqualTo("Address Country Name is required."));
        }

        [Test]
        public void CustomerPromotionDate_IsInFuture_IsValid()
        {
            var customer = new Customer() {PromotionDate = DateTime.Now.AddDays(1)};

            var spec = new CustomerSpecification();
            spec.Check(c => c.PromotionDate).Optional().IsInFuture();

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.That(notification, Is.Empty);

        }

        [Test]
        [Ignore]
        public void PastCustomerPromotionDate_IsInFuture_IsNotValid()
        {
            var customer = new Customer() { PromotionDate = DateTime.Now.AddDays(-1) };

            var spec = new CustomerSpecification();
            spec.Check(c => c.PromotionDate).Optional().IsInFuture();

            List<ValidationResult> notification = spec.Validate(customer);
            Assert.That(notification, Is.Not.Empty);

        }

        //[Test]
        //public void Test1()
        //{
        //    var customer = new Customer {Name = "X"};

        //    var spec = new CustomerSpecification();
        //    spec.Check(cust => cust.Name).Required().And.LengthBetween(2, 100);

        //    List<ValidationResult> notifications = spec.Validate(customer);

        //    Assert.IsNotNull(notifications);
        //    Assert.AreEqual(1, notifications.Count);
        //    Assert.AreEqual("'Name' must be between 2 and 100 characters. You entered 1 characters.",
        //                    notifications[0].Message);
        //}

        //[Test]
        //public void Test2()
        //{
        //    var customer = new Customer {CustomerDate = new DateTime(2009, 3, 1)};

        //    var spec = new CustomerSpecification();
        //    spec.Check(cust => cust.CustomerDate).Required()
        //        .And
        //        .LessThan(new DateTime(2009, 1, 1))
        //        .Or
        //        .GreaterThan(new DateTime(2010, 1, 1));

        //    List<ValidationResult> notifications = spec.Validate(customer);

        //    Assert.IsNotNull(notifications);
        //    Assert.AreEqual(2, notifications.Count);
        //    Assert.AreEqual(
        //        "'Customer Date' must be less than 1/1/2009 12:00:00 AM. You entered 3/1/2009 12:00:00 AM.",
        //        notifications[0].Message);
        //}

        //[Test]
        //public void Test3()
        //{
        //    var customer = new Customer {CustomerDate = new DateTime(2010, 3, 1)};

        //    var spec = new CustomerSpecification();
        //    spec.Check(cust => cust.CustomerDate).Required()
        //        .And
        //        .LessThan(new DateTime(2009, 1, 1))
        //        .Or
        //        .GreaterThan(new DateTime(2010, 1, 1));

        //    List<ValidationResult> notifications = spec.Validate(customer);

        //    Assert.IsEmpty(notifications);
        //}

        [Test]
        public void When_Customer_Contacts_IsInitializeButEmpty_And_DefinedRequired_IsInvalid()
        {
            var customer = new Customer { Contacts = new List<Contact>() };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Contacts).Required();

            List<ValidationResult> notifications = spec.Validate(customer);

            Assert.IsNotEmpty(notifications);
        }


    }
}