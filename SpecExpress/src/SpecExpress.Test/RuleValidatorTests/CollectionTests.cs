using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.Collection;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class CollectionTests : Validates<Contact>
    {
        [TestCase("string1", Result = true, TestName = "CollectionContains")]
        [TestCase("string100", Result = false, TestName = "CollectionDoesNotContain")]
        public bool Contains_IsValid(string lookingFor)
        {
            //Create Validator
            var validator = new Contains<Contact,IEnumerable>(lookingFor);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(PopulateListAction.Populate);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase("string1", Result = true, TestName = "CollectionContainsExpression")]
        [TestCase("string100", Result = false, TestName = "CollectionDoesNotContainExpression")]
        public bool Contains_Expression_IsValid(string lookingFor)
        {
            //Create Validator - Aliases must contain FirstName
            var validator = new Contains<Contact,IEnumerable>(c => c.FirstName);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(PopulateListAction.Populate);
            context.Instance.FirstName = lookingFor;

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Null, Result = true, TestName = "CollectionIsNull")]
        [TestCase(PopulateListAction.Empty, Result = true, TestName = "CollectionIsEmpty")]
        [TestCase(PopulateListAction.Populate, Result = false, TestName = "CollectionIsPopulated")]
        public bool IsEmpty_Expression_IsValid(PopulateListAction populateListAction)
        {
            //Create Validator
            var validator = new IsEmpty<Contact, IEnumerable>();
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Null, 3, Result = false, TestName = "Expect3WhenCollectionNull")]
        [TestCase(PopulateListAction.Null, 0, Result = true, TestName = "Expect0WhenCollectionNull")]
        [TestCase(PopulateListAction.Empty, 3, Result = false, TestName = "Expect3WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Empty, 0, Result = true, TestName = "Expect0WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Populate, 3, Result = true, TestName = "Expect3WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 4, Result = false, TestName = "Expect4WhenCollectionPopulated")]
        public bool CountEqualTo_Expression_IsValid(PopulateListAction populateListAction, int val)
        {
            //Create Validator
            var validator = new CountEqualTo<Contact, IEnumerable>(val);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }


        [TestCase(PopulateListAction.Null, 3, Result = true, TestName = "ExpectLessThan3WhenCollectionNull")]
        [TestCase(PopulateListAction.Null, 0, Result = false, TestName = "ExpectLessThan0WhenCollectionNull")]
        [TestCase(PopulateListAction.Empty, 3, Result = true, TestName = "ExpectLessThan3WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Empty, 0, Result = false, TestName = "ExpectLessThan0WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Populate, 3, Result = false, TestName = "ExpectLessThan3WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 4, Result = true, TestName = "ExpectLessThan4WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 0, Result = false, TestName = "ExpectLessThan0WhenCollectionPopulated")]
        public bool CountLessThan_Expression_IsValid(PopulateListAction populateListAction, int val)
        {
            //Create Validator
            var validator = new CountLessThan<Contact, IEnumerable>(val);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Null, 3, Result = true, TestName = "ExpectLessThanEqualTo3WhenCollectionNull")]
        [TestCase(PopulateListAction.Null, 0, Result = true, TestName = "ExpectLessThanEqualTo0WhenCollectionNull")]
        [TestCase(PopulateListAction.Empty, 3, Result = true, TestName = "ExpectLessThanEqualTo3WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Empty, 0, Result = true, TestName = "ExpectLessThanEqualTo0WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Populate, 3, Result = true, TestName = "ExpectLessThanEqualTo3WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 4, Result = true, TestName = "ExpectLessThanEqualTo4WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 0, Result = false, TestName = "ExpectLessThanEqualTo0WhenCollectionPopulated")]
        public bool CountLessThanEqual_Expression_IsValid(PopulateListAction populateListAction, int val)
        {
            //Create Validator
            var validator = new CountLessThanEqualTo<Contact, IEnumerable>(val);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Null, 3, Result = false, TestName = "ExpectGreaterThan3WhenCollectionNull")]
        [TestCase(PopulateListAction.Null, 0, Result = false, TestName = "ExpectGreaterThan0WhenCollectionNull")]
        [TestCase(PopulateListAction.Empty, 3, Result = false, TestName = "ExpectGreaterThan3WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Empty, 0, Result = false, TestName = "ExpectGreaterThan0WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Populate, 3, Result = false, TestName = "ExpectGreaterThan3WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 4, Result = false, TestName = "ExpectGreaterThan4WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 0, Result = true, TestName = "ExpectGreaterThan0WhenCollectionPopulated")]
        public bool CountGreaterThan_Expression_IsValid(PopulateListAction populateListAction, int val)
        {
            //Create Validator
            var validator = new CountGreaterThan<Contact, IEnumerable>(val);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Null, 3, Result = false, TestName = "ExpectGreaterThanEqualTo3WhenCollectionNull")]
        [TestCase(PopulateListAction.Null, 0, Result = true, TestName = "ExpectGreaterThanEqualTo0WhenCollectionNull")]
        [TestCase(PopulateListAction.Empty, 3, Result = false, TestName = "ExpectGreaterThanEqualTo3WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Empty, 0, Result = true, TestName = "ExpectGreaterThanEqualTo0WhenCollectionEmpty")]
        [TestCase(PopulateListAction.Populate, 3, Result = true, TestName = "ExpectGreaterThanEqualTo3WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 4, Result = false, TestName = "ExpectGreaterThanEqualTo4WhenCollectionPopulated")]
        [TestCase(PopulateListAction.Populate, 0, Result = true, TestName = "ExpectGreaterThanEqualTo0WhenCollectionPopulated")]
        public bool CountGreaterThanEqualTo_Expression_IsValid(PopulateListAction populateListAction, int val)
        {
            //Create Validator
            var validator = new CountGreaterThanEqualTo<Contact, IEnumerable>(val);
            RuleValidatorContext<Contact, IEnumerable> context = BuildContextForAliases(populateListAction);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        [TestCase(PopulateListAction.Populate, Result = true, TestName = "ExpectItemsAreUniqueWhenCollectionIsUnique")]
        [TestCase(PopulateListAction.NonUnique, Result = false, TestName = "ExpectItemsAreUniqueWhenCollectionIsNotUnique")]
        public bool ItemsAreUnique_IsValid(PopulateListAction action)
        {
            //Create Validator
            var validator = new ItemsAreUnique<Contact, IEnumerable>();
            var context = BuildContextForAliases(action);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null, notification);
        }

        public RuleValidatorContext<Contact, IEnumerable> BuildContextForAliases(PopulateListAction action)
        {
            Contact contact = new Contact();

            switch (action)
            {
                case PopulateListAction.Null:
                    contact.Aliases = null;
                    break;
                case PopulateListAction.Empty:
                    contact.Aliases = new List<string>();
                    break;
                case PopulateListAction.NonUnique:
                    contact.Aliases = NonUniqueStrings();
                    break;
                default:
                    contact.Aliases = UniqueStrings();
                    break;
            }

            var context = new RuleValidatorContext<Contact, IEnumerable>(contact, "Aliases", contact.Aliases, null, ValidationLevelType.Error, null);

            return context;
        }

        private List<string> UniqueStrings()
        {
            return new List<string>(new string[] { "string1", "string2", "string3" });
        }

        private List<string> NonUniqueStrings()
        {
            return new List<string>(new string[] { "string1", "string1", "string2" });
        }

        public enum PopulateListAction
        {
            Null,
            Empty,
            NonUnique,
            Populate
        }
    }
}