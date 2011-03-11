using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SpecExpress.Test
{
    [TestFixture]
    public class SpecificationInheritanceWithAbstractClasses
    {

        #region TestClasses

        public abstract class BaseClass
        {
            public string BaseName { get; set; }
        }

        public class DerivedClass : BaseClass
        {
            public string DerivedName { get; set; }
        }

        public class ClassWithAbstractProperty
        {
            public BaseClass BaseClassProperty { get; set; }
            public List<BaseClass> BaseClassCollectionProperty { get; set; }
        }

        public class BaseClassSpecification : Validates<BaseClass>
        {
            public BaseClassSpecification()
            {
                Check(c => c.BaseName).Required();
            }
        }

        public class DerivedClassSpecification : Validates<DerivedClass>
        {
            public DerivedClassSpecification()
            {
                Using<BaseClass, BaseClassSpecification>();
                Check(c => c.DerivedName).Required();
            }
        }


        public class ClassWithAbstractPropertySpecification : Validates<ClassWithAbstractProperty>
        {
            public ClassWithAbstractPropertySpecification()
            {
                Check(c => c.BaseClassProperty).Required().Specification();
                
            }
        }


        #endregion


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
        public void SpecificationAbstract_OnObject_WithSpecification_IsValid()
        {
            ValidationCatalog.AddSpecification<BaseClassSpecification>();
            ValidationCatalog.AddSpecification<DerivedClassSpecification>();
            ValidationCatalog.AddSpecification<ClassWithAbstractPropertySpecification>();
            
            var t = new DerivedClass();
            var a = new ClassWithAbstractProperty {BaseClassProperty = t};


            var n = ValidationCatalog.Validate(a); 

            Assert.That(n.IsValid, Is.False);
            Assert.That(n.All().Count(), Is.EqualTo(3));
        }

        [Test]
        public void SpecificationInheritance_OnObject_WithSpecification_IsValid()
        {
            ValidationCatalog.AddSpecification<BaseClassSpecification>();
            ValidationCatalog.AddSpecification<DerivedClassSpecification>();

            var a = new DerivedClass();
            var n = ValidationCatalog.Validate(a);

            Assert.That(n.IsValid, Is.False);
            Assert.That(n.All().Count(), Is.EqualTo(2));
        }

    }
}
