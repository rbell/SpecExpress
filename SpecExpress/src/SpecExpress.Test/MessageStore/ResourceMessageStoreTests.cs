using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpecExpress.MessageStore;
using SpecExpress.Rules;
using SpecExpress.Rules.StringValidators;
using SpecExpress.Test.Entities;
using SpecExpress.Test.MessageStore;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ResourceMessageStoreTests
    {
        [SetUp]
        public void Setup()
        {
            ValidationCatalog.ResetConfiguration();
            ValidationCatalog.Reset();
        }

        [Test]
        public void GetFormattedErrorMessage_ReturnsFormattedString()
        {
            //Create an Entity
            var emptyContact = new Contact();
            emptyContact.FirstName = null;
            emptyContact.LastName = null;

            //Create PropertyValidator
            var propertyValidator =
                new PropertyValidator<Contact, string>(contact => contact.LastName);

            //Create a rule
            RuleValidator<Contact, string> ruleValidator = new LengthBetween<Contact>(1, 5);

            //Create a context
            var context = new RuleValidatorContext<Contact, string>(emptyContact, propertyValidator, null);

            //create it like this? IOC? Factory?
            //IMessageStore messageStore = new ResourceMessageStore();

            //string errorMessage = messageStore.GetFormattedDefaultMessage(ruleValidator.GetType().Name, context, ruleValidator.Parameters);
            var messageService = new MessageService();
           
            var errorMessage = messageService.GetDefaultMessageAndFormat(new MessageContext(context, ruleValidator.GetType(), false, null, null), ruleValidator.Parameters);

            Assert.That(errorMessage, Is.Not.Null.Or.Empty);

            Assert.That(errorMessage, Is.StringContaining("Last Name"));
            Assert.That(errorMessage, Is.StringContaining("1"));
            Assert.That(errorMessage, Is.StringContaining("5"));
            //null: Search for Actual value but it's empty b/c the value is null
        }

        [Test]
        public void GetMessageForRuleWithMessageOverrride()
        {
            ValidationCatalog.Configure( x=>x.AddMessageStore(new ResourceMessageStore(TestRuleErrorMessages.ResourceManager), "OverrideMessages"));

            ValidationCatalog.AddSpecification<Contact>(c =>
                                                            {
                                                                c.Check(x => x.LastName).Required().IsAlpha();
                                                            }
                );

            //Create an Entity
            var contact = new Contact();
            contact.FirstName = null;
            contact.LastName = "1111";

            var results = ValidationCatalog.ValidateProperty(contact, c => c.LastName);

            Assert.That(results.Errors.ToList().First().Message == "Last Name should only contain letters, big boy!");
        }

        [Test]
        public void GetMessageForRuleWithMessageOverrrideAndMessageKey()
        {
            ValidationCatalog.Configure(x => x.AddMessageStore(new ResourceMessageStore(TestRuleErrorMessages.ResourceManager), "OverrideMessages"));
            
            ValidationCatalog.AddSpecification<Contact>(c =>
            {
                c.Check(x => x.LastName).Required().IsAlpha().With(m => m.MessageKey = "TestRule");
            }
                );

            //Create an Entity
            var contact = new Contact();
            contact.FirstName = null;
            contact.LastName = "1111";

            var results = ValidationCatalog.ValidateProperty(contact, c => c.LastName);

            Assert.That(results.Errors.ToList().First().Message == "Last Name is invalid!");
        }
    }

    

}