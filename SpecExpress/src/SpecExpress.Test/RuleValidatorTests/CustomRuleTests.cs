using System;
using NUnit.Framework;
using SpecExpress.Rules;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class CustomRuleTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [TestCase("IsValidName", Result = true, TestName = "ValidProperty")]
        [TestCase("IsNotValidName", Result = false, TestName = "InvalidProperty")]
        public bool CustomRule_WithExpression_IsValid(string propertyValue)
        {
            //Create Validator
            var validator = new CustomRule<Contact, string>((c, name) => name.ToUpper() == "ISVALIDNAME");
            validator.Message = "Invalid Name";
            RuleValidatorContext<Contact, string> context = BuildContextForName(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("IsValidName", Result = true, TestName = "ValidProperty")]
        [TestCase("IsNotValidName", Result = false, TestName = "InvalidProperty")]
        public bool CustomRule_WithDelegate_IsValid(string propertyValue)
        {
            //Create Validator
            var validator = new CustomRule<Contact, string>(IsValidName);
            validator.Message = "Invalid Name";
            RuleValidatorContext<Contact, string> context = BuildContextForName(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        private bool IsValidName(Contact contact, string name)
        {
            return name.ToUpper() == "ISVALIDNAME";
        }

        private RuleValidatorContext<Contact, string> BuildContextForName(string propertyValue)
        {
            var contact = new Contact { FirstName = propertyValue };
            var context = new RuleValidatorContext<Contact, string>(contact, "FirstName", contact.FirstName, null, null);

            return context;
        }
    }
}