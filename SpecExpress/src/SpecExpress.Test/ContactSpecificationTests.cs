using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecExpress;
using SpecExpress.Test.Entities;
using SpecExpress.Util;
using Address=SpecExpress.Test.Domain.Values.Address;

namespace SpecExpress.Test
{
    [TestFixture]
    public class ContactSpecificationTests
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
        public void Advanced()
        {
            var contact1 = new Contact() { FirstName = "Something", LastName = "Else"};

            var results = ValidationCatalog.Validate<ContactSpecification>(contact1);

            Assert.That(results.IsValid, Is.False);
        }


        [Test]
        public void GetAllValidationResults()
        {

           
            var contact = new SpecExpress.Test.Domain.Entities.Contact()
                               {
                                   Addresses = new List<Address>()
                                                   {
                                                       new Address()
                                                           {
                                                               City = "Gatlinburg"
                                                           }
                                                   }
                                   ,
                                   PrimaryAddress = new Address()
                                                        {

                                                        }
                               };

            var results = ValidationCatalog.Validate<SpecExpress.Test.Domain.Specifications.ContactSpecification>(contact);

            CollectionAssert.IsNotEmpty(results.All());

            Assert.That(results.IsValid, Is.False);
        }

       

        [Test]
        public void Collection()
        {
            var contact1 = new Contact() { 
                FirstName = "Something", 
                LastName = "Else", 
                Addresses = new List<Entities.Address>()
                                //{
                                //    new Entities.Address()B
                                //        {
                                //            Street = "Main",
                                //            City = "Someplace",
                                //            Country = new Country()
                                //                          {
                                //                              Id = "US",
                                //                              Name = "United States"
                                //                          },
                                //                          PostalCode = "12345",

                                //                          Province = "AA"
                                //        }
                                //}
            };

            var results = ValidationCatalog.Validate<ContactSpecification>(contact1);

            Assert.That(results.IsValid, Is.False);
        }

        //[Test]
        //public void GetAllValidationResults2()
        //{
        //    Assembly assembly = Assembly.LoadFrom("SpecExpress.Test.Domain.dll");
        //    ValidationCatalog.Scan(x => x.AddAssembly(assembly));
            
        //    var contact = new SpecExpress.Test.Domain.Entities.Contact()
        //    {
        //        Addresses = new List<Address>()
        //                                           {
        //                                               new Address()
        //                                                   {
        //                                                       City = "Gatlinburg"
        //                                                   }
        //                                           }
        //        ,
        //        PrimaryAddress = new Address()
        //        {

        //        }
        //    };

        //    var results = ValidationCatalog.Validate(contact);

        //    CollectionAssert.IsNotEmpty(results.All());

        //    Assert.That(results.IsValid, Is.False);
        //}


    }
}   