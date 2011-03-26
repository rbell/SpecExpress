using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SpecExpress.Test
{
    [TestFixture]
    public class RequiredEnumsTests
    {
        [SetUp]
        public void TestSetup()
        {
            ValidationCatalog.Reset();
        }

        protected enum Gender
        {
            Male, Female
        }

        protected class Person
        {
            public Gender PersonGender { get; set; }
            public Gender? NullablePersonGender { get; set; }
            
        }

        [Test]
        public void NullableEnum_ValueIsSecondValue_IsRequired()
        {
            ValidationCatalog.AddSpecification<Person>(spec =>
                                                           {
                                                               spec.Check(p => p.PersonGender).Required();
                                                               spec.Check(p => p.NullablePersonGender).Required();
                                                           }
               );

            var person = new Person() { PersonGender = Gender.Female };

            var vn = ValidationCatalog.Validate(person);

            Assert.That(vn.IsValid, Is.False);
            Assert.That(vn.Errors.First().Message, Is.EqualTo("Nullable Person Gender is required."));
        }

        [Test]
        public void Enum_ValueIsFirstValue_IsRequired()
        {
            ValidationCatalog.AddSpecification<Person>( spec => 
                spec.Check( p => p.PersonGender).Required());

            var person = new Person() {PersonGender = Gender.Male};

            var vn = ValidationCatalog.Validate(person);

            Assert.That(vn.IsValid, Is.True);


        }

        [Test]
        public void Enum_ValueIsFirstValue_IsRequiredAndEquals()
        {
            ValidationCatalog.AddSpecification<Person>(spec =>
                spec.Check(p => p.PersonGender).Required().Equals(Gender.Male)
                );

            var person = new Person() { PersonGender = Gender.Male };

            var vn = ValidationCatalog.Validate(person);

            Assert.That(vn.IsValid, Is.True);


        }

    }
}
