using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress.Enums;
//using SpecExpress.Test.Domain.Entities;
//using SpecExpress.Test.Domain.Values;
using SpecExpress.Util;

using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ValidationNotificationTests
    {
        [SetUp]
        public void Setup()
        {
            ValidationCatalog.Reset();
        }

        //[Test]
        //public void FindDescendents_IsValid()
        //{
        //    var addressToFind = new Address()
        //    {
        //        City = "Gatlinburg"
        //    };

        //    var primaryAddress = new Address();

        //    var contact = new Contact()
        //    {
        //        FirstName = "Charles",
        //        LastName = "radar",
        //        Addresses = new List<Address>() { addressToFind }
        //    };

        //    var allNotfication =
        //        ValidationCatalog.Validate<ContactSpecification>(contact);

        //    var summary = allNotfication.ErrorSummary();



        //    //var filteredNotfication = allNotfication.FindDescendents(v => v.Target == contact.Addresses);
        //    var filteredNotfication = allNotfication.FindDescendents(v => v.Target == addressToFind);

            

        //    Assert.That(filteredNotfication, Is.Not.Empty);
        //}

        [Test]
        public void FindDescendents_WithNestedChildren_IsValid()
        {
            ValidationCatalog.AddSpecification<Customer>(spec => spec.Check(c => c.Contacts).Required().ForEachSpecification<Contact>(
                cspec => cspec.Check(c => c.Addresses).Required().ForEachSpecification<Address>(aspec =>
                    aspec.Check(c => c.Street).Required())));
            
            var customer = new Customer();
            customer.Contacts = new List<Contact>();

            var contact1 = new Contact();
            contact1.Addresses = new List<Address>();
            var badAddresss1 = new Address() { City = "Somewhere1" };
            var badAddresss2 = new Address() { City = "Somewhere2" };
            contact1.Addresses.Add(badAddresss1);
            contact1.Addresses.Add(badAddresss2);



            var contact2 = new Contact();
            contact2.Addresses = new List<Address>();
            contact2.Addresses.Add(new Address() { Street = "Somewhere1" });
            contact2.Addresses.Add(new Address() { Street = "Somewhere2" });

            customer.Contacts.Add(contact1);
            customer.Contacts.Add(contact2);

            var notification = ValidationCatalog.Validate(customer);

            Assert.That(notification.IsValid, Is.False);

            var subNotification = notification.FindDescendents(v => v.Target == badAddresss1);

            Assert.That(subNotification.ToList().Any(), Is.True);

        }


    }


    public static class ValidationNotificationExtensions
    {
        public static List<string> ErrorSummary(this ValidationNotification notification)
        {
            return GetAllResultMessages(notification.Errors, string.Empty);
        }

        private static List<string> GetAllResultMessages(IEnumerable<ValidationResult> results, string prefix)
        {
            var messages = new List<string>();
            foreach (ValidationResult result in results)
            {
                if (result.NestedValidationResults.Any())
                {
                    messages.AddRange(GetAllResultMessages(result.NestedValidationResults, string.Empty));
                }
                else
                {
                    messages.Add(result.Message);
                }
            }

            return messages;
        }
    }
}