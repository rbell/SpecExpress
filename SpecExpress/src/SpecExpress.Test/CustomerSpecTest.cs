using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Rules.DateValidators;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
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

            var notification = spec.Validate(customer);
            Assert.IsEmpty(notification.Errors);
        }

        [Test]
        public void CustomerName_OptionalAndLength_IsValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Optional().LengthBetween(0, 100);

            var notification = spec.Validate(customer);
            Assert.IsEmpty(notification.Errors);
        }

        [Test]
        public void CustomerName_OptionalAndLength_IsNotValid()
        {
            var customer = new Customer() { Name = "A"};

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Optional().LengthBetween(2, 100);

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
            Assert.AreEqual(1, notification.Errors.Count);
        }

        [Test]
        public void CustomerName_Required_IsNotValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required();

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
        }

        [Test]
        public void CustomerName_RequiredAndLength_NullValue_IsNotValid()
        {
            var customer = new Customer();

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().LengthBetween(2, 100);

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
            Assert.AreEqual(1, notification.Errors.Count);
        }

        [Test]
        public void CustomerName_RequiredAndLength_InvalidLength_IsNotValid()
        {
            var customer = new Customer { Name = "X" };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().LengthBetween(2, 100);

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
            Assert.AreEqual(1, notification.Errors.Count);
        }

        [Test]
        public void CustomerName_RequiredAndNotMinLength_InvalidLength_IsNotValid()
        {
            var customer = new Customer { Name = string.Empty.PadLeft(105, 'X') };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Name).Required().Not.MinLength(100);

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
            Assert.AreEqual(1, notification.Errors.Count);
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

            var notification = spec.Validate(customer);
            Assert.IsNotEmpty(notification.Errors);
            Assert.AreEqual(1, notification.Errors.Count);
        }

        [Test]
        public void CustomerAddressCountry_Required_IsValid()
        {
            var customer = new Customer() { Address = new Address() { Country = new Country() } };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Address.Country.Name).Required();

            var notification = spec.Validate(customer);
            Assert.That(notification.Errors, Is.Not.Empty);
            Assert.That(notification.Errors.Count, Is.EqualTo(1));
            Assert.That(notification.Errors[0].Message, Is.EqualTo("Address Country Name is required."));
        }

        [Test]
        public void CustomerPromotionDate_IsInFuture_IsValid()
        {
            var customer = new Customer() {PromotionDate = DateTime.Now.AddDays(1)};

            var spec = new CustomerSpecification();
            spec.Check(c => c.PromotionDate).Optional().IsInFuture();

            var notification = spec.Validate(customer);
            Assert.That(notification.Errors, Is.Empty);

        }

        [Test]
        [Ignore]
        public void PastCustomerPromotionDate_IsInFuture_IsNotValid()
        {
            var customer = new Customer() { PromotionDate = DateTime.Now.AddDays(-1) };

            var spec = new CustomerSpecification();
            spec.Check(c => c.PromotionDate).Optional().IsInFuture();

            var notification = spec.Validate(customer);
            Assert.That(notification.Errors, Is.Not.Empty);

        }

        [Test]
        [TestCase(-20, true, Description = "Date before floor should be valid.")]
        [TestCase(10, true, Description = "Date after ceiling should be valid.")]
        [TestCase(-5, false, Description = "Date between floor and ceiling should fail.")]
        public void ActiveDate_GreaterThan_Or_LessThan(int activeDateDaysFromToday, bool isValid)
        {
            var customer = new Customer() { ActiveDate = DateTime.Now.AddDays(activeDateDaysFromToday) };

            var spec = new CustomerSpecification();
            spec.Check(c => c.ActiveDate).Required().LessThan(DateTime.Now.AddDays(-10)).Or.GreaterThan(DateTime.Now);

            var notification = spec.Validate(customer);
            if (isValid)
            {
                Assert.That(notification.Errors, Is.Empty);
            }
            else
            {
                Assert.That(notification.Errors, Is.Not.Empty);
            }
        }

        [Test]
        [TestCase(-15, true, Description = "Date in ten day windo should be valid.")]
        [TestCase(10, true, Description = "Date after now should be valid.")]
        [TestCase(-5, false, Description = "Date between window and now should fail.")]
        [TestCase(-25, false, Description = "Date before window should fail.")]
        public void ActiveDate_And_Or_RulePrecidence(int activeDateDaysFromToday, bool isValid)
        {
            var customer = new Customer() { ActiveDate = DateTime.Now.AddDays(activeDateDaysFromToday) };

            var spec = new CustomerSpecification();

            // Valid dates are a window of 10 days starting 20 days ago, or greater than now.
            spec.Check(c => c.ActiveDate).Required().GreaterThan(DateTime.Now.AddDays(-20)).And.LessThan(DateTime.Now.AddDays(-10)).Or.GreaterThan(DateTime.Now);

            var notification = spec.Validate(customer);
            if (isValid)
            {
                Assert.That(notification.Errors, Is.Empty);
            }
            else
            {
                Assert.That(notification.Errors, Is.Not.Empty);
            }
        }

        [Test]
        [TestCase(-15, true, Description = "Date in ten day windo should be valid.")]
        [TestCase(10, true, Description = "Date after now should be valid.")]
        [TestCase(-5, false, Description = "Date between window and now should fail.")]
        [TestCase(-25, false, Description = "Date before window should fail.")]
        public void ActiveDate_Or_And_RulePrecidence(int activeDateDaysFromToday, bool isValid)
        {
            var customer = new Customer() { ActiveDate = DateTime.Now.AddDays(activeDateDaysFromToday) };

            var spec = new CustomerSpecification();

            // Valid dates are greater than now or a window of 10 days starting 20 days ago.
            spec.Check(c => c.ActiveDate).Required().GreaterThan(DateTime.Now).Or.GreaterThan(DateTime.Now.AddDays(-20)).And.LessThan(DateTime.Now.AddDays(-10));

            var notification = spec.Validate(customer);
            if (isValid)
            {
                Assert.That(notification.Errors, Is.Empty);
            }
            else
            {
                Assert.That(notification.Errors, Is.Not.Empty);
            }
        }

        [Test]
        [TestCase(-15, false, Description = "Date 15 days ago should not be valid.")]
        [TestCase(10, false, Description = "Date 10 days from now should not be valid.")]
        [TestCase(0, false, Description = "A current date should not be valid.")]
        [TestCase(-7, true, Description = "Date 7 days ago should be valid.")]
        [TestCase(7, true, Description = "Date 7 days from now should be valid.")]
        public void ActiveDate_TwoWindowsUsingGroups(int activeDateDaysFromToday, bool isValid)
        {
            var customer = new Customer() { ActiveDate = DateTime.Now.AddDays(activeDateDaysFromToday) };

            var spec = new CustomerSpecification();

            // Valid dates are in a five day window starting 10 days ago OR a five day window starting in 5 days from now.
            spec.Check(c => c.ActiveDate).Required()
                .Group(d => d.GreaterThan(DateTime.Now.AddDays(-10))
                                .And.LessThan(DateTime.Now.AddDays(-5)))
                .Or
                .Group(d => d.GreaterThan(DateTime.Now.AddDays(5))
                                .And.LessThan(DateTime.Now.AddDays(10)));

            var notification = spec.Validate(customer);
            if (isValid)
            {
                Assert.That(notification.Errors, Is.Empty);
            }
            else
            {
                Assert.That(notification.Errors, Is.Not.Empty);
            }
        }

        [Test]
        public void When_Customer_Contacts_IsInitializeButEmpty_And_DefinedRequired_IsInvalid()
        {
            var customer = new Customer { Contacts = new List<Contact>() };

            var spec = new CustomerSpecification();
            spec.Check(cust => cust.Contacts).Required();

            var notifications = spec.Validate(customer);

            Assert.IsNotEmpty(notifications.Errors);
        }


    }
}