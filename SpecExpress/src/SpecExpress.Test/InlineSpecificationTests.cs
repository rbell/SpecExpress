using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SpecExpress.Test
{
    [TestFixture]
    public class InlineSpecificationTests
    {

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }


        public class PersonSpecification : Validates<Person>
        {
            public PersonSpecification()
            {
                Check(p => p.LastName).Required();
            } 
        }

       
        private string s0 = "not hello";

        private List<Person> _persons = new List<Person>();

        public string PString
        {
            get { return s0; }
        }

        [Test]
        public void Specification_Assert_ThrowsException()
        {
            string localString = "hello";

            Assert.Throws<ValidationException>(() =>
            {
                Specification.Assert(sp => sp.Check(s => localString).Required().EqualTo(PString));
            });
        }

        [Test]
        public void Specification_Validate_IsInvalid()
        {
            var localString = "hello";
            var vn = Specification.Validate(sp => sp.Check(s => localString).Required().EqualTo(PString));

            Assert.That(vn.IsValid, Is.False);
        }

        [Test]
        public void Specification_WithForEachSpecification_UsingValidationCatalog_Assert_ThrowsException()
        {
            //Add spec to catalog
            ValidationCatalog.AddSpecification<PersonSpecification>();

            //Setup test data
            _persons.Add(new Person() {FirstName = "First", LastName = "Last"});
            _persons.Add(new Person() { FirstName = "First" }); //Missing Last Name
            

            Assert.Throws<ValidationException>(() =>
            {
                Specification.Assert(sp => sp.Check(s => _persons).Required().ForEachSpecification());
            });
        }

    }
}
