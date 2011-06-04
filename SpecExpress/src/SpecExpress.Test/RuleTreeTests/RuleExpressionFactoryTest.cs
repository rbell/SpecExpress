using System;
using NUnit.Framework;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.Boolean;
using SpecExpress.Rules.IComparableValidators;
using SpecExpress.RuleTree;
using SpecExpress.Test.Entities;
using Contact = SpecExpress.Test.Domain.Entities.Contact;

namespace SpecExpress.Test.RuleTreeTests
{
    [TestFixture]
    public class RuleExpressionFactoryTest
    {
        [Test]
        [TestCase(true, Result = true, TestName = "PropertyIsTrue")]
        [TestCase(false, Result = false, TestName = "PropertyIsFalse")]
        public bool CreateExpression_SingleRule_BuildsExpression(bool propertyValue)
        {
            var tree = new RuleTree.RuleTree<Contact, bool>(
                new RuleTree.RuleNode(new IsTrue<Contact>()));

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            RuleValidatorContext<Contact, bool> context = BuildContextForContactActive(propertyValue);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            var isValid = tree.LambdaExpression(context, null, notification);

            return isValid;
        }

        [Test]
        [TestCase(2010, 1, 1, false, 1)]
        [TestCase(2010, 4, 1, false, 1)]
        [TestCase(2010, 2, 15, true, 0)]
        public void CreateExpression_TwoRules_AndRelation_BuildsExpression(int year, int month, int day, bool validates, int numberOfErrorMsg)
        {
            var floorDate = new DateTime(2010, 2, 1);
            var ceilingDate = new DateTime(2010, 3, 1);
            var testDate = new DateTime(year, month, day);

            var tree = new RuleTree.RuleTree<CalendarEvent, DateTime>(
                new RuleTree.RuleNode(new GreaterThan<CalendarEvent, DateTime>(floorDate)));

            tree.Root.AndChild(new RuleNode(new LessThan<CalendarEvent, DateTime>(ceilingDate)));

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            var context = BuildContextForCalendarEventStartDate(testDate);
            var notification = new ValidationNotification();
            var isValid = tree.LambdaExpression(context, null, notification);

            Assert.That(isValid, Is.EqualTo(validates));
        }

        [Test]
        [TestCase(2010, 1, 1, true, 0)]
        [TestCase(2010, 4, 1, true, 0)]
        [TestCase(2010, 2, 15, false, 1)]
        public void CreateExpression_TwoRules_OrRelation_BuildsExpression(int year, int month, int day, bool validates, int numberOfErrorMsg)
        {
            var floorDate = new DateTime(2010, 2, 1);
            var ceilingDate = new DateTime(2010, 3, 1);
            var testDate = new DateTime(year, month, day);

            var tree = new RuleTree.RuleTree<CalendarEvent, DateTime>(
                new RuleTree.RuleNode(new LessThan<CalendarEvent, DateTime>(floorDate)));

            tree.Root.OrChild(new RuleNode(new GreaterThan<CalendarEvent, DateTime>(ceilingDate)));

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            var context = BuildContextForCalendarEventStartDate(testDate);
            var notification = new ValidationNotification();
            var isValid = tree.LambdaExpression(context, null, notification);

            Assert.That(isValid, Is.EqualTo(validates));
        }


        [Test]
        [TestCase(true, Result = true, TestName = "PropertyIsTrue")]
        [TestCase(false, Result = false, TestName = "PropertyIsFalse")]
        public bool CreateExpression_GroupNodeContainingSingleRuleNode_BuildsExpression(bool propertyValue)
        {
            var tree = new RuleTree.RuleTree<Contact, bool>(
                new GroupNode(new RuleTree.RuleNode(new IsTrue<Contact>())));

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            RuleValidatorContext<Contact, bool> context = BuildContextForContactActive(propertyValue);

            var notification = new ValidationNotification();

            //Validate the validator only, return true of no error returned
            var isValid = tree.LambdaExpression(context, null, notification);

            return isValid;
        }

