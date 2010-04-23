using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SpecExpress.Test.Domain.Entities;
using SpecExpress.Test.Domain.Specifications;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test
{
    [TestFixture]
    public class SpecificationRegistryTests
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
        public void ResetRegistries_RegisterAndReset_RegistryIsClean()
        {
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            //Set Assemblies to scan for Specifications
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));
            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications(), Is.Not.Empty);

            ValidationCatalog.Reset();
            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications(), Is.Empty);
            
        }

        [Test]
        public void TheCallingAssembly_FindsSpecifications()
        {
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");

            //Set Assemblies to scan for Specifications
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));

            //Assert.That(ValidationCatalog.Registry, Is.Not.Empty);
            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetSpecification<Address>(), Is.Not.Null);
        }

        [Test]
        public void Scan_PathForSpecification_SpecsFound()
        {
            //Set Assemblies to scan for Specifications
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));
           
            //ValidationCatalog.Scan(x => x.AddAssembliesFromPath(@"C:\Dev\SpecExpress\trunk\SpecExpress\src\SpecExpressTest\bin\Debug"));

            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications().Any(), Is.True);
        }
        
        [Test]
        [Ignore]
        public void Scan_AppDomainForSpecification_SpecsFound()
        {
            //In Resharper Unit Test, generates:
            //NotSupportedException: The invoked member is not supported in a dynamic assembly
            
            //Set Assemblies to scan for Specifications
            ValidationCatalog.Scan(x => x.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies().ToList()));
            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications().Any(), Is.True);
        }

        [Test]
        public void Scan_Specification_Inhertitance()
        {
            //Set Assemblies to scan for Specifications
            Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly));

            //ValidationCatalog.Scan(x => x.AddAssembliesFromPath(@"C:\Dev\SpecExpress\trunk\SpecExpress\src\SpecExpressTest\bin\Debug"));

            Assert.That(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications().Any(), Is.True);

            var USWidget = new Widget() {Name = "ABC"};
            var invalidLengthUSWidget = new Widget() { Name = "ABCDFEFGA" };
            var IntWidget = new Widget() { Name = "123" };

            var results = ValidationCatalog.Validate(USWidget);
            Assert.That(results.IsValid, Is.True);

            var results2 = ValidationCatalog.Validate<InternationalWidgetSpecification>(IntWidget);
            Assert.That(results2.IsValid, Is.True);

            var results3 = ValidationCatalog.Validate(IntWidget);
            Assert.That(results3.IsValid, Is.False);

            var results4 = ValidationCatalog.Validate(invalidLengthUSWidget);
            Assert.That(results4.IsValid, Is.False);



        }

       

    }
}