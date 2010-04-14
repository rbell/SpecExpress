using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.Boolean;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class BooleanTests
    {
        [TestCase(true, Result = true, TestName = "PropertyIsTrue")]
        [TestCase(false, Result = false, TestName = "PropertyIsFalse")]
        public bool IsTrue_IsValid(bool propertyValue)
        {
            //Create Validator
            var validator = new IsTrue<Contact>();
            RuleValidatorContext<Contact, bool> context = BuildContextForContactActive(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(true, Result = false, TestName = "PropertyIsTrue")]
        [TestCase(false, Result = true, TestName = "PropertyIsFalse")]
        public bool IsFalse_IsValid(bool propertyValue)
        {
            //Create Validator
            var validator = new IsFalse<Contact>();
            RuleValidatorContext<Contact, bool> context = BuildContextForContactActive(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        public RuleValidatorContext<Contact, bool> BuildContextForContactActive(bool value)
        {
            var contact = new Contact { Active = value };
            var context = new RuleValidatorContext<Contact, bool>(contact, "Active", contact.Active, null, ValidationLevelType.Error, null);

            return context;
        }

    }
}