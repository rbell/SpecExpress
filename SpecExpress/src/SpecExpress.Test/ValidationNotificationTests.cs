using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress.Test.Domain.Entities;
using SpecExpress.Test.Domain.Values;

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


        [Test]
        public void GetAllErrors()
        {
            //Add a rule
            ValidationCatalog.AddSpecification<Contact>(spec =>
            {
                spec.Check(c => c.FirstName).Required();
                spec.Check(c => c.LastName).Required();
                spec.Check(c => c.Addresses).Required().ForEachSpecification<Address, SpecExpress.Test.Domain.Specifications.AddressSpecification>();
                spec.Check(c => c.PrimaryAddress).Required().Specification<SpecExpress.Test.Domain.Specifications.AddressSpecification>();
                spec.Check(c => c.DefaultProject).Required();
                spec.Check(c => c, "Credit Score").Required().Expect((c, d) => 1 == 1, "Error");
            });

            //dummy data 
            var contact = new Contact()
                              {
                                  Addresses = new List<Address>()
                                                  {
                                                      new Address()
                                                          {
                                                              City = "Gatlinburg",
                                                              Country = "US"
                                                          }
                                                  },
                                                  PrimaryAddress = new Address(),
                                                  DefaultProject = new Project()
                                                 
                              };

            //Validate
            var notification = ValidationCatalog.Validate(contact);
            ValidationNotification valNotProperties = notification.GetNotificationForProperty("FirstName");

            var allErrors = notification.AllErrors();
            
            Assert.That(allErrors.ToList().Count(), Is.EqualTo(10));

            var errors = allErrors.Where(vr => vr.Property.Name == "FirstName");
            var streetErrors = allErrors.Where(vr => vr.Target == contact.Addresses.First()).First()
                .AllValidationResults();
                //.Where( vr => vr.Property.Name == "Street");

            Assert.That(streetErrors.Count(), Is.EqualTo(2));

        }

    }
}