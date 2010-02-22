using System;
using SpecExpress;
using SpecExpress.Test;
using SpecExpress.Test.Entities;
using System.Linq;
using SpecExpressTest.Entities;

namespace SpecExpressTest
{
    public class ContactSpecification : Validates<Contact>
    {
        public ContactSpecification()
        {
            Check(c => c.FirstName).Required();
            Check(c => c.LastName).Required();

            Check(c => c.DateOfBirth).Required().IsInPast();

            //Check(c => c, "Contact").Required()
            //    .And.Expect((x, y) => String.IsNullOrEmpty(x.LastName), "Last Name must be empty")
            //    .Or.Expect((x, y) => String.IsNullOrEmpty(x.FirstName), "First Name must be empty")
            //    .With.Message("Either First or Last Name must be empty");
            //;

            
            Check(c => c, "Contact").Required().Expect((c, x) =>
                                                               {
                                                                   return (String.IsNullOrEmpty(c.FirstName) ||
                                                                           String.IsNullOrEmpty(c.LastName));
                                                               }, "Either First or Last Name must be empty");

            //This will throw an exception
            Check(x => x.LastName).Required().Expect((y, z) => new BadWolf().IsTrue, "Error");
                
        }
    }
}