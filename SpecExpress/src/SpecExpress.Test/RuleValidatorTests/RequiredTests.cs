using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class RequiredTests
    {
     
        [Test]
        public void When_Required_And_StringValue_Is_Null()
        {
            var customer = new Customer();

            var validator = new Required<Customer, string>();
            var context = new RuleValidatorContext<Customer, string>(customer, "Name", customer.Name, null, ValidationLevelType.Error, null);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            validator.Validate(context, null, notification);

            Assert.IsNotEmpty(notification.Errors[0].Message);
        }

        [Test]
        public void When_Required_And_CollectionValue_Is_Null()
        {
            var customer = new Customer();
            
            var validator = new Required<Customer, IEnumerable>();
            var context = new RuleValidatorContext<Customer, IEnumerable>(customer, "Contacts", customer.Contacts, null, ValidationLevelType.Error, null);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            validator.Validate(context, null, notification);

            Assert.IsNotEmpty(notification.Errors[0].Message);
        }

        [Test]
        public void When_Required_And_CollectionValue_Is_Empty_IsInvalid()
        {
            var customer = new Customer() {Contacts = new List<Contact>()};

            var validator = new Required<Customer, IEnumerable>();
            var context = new RuleValidatorContext<Customer, IEnumerable>(customer, "Contacts", customer.Contacts, null, ValidationLevelType.Error, null);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            validator.Validate(context, null, notification);

            Assert.IsNotEmpty(notification.Errors[0].Message);
        }


    }
}
