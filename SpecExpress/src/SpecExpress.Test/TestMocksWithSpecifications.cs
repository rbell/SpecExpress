using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SpecExpress.Util;
using SpecExpress.Test.Entities;


namespace SpecExpress.Test
{
    [TestFixture]
    public class TestMocksWithSpecifications
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

        [Test]
        public void ValidateMock()
        {
            var mockRepository = new MockRepository();

            mockRepository.Record();

            Contact contact = mockRepository.DynamicMock<Contact>();

            SetupResult.For(contact.FirstName).Return("Something");
            SetupResult.For(contact.LastName).Return("Else");

            mockRepository.ReplayAll();

            var results = ValidationCatalog.Validate<ContactSpecification>(contact);
            Assert.That(results.IsValid, Is.False);
        }

        [Test]
        public void TryGetSpecification_WithMock_ReturnsSpecification()
        {
            //Add the target specification to the container
            ValidationCatalog.AddSpecification<Contact>( x => new ContactSpecification());

            var container = new SpecificationContainer();
            container.Add<ContactSpecification>();

            //create the mock object which is proxy type
            var mockRepository = new MockRepository();
            mockRepository.Record();
            Contact contact = mockRepository.DynamicMock<Contact>();
            SetupResult.For(contact.FirstName).Return("Something");
            SetupResult.For(contact.LastName).Return("Else");
            mockRepository.ReplayAll();

            //Get a specification that matches the underlying type for the proxy
            var specification = ValidationCatalog.SpecificationContainer.TryGetSpecification(contact.GetType());

            Assert.That(specification, Is.Not.Null);
        }


    }
}   

