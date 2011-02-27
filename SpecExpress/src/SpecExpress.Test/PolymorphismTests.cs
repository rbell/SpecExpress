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

    public class InheritedClass : MyBaseClass
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

    public class InheritedClassSpecification : Validates<InheritedClass>
    {
        public InheritedClassSpecification()
        {
            Using<MyBaseClass,MyBaseClassSpecification>();
            Check(c => c.AdditionalProperty).Required();
        }
    }

    public class ClassASpecificaiton : Validates<ClassA>
    {
        public ClassASpecificaiton()
        {
            Check(c => c.BaseProperty).Required().Specification();
        }
    }
    #endregion

    [TestFixture]
    public class PolymorphismTests
    {
        [Test]
        public void InheritedSpecifications_PolymorphicProperty_GetsSpecificationForInstanceAndNotClassType()
        {
            ValidationCatalog.SpecificationContainer.Add(new MyBaseClassSpecification());
            ValidationCatalog.SpecificationContainer.Add(new InheritedClassSpecification());
            ValidationCatalog.SpecificationContainer.Add(new ClassASpecificaiton());

            var classA = new ClassA()
                             {
                                 BaseProperty =  new InheritedClass()
                             };

            var vn = ValidationCatalog.ValidateProperty(classA, c => c.BaseProperty);
            var d = vn.FindDescendents(desc => desc.Property.Name == "AdditionalProperty").ToList();
            Assert.That(d.Any() , Is.True);
        }
    }
}