        [Test]
        [TestCase(2010, 1, 4, true, 0, Description = "Test that a Monday before the range is valid.")]
        [TestCase(2010, 5, 3, true, 0, Description = "Test that a Monday after the range is valid.")]
        [TestCase(2010, 2, 8, false, 1, Description = "Test that a Monday inside the range fails.")]
        [TestCase(2010, 2, 15, false, 1, Description = "Test that any date inside the range fails.")]
        [TestCase(2010, 1, 5, false, 1, Description = "Test that a non Monday before range fails.")]
        [TestCase(2010, 5, 4, false, 1, Description = "Test that a non Monday after range fails.")]
        public void CreateExpression_DateOutsideOfWindowAndIsMonday_BuildsExpression(int year, int month, int day, bool validates, int numberOfErrorMsg)
        {
            // Test: (DateTime d) => (d < floorDate | d > ceilingDate) & d.DayOfWeek == DayOfWeek.Monday
            var floorDate = new DateTime(2010, 2, 1);
            var ceilingDate = new DateTime(2010, 3, 1);
            var testDate = new DateTime(year, month, day);

            // build rule / rule node for "d.DayOfWeek == DayOfWeek.Monday
            var dateOneMondayRule = new CustomRule<CalendarEvent, DateTime>((c, d) => d.DayOfWeek == DayOfWeek.Monday);
            dateOneMondayRule.Message = "Date does not fall on a Monday.";
            var dateOneMondayRuleNode = new RuleNode(dateOneMondayRule);

            // build rules / rule nodes for "d < floorDate | d > ceilingDate"
            var rangeRuleNode = new RuleNode(new LessThan<CalendarEvent, DateTime>(floorDate));
            rangeRuleNode.OrChild(new RuleNode(new GreaterThan<CalendarEvent, DateTime>(ceilingDate)));

            // put the rules / rule nodes together using a group to enforce the "or" precidence over the "and"
            var groupNode = new GroupNode(rangeRuleNode);
            groupNode.AndChild(dateOneMondayRuleNode);

            var tree = new RuleTree<CalendarEvent, DateTime>(groupNode);

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            var context = BuildContextForCalendarEventStartDate(testDate);
            var notification = new ValidationNotification();
            var isValid = tree.LambdaExpression(context, null, notification);

            Assert.That(isValid, Is.EqualTo(validates));
        }


        [Test]
        [TestCase(2010, 1, 4, true, 0, Description = "Test that a Monday before the range is valid.")]
        [TestCase(2010, 5, 3, true, 0, Description = "Test that a Monday after the range is valid.")]
        [TestCase(2010, 2, 8, false, 1, Description = "Test that a Monday inside the range fails.")]
        [TestCase(2010, 2, 15, false, 1, Description = "Test that any date inside the range fails.")]
        [TestCase(2010, 1, 5, false, 1, Description = "Test that a non Monday before range fails.")]
        [TestCase(2010, 5, 4, false, 1, Description = "Test that a non Monday after range fails.")]
        public void CreateExpression_DateOutsideOfWindowAndIsMonday_ReverseExpression_BuildsExpression(int year, int month, int day, bool validates, int numberOfErrorMsg)
        {
            // Test: (DateTime d) => d.DayOfWeek == DayOfWeek.Monday & (d < floorDate | d > ceilingDate)
            var floorDate = new DateTime(2010, 2, 1);
            var ceilingDate = new DateTime(2010, 3, 1);
            var testDate = new DateTime(year, month, day);

            // build rules / rule nodes for "d < floorDate | d > ceilingDate" and put in a group
            var rangeRuleNode = new RuleNode(new LessThan<CalendarEvent, DateTime>(floorDate));
            rangeRuleNode.OrChild(new RuleNode(new GreaterThan<CalendarEvent, DateTime>(ceilingDate)));
            var groupNode = new GroupNode(rangeRuleNode);

            // build rule / rule node for "d.DayOfWeek == DayOfWeek.Monday
            var dateOneMondayRule = new CustomRule<CalendarEvent, DateTime>((c, d) => d.DayOfWeek == DayOfWeek.Monday);
            dateOneMondayRule.Message = "Date does not fall on a Monday.";
            var dateOneMondayRuleNode = new RuleNode(dateOneMondayRule);

            // add the rangeRuleNode as an And child of dateOneMondayRuleNode
            dateOneMondayRuleNode.AndChild(groupNode);

            var tree = new RuleTree<CalendarEvent, DateTime>(dateOneMondayRuleNode);

            Assert.That(tree.LambdaExpression, Is.Not.Null);

            var context = BuildContextForCalendarEventStartDate(testDate);
            var notification = new ValidationNotification();
            var isValid = tree.LambdaExpression(context, null, notification);

            Assert.That(isValid, Is.EqualTo(validates));
        }

        public RuleValidatorContext<Contact, bool> BuildContextForContactActive(bool value)
        {
            var contact = new Contact { Active = value };
            var context = new RuleValidatorContext<Contact, bool>(contact, "Active", contact.Active, null, ValidationLevelType.Error, null);

            return context;
        }

        public RuleValidatorContext<CalendarEvent, System.DateTime> BuildContextForCalendarEventStartDate(DateTime startDate)
        {
            var calendarEvent = new CalendarEvent() { StartDate = startDate };
            var context = new RuleValidatorContext<CalendarEvent, DateTime>(calendarEvent, "StartDate", calendarEvent.StartDate, null, ValidationLevelType.Error, null);

            return context;
        }
    }
}