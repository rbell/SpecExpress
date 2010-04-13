using NUnit.Framework;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ValidationNotificationTests
    {
        [Test]
        public void GetNotificationForProperty_IsValid()
        {
            //Add a rule
            ValidationCatalog.AddSpecification<Contact>(spec =>
                                                            {
                                                                spec.Check(c => c.FirstName).Required();
                                                                spec.Check(c => c.LastName).Required();
                                                            });

            //dummy data 
            var contact = new Contact();

            //Validate
            ValidationNotification valNot = ValidationCatalog.Validate(contact);
            ValidationNotification valNotProperties = valNot.GetNotificationForProperty("FirstName");

            Assert.That(valNot.Errors.Count == 2);

            Assert.That(valNotProperties.Errors.Count == 1);
        }
    }
}