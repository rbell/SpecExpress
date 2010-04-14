using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.MessageStore;
using SpecExpress.Rules;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    [TestFixture]
    public class MessageServiceTests
    {
        [TestCase("Error Message.", "King Kong", null, null, "Error Message.", TestName = "SimpleMessage")]
        [TestCase("Error Message for {PropertyName}.", "King Kong", null, null, "Error Message for First Name.", TestName = "PropertyNameMessage")]
        [TestCase("Error Message: {PropertyValue}.", "King Kong", null, null, "Error Message: King Kong.", TestName = "PropertyValueMessage")]
        [TestCase("Error Message for {PropertyName}: {PropertyValue}.", "King Kong", null, null, "Error Message for First Name: King Kong.", TestName = "PropertyNameAndValueMessage")]
        [TestCase("Error Message {0}", "King Kong", "Param1", null, "Error Message Param1", TestName = "PropertySingleParam")]
        [TestCase("Error Message {0}", "King Kong", null, null, "Error Message ", TestName = "PropertyNullParam")]
        [TestCase("Error Message {0}", "King Kong", "", null, "Error Message ", TestName = "PropertyEmptyParam")]
        [TestCase("Error Message {0}", "King Kong", " ", null, "Error Message  ", TestName = "PropertyEmptySpaceParam")]
        [TestCase("Error Message {0}, {1}", "King Kong", "Param1", "Param2", "Error Message Param1, Param2", TestName = "PropertyDoubleParam")]
        [TestCase("Error Message for {PropertyName}: {PropertyValue}, {0}, {1}", "King Kong", "Param1", "Param2", "Error Message for First Name: King Kong, Param1, Param2", TestName = "PropertyNameValueParams")]
        public void FormatMessage_IsValid(string message, string propertyValue, string parm1, string parm2, string expectedResult)
        {
            var context = BuildContext(propertyValue);
            string result = new MessageService().FormatMessage(message, context, new object[] {parm1, parm2});
            Assert.That(result,Is.EqualTo(expectedResult));
        }

        public RuleValidatorContext<Contact, string> BuildContext(string value)
        {
            var contact = new Contact { FirstName = value };
            var context = new RuleValidatorContext<Contact, string>(contact, "First Name", contact.FirstName, null, ValidationLevelType.Error, null);
            return context;
        }
    }
}