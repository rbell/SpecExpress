using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.Concurrency
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ValidationCatalogTests
    {
        public ValidationCatalogTests()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            ValidationCatalog.Reset();
            ValidationCatalog.ResetConfiguration();
        }

        [TestMethod]
        [HostType("Chess")]
        public void ScanConcurrently()
        {
            var childThread = new Thread(() =>
                                             {
                                                 Assembly assembly1 = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
                                                 ValidationCatalog.Scan(x => x.AddAssembly(assembly1));
                                             });

            childThread.Start();

            Assembly assembly2 = Assembly.LoadFrom("SpecExpress.Test.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly2));

            childThread.Join();

            int specCount = ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications().Count;

            Assert.AreEqual(21, specCount);
        }

        [TestMethod]
        [HostType("Chess")]
        public void ScanAndEnumerateSpecificationsConcurrently()
        {
            var childThread = new Thread(() =>
            {
                Assembly assembly1 = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
                ValidationCatalog.Scan(x => x.AddAssembly(assembly1));
            });

            childThread.Start();

            Assembly assembly2 = Assembly.LoadFrom("SpecExpress.Test.dll");
            ValidationCatalog.Scan(x => x.AddAssembly(assembly2));

            foreach (var specification in ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications())
            {
                Assert.IsNotNull(specification);
            }

            childThread.Join();
        }


        [TestMethod]
        [HostType("Chess")]
        public void ValidateConcurrently()
        {
            ValidationCatalog.AddSpecification<Customer>(s => s.Check(c => c.Name).Required().MaxLength(50) );
            Customer customer1 = new Customer() { Name = string.Empty.PadLeft(55, 'X') };
            Customer customer2 = new Customer() { Name = string.Empty.PadLeft(45, 'X') };

            var childThread = new Thread(() =>
            {
                var customer1Notification = ValidationCatalog.Validate(customer1);
                Assert.IsFalse(customer1Notification.IsValid);
            });

            childThread.Start();

            var customer2Notification = ValidationCatalog.Validate(customer2);
            Assert.IsTrue(customer2Notification.IsValid);

            childThread.Join();
        }
    }
}
