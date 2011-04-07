using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SpecExpress.Test.Polymorphism
{
    #region Setup Classes
    public abstract class MyBaseClass
    {
        public string Name { get; set; }
    }

    public class InheritedClassA : MyBaseClass
    {
        public string AdditionalProperty { get; set; }
    }

    public class InheritedClassB : MyBaseClass
    {
        public string AdditionalProperty { get; set; }
    }

    public class ClassA
    {
        public MyBaseClass BaseProperty { get; set; }
        public List<MyBaseClass> BasePropertyList { get; set; }
    }
    #endregion

    #region Setup Specifications
    public class MyBaseClassSpecification: Validates<MyBaseClass>
    {
        public MyBaseClassSpecification()
        {
            Check(c => c.Name).Required();
        }
    }

    public class InheritedClassASpecification : Validates<InheritedClassA>
    {
        public InheritedClassASpecification()
        {
            Using<MyBaseClass,MyBaseClassSpecification>();
            Check(c => c.AdditionalProperty).Required();
        }
    }

    public class InheritedClassBSpecification : Validates<InheritedClassB>
    {
        public InheritedClassBSpecification()
        {
            Using<MyBaseClass, MyBaseClassSpecification>();
            Check(c => c.AdditionalProperty).Optional();
        }
    }

    public class ClassASpecification : Validates<ClassA>
    {
        public ClassASpecification()
        {
            Check(c => c.BaseProperty).Required().Specification();
            Check(c => c.BasePropertyList).Required().ForEachSpecification();
        }
    }
    #endregion

    [TestFixture]
    public class PolymorphismTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            ValidationCatalog.SpecificationContainer.Add(new MyBaseClassSpecification());
            ValidationCatalog.SpecificationContainer.Add(new InheritedClassASpecification());
            ValidationCatalog.SpecificationContainer.Add(new InheritedClassBSpecification());
            ValidationCatalog.SpecificationContainer.Add(new ClassASpecification());
        }

        [Test]
        public void InheritedSpecifications_PolymorphicProperty_GetsSpecificationForInstanceAndNotClassType()
        {
            var classA = new ClassA()
                             {
                                 BaseProperty =  new InheritedClassA()
                             };

            var vn = ValidationCatalog.ValidateProperty(classA, c => c.BaseProperty);
            var d = vn.FindDescendents(desc => desc.Property.Name == "AdditionalProperty").ToList();
            Assert.That(d.Any() , Is.True);
        }

        [Test]
        public void InheritedSpecifications_PolymorphicProperty_GetsSpecificationForInstanceAndNotClassType_Cached()
        {
            var classA = new ClassA()
            {
                BaseProperty = new InheritedClassB()
            };

            var vn = ValidationCatalog.ValidateProperty(classA, c => c.BaseProperty);
            var d = vn.FindDescendents(desc => desc.Property.Name == "AdditionalProperty").ToList();
            Assert.That(d.Any(), Is.False);
        }


        [Test]
        public void InheritedSpecifications_PolymorphicListProperty()
        {
            var invalidInheritedClassA = new InheritedClassA();
            invalidInheritedClassA.Name = "valid";
            //NULL: invalidInheritedClassA.AdditionalProperty

            var validInheritedClassB = new InheritedClassB();
            validInheritedClassB.Name = "valid";

            

            var classA = new ClassA();
            classA.BaseProperty = validInheritedClassB;
            classA.BasePropertyList = new List<MyBaseClass>()
                                          {
                                              invalidInheritedClassA,
                                              validInheritedClassB
                                          };




            var vn = ValidationCatalog.Validate(classA);
            var d = vn.FindDescendents(desc => desc.Property.Name == "BasePropertyList").ToList();
            Assert.That(d.Any(), Is.True);
        }
    }
}
