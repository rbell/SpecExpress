using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SpecExpress;
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

    }
}   

