using System;
using NUnit.Framework;
using SpecExpress.Rules.DateValidators;
using SpecExpress.Rules.IComparableValidators;
using SpecExpress.Test.Entities;
using SpecExpress.Rules;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class DateTimeValidatorTests : Validates<CalendarEvent>
    {
        [TestCase("1/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = false, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueEquals")]
        public bool GreaterThan_IsValid(string propertyValue, string end)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime endDateTime = DateTime.Parse(end);

            //Create Validator
            var validator = new GreaterThan<CalendarEvent,DateTime>(endDateTime);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventEndDate("Test Event", DateTime.Now, propertyValueDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = false, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueEquals")]
        public bool GreaterThan_Expression_IsValid(string propertyValue, string greaterThan)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime greaterThanDateTime = DateTime.Parse(greaterThan);

            //Create Validator
            var validator = new GreaterThan<CalendarEvent,DateTime>(c => c.StartDate);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventEndDate("Test Event", greaterThanDateTime, propertyValueDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = false, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueEquals")]
        public bool GreaterThanEqualTo_IsValid(string propertyValue, string end)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime endDateTime = DateTime.Parse(end);

            //Create Validator
            var validator = new GreaterThanEqualTo<CalendarEvent,DateTime>(endDateTime);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventEndDate("Test Event", DateTime.Now, propertyValueDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = false, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueEquals")]
        public bool GreaterThanEqualTo_Expression_IsValid(string propertyValue, string greaterThan)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime greaterThanDateTime = DateTime.Parse(greaterThan);

            //Create Validator
            var validator = new GreaterThanEqualTo<CalendarEvent,DateTime>(c => c.StartDate);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventEndDate("Test Event", greaterThanDateTime, propertyValueDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = true, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueEquals")]
        public bool LessThan_IsValid(string propertyValue, string before)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime beforeDateTime = DateTime.Parse(before);

            //Create Validator
            var validator = new LessThan<CalendarEvent,DateTime>(beforeDateTime);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, DateTime.Now);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = true, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueEquals")]
        public bool LessThan_Expression_IsValid(string propertyValue, string lessThan)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime lessThanDateTime = DateTime.Parse(lessThan);

            //Create Validator
            var validator = new LessThan<CalendarEvent,DateTime>(c=>c.EndDate);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, lessThanDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = true, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueEquals")]
        public bool LessThanEqualTo_IsValid(string propertyValue, string before)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime beforeDateTime = DateTime.Parse(before);

            //Create Validator
            var validator = new LessThanEqualTo<CalendarEvent,DateTime>(beforeDateTime);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, DateTime.Now);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueIsBefore")]
        [TestCase("1/1/2010", "12/1/2009", Result = false, TestName = "DateOnlyPropertyValueIsAfter")]
        [TestCase("12/1/2009", "12/1/2009", Result = true, TestName = "DateOnlyPropertyValueEquals")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 PM", Result = true, TestName = "DateTimePropertyValueIsBefore")]
        [TestCase("12/1/2009 9:00 AM", "12/1/2009 1:00 AM", Result = false, TestName = "DateTimePropertyValueIsAfter")]
        [TestCase("12/1/2009 1:00 AM", "12/1/2009 1:00 AM", Result = true, TestName = "DateTimePropertyValueEquals")]
        public bool LessThanEqualTo_Expression_IsValid(string propertyValue, string lessThan)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);
            DateTime lessThanDateTime = DateTime.Parse(lessThan);

            //Create Validator
            var validator = new LessThanEqualTo<CalendarEvent,DateTime>(c => c.EndDate);
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, lessThanDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", Result = false, TestName = "PropertyValueIsInPast")]
        [TestCase("1/1/2100", Result = true, TestName = "PropertyValueIsInFuture")]
        public bool IsInFuture_IsValid(string propertyValue)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);

            //Create Validator
            var validator = new IsInFuture<CalendarEvent>();
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, DateTime.Now);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;            
        }

        [TestCase("1/1/2009", Result = true, TestName = "PropertyValueIsInPast")]
        [TestCase("1/1/2100", Result = false, TestName = "PropertyValueIsInFuture")]
        public bool IsInPast_IsValid(string propertyValue)
        {
            DateTime propertyValueDateTime = DateTime.Parse(propertyValue);

            //Create Validator
            var validator = new IsInPast<CalendarEvent>();
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, DateTime.Now);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", "1/1/2010", Result = true, TestName = "StartDateBetweenRange")]
        [TestCase("1/1/2010", "12/1/2009", "12/1/2010", Result = false, TestName = "StartDateBeforeRange")]
        [TestCase("1/1/2009", "1/1/2010", "12/1/2009", Result = false, TestName = "StartDateAfterRange")]
        [TestCase("1/1/2009", "1/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualFloor")]
        [TestCase("1/1/2009", "12/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualCeiling")]
        public bool Between_IsValid(string createDate, string startDate, string endDate)
        {
            DateTime createDateTime = DateTime.Parse(createDate);
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            //Create Validator
            var validator = new Between<CalendarEvent,DateTime>(createDateTime, endDateTime);
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event",
                                                                                                          createDateTime,
                                                                                                          startDateTime,
                                                                                                          endDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null; 
        }

        [TestCase("1/1/2009", "12/1/2009", "1/1/2010", Result = true, TestName = "StartDateBetweenRange")]
        [TestCase("1/1/2010", "12/1/2009", "12/1/2010", Result = false, TestName = "StartDateBeforeRange")]
        [TestCase("1/1/2009", "1/1/2010", "12/1/2009", Result = false, TestName = "StartDateAfterRange")]
        [TestCase("1/1/2009", "1/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualFloor")]
        [TestCase("1/1/2009", "12/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualCeiling")]
        public bool Between_FloorExpression_IsValid(string createDate, string startDate, string endDate)
        {
            DateTime createDateTime = DateTime.Parse(createDate);
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            //Create Validator
            var validator = new Between<CalendarEvent,DateTime>(c=>c.CreateDate, endDateTime);
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event",
                                                                                                          createDateTime,
                                                                                                          startDateTime,
                                                                                                          endDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", "1/1/2010", Result = true, TestName = "StartDateBetweenRange")]
        [TestCase("1/1/2010", "12/1/2009", "12/1/2010", Result = false, TestName = "StartDateBeforeRange")]
        [TestCase("1/1/2009", "1/1/2010", "12/1/2009", Result = false, TestName = "StartDateAfterRange")]
        [TestCase("1/1/2009", "1/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualFloor")]
        [TestCase("1/1/2009", "12/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualCeiling")]
        public bool Between_CeilingExpression_IsValid(string createDate, string startDate, string endDate)
        {
            DateTime createDateTime = DateTime.Parse(createDate);
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            //Create Validator
            var validator = new Between<CalendarEvent,DateTime>(createDateTime, c=>c.EndDate);
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event",
                                                                                                          createDateTime,
                                                                                                          startDateTime,
                                                                                                          endDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", "12/1/2009", "1/1/2010", Result = true, TestName = "StartDateBetweenRange")]
        [TestCase("1/1/2010", "12/1/2009", "12/1/2010", Result = false, TestName = "StartDateBeforeRange")]
        [TestCase("1/1/2009", "1/1/2010", "12/1/2009", Result = false, TestName = "StartDateAfterRange")]
        [TestCase("1/1/2009", "1/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualFloor")]
        [TestCase("1/1/2009", "12/1/2009", "12/1/2009", Result = true, TestName = "StartDateEqualCeiling")]
        public bool Between_Expressions_IsValid(string createDate, string startDate, string endDate)
        {
            DateTime createDateTime = DateTime.Parse(createDate);
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);

            //Create Validator
            var validator = new Between<CalendarEvent,DateTime>(c=>c.CreateDate, c => c.EndDate);
            RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event",
                                                                                                          createDateTime,
                                                                                                          startDateTime,
                                                                                                          endDateTime);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        [TestCase("1/1/2009", Result = true, TestName = "PropertyValueIsInPast")]
        [TestCase("1/1/2100", Result = false, TestName = "PropertyValueIsInFuture")]
        public bool IsInPast_NullableDate_IsValid(string propertyValue)
        {
            DateTime? propertyValueDateTime = null;

            //Create Validator
            var validator = new IsInPastNullable<NullableCalendarEvent>();
            
            // Build context for CalendarEvent containing a StartDate of propertyValue.
            var calendarEvent = new NullableCalendarEvent() { CreateDate = null, StartDate = System.DateTime.Parse(propertyValue), EndDate = null };
            RuleValidatorContext<NullableCalendarEvent, DateTime?> context = new RuleValidatorContext<NullableCalendarEvent, DateTime?>(calendarEvent, "StartDate", calendarEvent.StartDate, null, null);


            //RuleValidatorContext<CalendarEvent, DateTime> context = BuildContextForCalendarEventStartDate("Test Event", propertyValueDateTime, DateTime.Now);

            //Validate the validator only, return true of no error returned
            return validator.Validate(context, null) == null;
        }

        public RuleValidatorContext<CalendarEvent, System.DateTime> BuildContextForCalendarEventStartDate(string subject, DateTime startDate, DateTime endDate)
        {
            var calendarEvent = new CalendarEvent() {Subject = subject, StartDate = startDate, EndDate = endDate};
            var context = new RuleValidatorContext<CalendarEvent,DateTime>(calendarEvent, "StartDate", calendarEvent.StartDate, null, null);

            return context;
        }

        public RuleValidatorContext<CalendarEvent, System.DateTime> BuildContextForCalendarEventStartDate(string subject, DateTime createDate, DateTime startDate, DateTime endDate)
        {
            var calendarEvent = new CalendarEvent() { Subject = subject, CreateDate = createDate, StartDate = startDate, EndDate = endDate };
            var context = new RuleValidatorContext<CalendarEvent, DateTime>(calendarEvent, "StartDate", calendarEvent.StartDate, null, null);

            return context;
        }

        public RuleValidatorContext<CalendarEvent, System.DateTime> BuildContextForCalendarEventEndDate(string subject, DateTime startDate, DateTime endDate)
        {
            var calendarEvent = new CalendarEvent() { Subject = subject, StartDate = startDate, EndDate = endDate };
            var context = new RuleValidatorContext<CalendarEvent, DateTime>(calendarEvent, "EndDate", calendarEvent.EndDate, null, null);

            return context;
        }

    }
}