//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using SpecExpress.Test.Entities;
//using LookupFactory = SpecExpress.Test.Entities.LookupFactory;

//namespace SpecExpress.Test
//{

//    public class Configuration
//    {
//        public static string Application { get; set; }
//    }

//    [TestFixture]
//    public class SpecificationTestStub
//    {
//        [SetUp]
//        public void Setup()
//        {
//            ValidationCatalog.Reset();
//        }

//        [Test]
//        public void Test()
//        {
//            ValidationCatalog.AddSpecification<Contact>( spec =>

//                  spec.Check(c => c.Addresses).Required().ForEachSpecification<Address>(addr =>
//                        addr.Check(a => a.Country).Required()
//                            .IsInSet(LookupFactory.GetCountries())
//                            )
                  
//                  //spec.Check( c => c.Addresses).Required()
//                  //.ForEach(( c, addr) =>
//                  //             {
//                  //                 var c1 = c as Contact;
//                  //                 var a1 = addr as Address;
//                  //                 return c1.AvailableCountries.Contains(a1.Country);
//                  //             }, "Invalid Country")
//                );

//            var contact = new Contact()
//                        {
//                            AvailableCountries = new List<Country>()
//                                                     {
//                                                         new Country() { Id = "US", Name = "United States"}
//                                                     },

//                            Addresses = new List<Address>()
//                                            {
//                                                new Address()
//                                                    {
//                                                        Country = new Country(){ Id = "AA", Name = "Imagination Land"}
                                                       
//                                                    },
//                                                     new Address()
//                                                    {
//                                                        Country = new Country(){ Id = "BB", Name = "Imagination Land"}
                                                       
//                                                    }
//                                            }
//                        };


//            var vn = ValidationCatalog.Validate(contact);

//            System.Console.WriteLine( vn.ToString());


              
            
//        }

//        [Test]
//        public void TestContext()
//        {
//            ValidationCatalog.Reset();
            
//            ValidationCatalog.AddSpecification<Customer>( spec => spec.Check( c => c.Contacts).Required().ForEachSpecification<Contact>(
//                cspec => cspec.Check(c => c.Addresses).Required().ForEachSpecification<Address>(aspec =>
//                    aspec.Check(c => c.Street).Required())));
           
//            // ValidationCatalog.AddSpecification<Contact>(spec => spec.Check(c => c.Addresses).Required().ForEachSpecification<Address, AddressSpecification>());
            
            
//            //ValidationCatalog.AddSpecification<Address>(spec => spec.Check(c => c.Street).Required());

//            var customer = new Customer();
//            customer.Contacts = new List<Contact>();
            
//            var contact1 = new Contact();
//            contact1.Addresses = new List<Address>();
//            var badAddresss1 = new Address() {City = "Somewhere1"};
//            var badAddresss2 = new Address() { City = "Somewhere2" };
//            contact1.Addresses.Add(badAddresss1);
//            contact1.Addresses.Add(badAddresss2);
           
            
            
//            var contact2 = new Contact();
//            contact2.Addresses = new List<Address>();
//            contact2.Addresses.Add(new Address() { Street = "Somewhere1" });
//            contact2.Addresses.Add(new Address() { Street = "Somewhere2" });

//            customer.Contacts.Add(contact1);
//            customer.Contacts.Add(contact2);

//            var notification = ValidationCatalog.Validate(customer);

//            Assert.That(notification.IsValid, Is.False);

//            var subNotification = notification.FindDescendents(v => v.Target == badAddresss1);

//            Assert.That(subNotification.ToList().Any(), Is.True);

//        }
//    }
//}
