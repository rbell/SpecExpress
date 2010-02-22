using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SpecExpress.Test.Domain.Entities;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ComplexTypesTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            ValidationCatalog.Reset();

            //Load specifications
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));
        }

        [TearDown]
        public void TearDown()
        {
            ValidationCatalog.Reset();
        }

        #endregion

        [Test]
        public void Validate_CustomerWithInvalidContact_WithNestedRequiredRegisteredTypes_IsInValid()
        {
            var customerWithInvalidContact = new Customer();
            customerWithInvalidContact.PrimaryContact = new Contact {FirstName = "First"};
            customerWithInvalidContact.PrimaryContact.PrimaryAddress = new Address {Street = "123 Main Street"};

            ValidationNotification results = ValidationCatalog.Validate(customerWithInvalidContact);
            Assert.That(results.Errors, Is.Not.Empty);
            Assert.That(
                results.Errors.Select(e => e.Message.Contains("Primary Contact Last Name is required.")).Any(),
                Is.True);
        }

        [Test]
        public void Validate_CustomerWithNullContact_WithNestedRequiredRegisteredTypes_IsInValid()
        {
            var customerWithMissingContact = new Customer {Name = "Customer"};
            ValidationNotification results = ValidationCatalog.Validate(customerWithMissingContact);
            Assert.That(results.Errors, Is.Not.Empty);
            //Check that null PrimaryContact only generates 1, error and the validator doesn't continue down the tree
            Assert.That(results.Errors.Count, Is.EqualTo(1));
        }
    }
}