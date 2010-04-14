using System;
using NUnit.Framework;
//using SpecExpress.Rules.NumericValidators.Int;
using SpecExpress.Enums;
using SpecExpress.Test.Entities;
using SpecExpress.Rules;
using SpecExpress.Rules.IComparableValidators;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class IComparableValidatorTests : Validates<Contact>
    {

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = true, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool GreaterThanEqualTo_IsValid(int propertyValue, int greaterThanEqualTo)
        {
            //Create Validator
            var validator = new GreaterThanEqualTo<Contact,int>(greaterThanEqualTo);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = true, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool GreaterThanEqualTo_Expression_IsValid(int propertyValue, int greaterThanEqualTo)
        {
            //Create Validator
            var validator = new GreaterThanEqualTo<Contact,int>(c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = greaterThanEqualTo;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = false, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = true, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool GreaterThan_IsValid(int propertyValue, int greaterThan)
        {
            //Create Validator
            var validator = new GreaterThan<Contact,int>(greaterThan);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = false, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = true, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool GreaterThan_Expression_IsValid(int propertyValue, int greaterThan)
        {
            //Create Validator
            var validator = new GreaterThan<Contact,int>(c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = greaterThan;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = true, TestName = "PropertyLessThan")]
        public bool LessThanEqualTo_IsValid(int propertyValue, int lessThanEqualTo)
        {
            //Create Validator
            var validator = new LessThanEqualTo<Contact,int>(lessThanEqualTo);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = true, TestName = "PropertyLessThan")]
        public bool LessThanEqualTo_Expression_IsValid(int propertyValue, int lessThanEqualTo)
        {
            //Create Validator
            var validator = new LessThanEqualTo<Contact,int>(c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = lessThanEqualTo;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = false, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = true, TestName = "PropertyLessThan")]
        public bool LessThan_IsValid(int propertyValue, int lessThan)
        {
            //Create Validator
            var validator = new LessThan<Contact,int>(lessThan);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = false, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = true, TestName = "PropertyLessThan")]
        public bool LessThan_Expression_IsValid(int propertyValue, int lessThan)
        {
            //Create Validator
            var validator = new LessThan<Contact,int>(c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = lessThan;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool EqualTo_IsValid(int propertyValue, int equalTo)
        {
            //Create Validator
            var validator = new EqualTo<Contact,int>(equalTo);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, Result = true, TestName = "PropertyEqual")]
        [TestCase(2, 1, Result = false, TestName = "PropertyGreater")]
        [TestCase(0, 1, Result = false, TestName = "PropertyLessThan")]
        public bool EqualTo_Expression_IsValid(int propertyValue, int equalTo)
        {
            //Create Validator
            var validator = new EqualTo<Contact,int>(c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = equalTo;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }


        [TestCase(1, 1, 10, Result = true, TestName = "PropertyEqualFloor")]
        [TestCase(10, 1, 10, Result = true, TestName = "PropertyEqualCeiling")]
        [TestCase(5, 1, 10, Result = true, TestName = "PropertyWithinRange")]
        [TestCase(11, 1, 10, Result = false, TestName = "PropertyGreaterThanCeiling")]
        [TestCase(0, 1, 10, Result = false, TestName = "PropertyLessThanFloor")]
        public bool Between_IsValid(int propertyValue, int floor, int ceiling)
        {
            //Create Validator
            var validator = new Between<Contact, int>(floor, ceiling);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, 10, Result = true, TestName = "PropertyEqualFloor")]
        [TestCase(10, 1, 10, Result = true, TestName = "PropertyEqualCeiling")]
        [TestCase(5, 1, 10, Result = true, TestName = "PropertyWithinRange")]
        [TestCase(11, 1, 10, Result = false, TestName = "PropertyGreaterThanCeiling")]
        [TestCase(0, 1, 10, Result = false, TestName = "PropertyLessThanFloor")]
        public bool Between_FloorExpression_IsValid(int propertyValue, int floor, int ceiling)
        {
            //Create Validator
            var validator = new Between<Contact, int>(c => c.NumberOfDependents, ceiling);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = floor;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, 10, Result = true, TestName = "PropertyEqualFloor")]
        [TestCase(10, 1, 10, Result = true, TestName = "PropertyEqualCeiling")]
        [TestCase(5, 1, 10, Result = true, TestName = "PropertyWithinRange")]
        [TestCase(11, 1, 10, Result = false, TestName = "PropertyGreaterThanCeiling")]
        [TestCase(0, 1, 10, Result = false, TestName = "PropertyLessThanFloor")]
        public bool Between_CielingExpression_IsValid(int propertyValue, int floor, int ceiling)
        {
            //Create Validator
            var validator = new Between<Contact, int>(floor, c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = ceiling;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase(1, 1, 10, Result = true, TestName = "PropertyEqualFloor")]
        [TestCase(10, 1, 10, Result = true, TestName = "PropertyEqualCeiling")]
        [TestCase(5, 1, 10, Result = true, TestName = "PropertyWithinRange")]
        [TestCase(11, 1, 10, Result = false, TestName = "PropertyGreaterThanCeiling")]
        [TestCase(0, 1, 10, Result = false, TestName = "PropertyLessThanFloor")]
        public bool Between_Expressions_IsValid(int propertyValue, int floor, int ceiling)
        {
            //Create Validator
            var validator = new Between<Contact, int>(c => c.NumberOfChildren, c => c.NumberOfDependents);
            RuleValidatorContext<Contact, int> context = BuildContextForNumberOfDependents(propertyValue);
            context.Instance.NumberOfDependents = ceiling;
            context.Instance.NumberOfChildren = floor;

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        public RuleValidatorContext<Contact, int> BuildContextForNumberOfDependents(int value)
        {
            var contact = new Contact { NumberOfDependents = value };
            var context = new RuleValidatorContext<Contact, int>(contact, "NumberOfDependents", contact.NumberOfDependents, null, ValidationLevelType.Error, null);

            return context;
        }
    }
}