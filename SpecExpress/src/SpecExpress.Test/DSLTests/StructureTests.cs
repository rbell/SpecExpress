using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpecExpress.MessageStore;
using SpecExpress.Rules.Collection;
using SpecExpress.Test.Entities;
using SpecExpress.Test;

namespace SpecExpress.Test.DSLTests
{
    /// <summary>
    /// This class is not actually a unit test but rather verifies that the format structure of the DSL does not change by
    /// making sure varients of the structure compile without issue.
    /// </summary>
    public class StructureTests : Validates<Customer>
    {
        /// <summary>
        /// Ensures that various Check statements compile:
        ///     Required and Optional
        ///     Conditional If
        ///     ActionJoins
        ///     With.Message
        /// </summary>
        public void EssentialCompileCheckDSLStatements()
        {
            Check(c => c.Name).If(c => c.CustomerDate > DateTime.Now).Required()
                .With(m => m.Message = "Name is Required")
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .And.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Dumber Message");

            Check(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Required().With(m => m.Message = "You broke a rule!")
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .Or.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Dumber Message");

            Check(c => c.Name).Required().LengthBetween(0, 10);

            Check(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Optional()
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .And.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Dumber Message");

            Check(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Optional()
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .Or.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Message");

            Check(c => c.Name).Optional().LengthBetween(0, 10);

            Check(c => c.Name).Required().Not.LengthBetween(0, 10);
        }


        /// <summary>
        /// Ensures that various Warn statements compile:
        ///     Required and Optional
        ///     Conditional If
        ///     ActionJoins
        ///     With.Message
        /// </summary>
        public void EssentialCompileWarnDSLStatements()
        {
            Warn(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Required().With(m => m.Message = "You broke a rule!")
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .And.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Message");

            Warn(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Required().With(m => m.Message = "You broke a rule!")
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .Or.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Message");

            Warn(c => c.Name).Required().LengthBetween(0, 10);

            Warn(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Optional()
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .And.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Message");

            Warn(c => c.Name).If(c => c.CustomerDate > DateTime.Now)
                .Optional()
                .LengthBetween(0, 10).With(m => m.Message = "Message")
                .Or.IsInSet(new List<string>(new[] { "Msg", "Another" })).With(m => m.Message = "Message");

            Warn(c => c.Name).Optional().LengthBetween(0, 10);
        }

        /// <summary>
        /// Ensures that various IComparable statements compile:
        ///     GreaterThan
        ///     GreaterThanEqualTo
        ///     LessThan
        ///     LessThanEqualTo
        ///     Between
        /// </summary>
        public void IComparableRules()
        {
            // Greater Than Constant
            Check(c => c.CustomerDate).Required().GreaterThan(DateTime.Now);
            // Greater Than Expression
            Check(c => c.CustomerDate).Required().GreaterThan(c => c.ActiveDate);
            // Greater Than Equal To Constant
            Check(c => c.CustomerDate).Required().GreaterThanEqualTo(DateTime.Now);
            // Greater Than Equal To Expression
            Check(c => c.CustomerDate).Required().GreaterThanEqualTo(c => c.ActiveDate);
            // Less Than Constant
            Check(c => c.CustomerDate).Required().LessThan(DateTime.Now);
            // Less Than Expression
            Check(c => c.CustomerDate).Required().LessThan(c => c.ExpireDate);
            // Less Than Equal To Constant
            Check(c => c.CustomerDate).Required().LessThanEqualTo(DateTime.Now);
            // Less Than Equal To Expression
            Check(c => c.CustomerDate).Required().LessThanEqualTo(c => c.ExpireDate);
            // Between constant and constant
            Check(c => c.CustomerDate).Required().Between(new DateTime(200, 1, 1), DateTime.Now);
            // Between expression and constant
            Check(c => c.CustomerDate).Required().Between(c => c.ActiveDate, DateTime.Now);
            // Between constant and expression
            Check(c => c.CustomerDate).Required().Between(new DateTime(200, 1, 1), c => c.ExpireDate);
            // Between expression and expression
            Check(c => c.CustomerDate).Required().Between(c => c.ActiveDate, c => c.ExpireDate);
        }

        /// <summary>
        /// Ensures that String statements compile:
        ///     IsAlpha
        ///     IsInSet
        ///     LengthBetween
        ///     MinLength
        ///     MaxLength
        ///     Numeric
        /// </summary>
        public void StringRules()
        {
            // IsAlpha
            Check(c => c.Name).Required().IsAlpha();

            // IsInSet
            Check(c => c.Name).Required().IsInSet(new string[] { "Option1", "Option2" });
            Check(c => c.Name).Required().IsInSet(new List<string>(new string[] { "Option1", "Option2" }));
            Check(c => c.Address.Country.Id).Required().IsInSet(c => c.Address.CountryList);

            // LengthBetween
            Check(c => c.Name).Required().LengthBetween(0, 100);
            Check(c => c.Name).Required().LengthBetween(0, c => c.Max);
            Check(c => c.Name).Required().LengthBetween(c => c.Min, 100);
            Check(c => c.Name).Required().LengthBetween(c => c.Min, c => c.Max);

            // MinLength
            Check(c => c.Name).Required().MinLength(100);
            Check(c => c.Name).Required().MinLength(c => c.Min);

            // MaxLength
            Check(c => c.Name).Required().MaxLength(100);
            Check(c => c.Name).Required().MaxLength(c => c.Max);

            // Matches
            Check(c => c.Name).Required().Matches(".*");
            Check(c => c.Name).Required().Matches(c => c.NamePattern);

            // Numeric
            Check(c => c.Id).Required().IsNumeric();
        }

        /// <summary>
        /// Ensures that Collection statements compile:
        ///     Contains
        ///     CheckForEach
        /// </summary>
        public void CollectionRules()
        {
            // Contains
            Check(c => c.Contacts).Required().Contains(new Contact());

            // CheckForEach
            Check(c => c.Contacts).Required().ForEach(c => ((Contact)c).Active,
                                                               "Contact {FirstName} {LastName} should be active.");

            Check(c => c.Contacts).Required().ForEach(c => ((Contact)c).Active, ValidationCatalog.Configuration.DefaultMessageStore.GetMessageTemplate("AllContactActive"));

            Check(c => c.Contacts).Required().ForEachSpecification<Contact>();
            Check(c => c.Contacts).Required().ForEachSpecification<Contact, ContactSpecification>();

            // CheckForEach with Linq
            Check(c => from contact in c.Contacts where contact.Active select new { BirthDate = contact.DateOfBirth })
                .Optional()
                .ForEach(generic => /* what to cast generic to since its generic? */ true, "Some Error");

            // Count Rules
            Check(c => c.Contacts).Required().CountEqualTo(0);
            Check(c => c.Contacts).Required().CountGreaterThan(0);
            Check(c => c.Contacts).Required().CountGreaterThanEqualTo(0);
            Check(c => c.Contacts).Required().CountLessThan(0);
            Check(c => c.Contacts).Required().CountLessThanEqualTo(0);
            Check(c => c.Contacts).Required().IsEmpty();
            Check(c => c.Contacts).Required().Not.CountEqualTo(0);
            Check(c => c.Contacts).Required().Not.CountGreaterThan(0);
            Check(c => c.Contacts).Required().Not.CountGreaterThanEqualTo(0);
            Check(c => c.Contacts).Required().CountLessThan(0);
            Check(c => c.Contacts).Required().CountLessThanEqualTo(0);
            Check(c => c.Contacts).Required().IsEmpty();
        }

        /// <summary>
        /// Ensures that a custom rule compiles
        /// </summary>
        public void CustomRules()
        {
            Check(c => c.Name).Required().Expect((c, name) => name == "A valid name.", "You entered an invalid name.");
            Check(c => c.Name).Required().Expect(ValidName, "You entered an invalid name.");
        }


        private bool ValidName(Customer customer, string name)
        {
            return name == "A valid name.";
        }
    }
}