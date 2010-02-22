using System;
using System.Linq;
using NUnit.Framework;
using SpecExpress.Test.Entities;
using SpecExpressTest;
using SpecExpressTest.Entities;
using System.Collections.Generic;

namespace SpecExpress.Test.RuleValidatorTests
{
    [TestFixture]
    public class ForEachSpecificationRuleTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
             ValidationCatalog.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            ValidationCatalog.Reset();
        }

        #endregion

        [Test]
        public void ForEachSpecification_IsInvalid()
        {
            //Don't implicitly validate object graph
            ValidationCatalog.ValidateObjectGraph = false;

            //create list of contacts to validate
            var contacts = new List<Contact>
                               {
                                   new Contact() {FirstName = String.Empty, LastName = "Smith"},
                                   new Contact() {FirstName = String.Empty, LastName = String.Empty},
                                   new Contact() {FirstName = "Joe", LastName = "Smith"}
                               };

            var customer = new Customer() {Name = "Smith Industries", Contacts = contacts};
         

            //Add Specification for Customer and Address
            ValidationCatalog.AddSpecification<Customer>(spec =>
               {
                   spec.Check(c => c.Contacts).Required().ForEachSpecification<Contact>(
                       cspec =>
                           {
                               cspec.Check(c => c.LastName).Required();
                               cspec.Check(c => c.FirstName).Required();
                           });
               });

            //Validate Customer
            var results = ValidationCatalog.Validate(customer);

            Assert.That(results.Errors, Is.Not.Empty);

            var allerrors = results.Errors.First().AllErrors().ToList();

            Assert.That(results.Errors.First().NestedValdiationResults, Is.Not.Empty);

           

        }

       



    }
}