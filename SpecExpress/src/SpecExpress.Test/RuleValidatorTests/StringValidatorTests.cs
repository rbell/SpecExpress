using System.Collections.Generic;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.StringValidators;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class StringValidatorTests
    {
        /// <summary>
        /// Test MinLength Bounds
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="minLength"></param>
        /// <returns></returns>
        [TestCase("", 3, Result = false, TestName = "Empty")]
        [TestCase("      ", 3, Result = false, TestName = "Empty")]
        [TestCase(null, 3, Result = false, TestName = "Null")]
        [TestCase("Joe", 4, Result = false, TestName = "Less")]
        [TestCase("Joes", 4, Result = true, TestName = "Equal")]
        [TestCase("Joesph", 4, Result = true, TestName = "Greater")]
        public bool MinLength_IsValid(string propertyValue, int minLength)
        {
            //Create Validator
            var validator = new MinLength<Contact>(minLength);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("Joe", "Smith", Result = false, TestName = "First name less then lastname length")]
        [TestCase("Joesph", "Smith", Result = true, TestName = "First name greater than lastname length")]
        public bool MinLength_Expression_IsValid(string firstName, string lastName)
        {
            //Create Validator
            //FirstName Length must be at least the same length as the LastName
            var validator = new MinLength<Contact>(c => (int)(c.LastName.Length));
            RuleValidatorContext<Contact, string> context = BuildContextForLength(firstName, lastName);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("", 1, Result = true, TestName = "Empty")]
        [TestCase("      ", 1, Result = true, TestName = "Empty")]
        [TestCase(null, 1, Result = true, TestName = "Null")]
        [TestCase("Joesph", 7, Result = true, TestName = "Less")]
        [TestCase("Joesph", 6, Result = true, TestName = "Equal")]
        [TestCase("Joesph", 5, Result = false, TestName = "Greater")]
        public bool MaxLength_IsValid(string propertyValue, int maxLength)
        {
            //Create Validator
            var validator = new MaxLength<Contact>(maxLength);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("Joesph", "Smith", Result = false, TestName = "First name greater then lastname length")]
        [TestCase("Joe", "Smith", Result = true, TestName = "Last name greater than firstname length")]
        public bool MaxLength_Expression_IsValid(string firstName, string lastName)
        {
            //Create Validator
            //FirstName Length must be at least the same length as the LastName
            var validator = new MaxLength<Contact>(c => (int)(c.LastName.Length));
            RuleValidatorContext<Contact, string> context = BuildContextForLength(firstName, lastName);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("King Kong",".*",Result = true,TestName = "MatchAnyNumberOfCharacters")]
        [TestCase("King Kong", @"\d", Result = false, TestName = "MatchAnyNumberOfDigits")]
        public bool Matches_IsValid(string firstName, string regexPattern)
        {
            //Create Validator
            var validator = new Matches<Contact>(regexPattern);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(firstName);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("King Kong", ".*", Result = true, TestName = "MatchAnyNumberOfCharacters")]
        [TestCase("King Kong", @"\d", Result = false, TestName = "MatchAnyNumberOfDigits")]
        public bool Matches_Expression_IsValid(string firstName, string regexPattern)
        {
            //Create Validator
            var validator = new Matches<Contact>(c=>c.NamePattern);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(firstName);
            context.Instance.NamePattern = regexPattern;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }


        [TestCase("", Result = false, TestName = "Empty")]
        [TestCase("      ", Result = false, TestName = "Whitespace")]
        [TestCase(null, Result = false, TestName = "Null")]
        [TestCase("1234567890", Result = true, TestName = "all digits")]
        [TestCase("123 4567 890", Result = false, TestName = "digits with spaces")]
        [TestCase("abc", Result = false, TestName = "alpha")]
        [TestCase("1a2b3c", Result = false, TestName = "alphanumeric")]
        public bool IsNumerc_IsValid(string propertyValue)
        {
            //Create Validator
            var validator = new Numeric<Contact>();
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("", 5, 10, Result = false, TestName = "Empty")]
        [TestCase("      ", 5, 10, Result = false, TestName = "EmptyWhitespace")]
        [TestCase(null, 5, 10, Result = false, TestName = "Null")]
        [TestCase("abcd", 5, 10, Result = false, TestName = "Less")]
        [TestCase("abcde", 5, 10, Result = true, TestName = "EqualToLow")]
        [TestCase("abcdefgh", 5, 10, Result = true, TestName = "Middle")]
        [TestCase("abcdefghij", 5, 10, Result = true, TestName = "EqualToHigh")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", 5, 10, Result = false, TestName = "Greater")]
        public bool LengthBetween_IsValid(string propertyValue, int low, int high)
        {
            //Create Validator
            var validator = new LengthBetween<Contact>(low, high);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }


        [TestCase("US", Result = true, TestName = "InSet")]
        [TestCase("zz", Result = false, TestName = "NotInSet")]
        [TestCase("US ", Result = false, TestName = "ValidValue With whitespace")]
        [TestCase("  ", Result = false, TestName = "Whitespace")]
        [TestCase(null, Result = false, TestName = "Null")]
        public bool IsInSet_GenericList_IsValid(string propertyValue)
        {
            //List
            var list = new List<string> {"US", "GB", "AU", "CA"};
            //Create Validator
            var validator = new Rules.GeneralValidators.IsInSet<Contact,string>(list);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;

        }

        [TestCase("US", Result = true, TestName = "InSet")]
        [TestCase("zz", Result = false, TestName = "NotInSet")]
        [TestCase("US ", Result = false, TestName = "ValidValue With whitespace")]
        [TestCase("  ", Result = false, TestName = "Whitespace")]
        [TestCase(null, Result = false, TestName = "Null")]
        public bool IsInSet_Expression_GenericList_IsValid(string propertyValue)
        {
            //List
            var list = new List<string> { "US", "GB", "AU", "CA" };
            //Create Validator
            var validator = new Rules.GeneralValidators.IsInSet<Contact,string>(c => list);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("", Result = false, TestName = "Empty")]
        [TestCase("      ", Result = false, TestName = "Whitespace")]
        [TestCase(null, Result = false, TestName = "Null")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", Result = true, TestName = "all characters")]
        [TestCase("abcdef ghijklmno qrstuvwxyz", Result = true, TestName = "alpha with spaces")]
        [TestCase("1a2b3c", Result = false, TestName = "alphanumeric")]
        public bool IsAlpha_IsValid(string propertyValue)
        {
            //Create Validator
            var validator = new Alpha<Contact>();
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }


        [TestCase("", 0,Result = true, TestName = "Empty")]
        [TestCase("      ", 0, Result = true, TestName = "Whitespace")]
        [TestCase(null, 0, Result = true, TestName = "Null")]
        [TestCase("abcde", 5, Result = true, TestName = "5 characters")]
        [TestCase("abcdef", 4, Result = false, TestName = "5 characters invalid")]
        public bool LengthEqualTo_IsValid(string propertyValue, int length)
        {
            //Create Validator
            var validator = new LengthEqualTo<Contact>(length);
            RuleValidatorContext<Contact, string> context = BuildContextForLength(propertyValue);
            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

      


        public RuleValidatorContext<Contact, string> BuildContextForLength(string value)
        {
            var contact = new Contact {FirstName = value};
            var context = new RuleValidatorContext<Contact, string>(contact, "First Name", contact.FirstName, null, ValidationLevelType.Error, null);
            return context;
        }

        public RuleValidatorContext<Contact, string> BuildContextForLength(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                lastName = "Default";
            }

            var contact = new Contact { FirstName = firstName, LastName = lastName };
            var context = new RuleValidatorContext<Contact, string>(contact, "First Name", contact.FirstName, null, ValidationLevelType.Error, null);
            return context;
        }
    }
}