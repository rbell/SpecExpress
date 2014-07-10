using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Test.Entities;


namespace SpecExpress.Test
{
    [TestFixture]
    public class ExpressionValidationTests
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
        public void InvalidExpression_IsInvalid()
        {
            ValidationCatalog.AddSpecification<Contact>(x => x.Check(c => c.NumberOfDependents).Required().   
                                                                 GreaterThan( z=> new BadWolf().Max(z.NumberOfDependents)));

            var contact = new Contact() {LastName = "Bill"};

            var results = ValidationCatalog.Validate(contact);

            Assert.That(results.Errors, Is.Not.True);

           

        }


        [Test]
        public void ValidateMethodCallProperty_IsSuccessful()
        {
            //ConstantExpress
            Specification.Assert( sp=> sp.Check( x => GetCollection()).Required().CountGreaterThan(0));
            
        }

        [Test]
        public void ValidateMethodCallPropertyOnClass_IsSuccessful()
        {
            ValidationCatalog.AddSpecification<StubClass>(validates => validates.Check( x=> x.GetCollection()).Required().CountGreaterThan(1));
            var c = new StubClass();
            Assert.DoesNotThrow( () => ValidationCatalog.Validate(c)   );
        }

        public class StubClass
        {
            public List<string> GetCollection()
            {
                 return new List<string>()
                {
                    "A",
                    "B",
                    "C"
                };
            }
        }
        public List<string> GetCollection()
        {
            return new List<string>()
            {
                "A",
                "B",
                "C"
            };
        }
    }
}
