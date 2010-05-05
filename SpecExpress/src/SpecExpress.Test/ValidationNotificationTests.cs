using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Test.Domain.Entities;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ValidationNotificationTests
    {

        [Test]
        public void FindDescendents_IsValid()
        {
            var addressToFind = new Address()
            {
                City = "Gatlinburg"
            };

            var primaryAddress = new Address();

            var contact = new SpecExpress.Test.Domain.Entities.Contact()
            {
                FirstName = "Charles",
                LastName = "radar",
                Addresses = new List<Address>() { addressToFind },
                PrimaryAddress = primaryAddress
            };

            var allNotfication =
                ValidationCatalog.Validate<SpecExpress.Test.Domain.Specifications.ContactSpecification>(contact);

            var filteredNotfication = ValidationCatalog.Validate<SpecExpress.Test.Domain.Specifications.ContactSpecification >(contact)
                .FindDescendents(v => v.Target == addressToFind)
                .SelectMany(vr => vr.NestedValidationResults)
                .ToNotification();
            
            Assert.That(filteredNotfication.IsValid, Is.False);
        }


    }
}