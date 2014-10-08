//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SpecExpress.Test.Domain.Specifications
//{
//    using SpecExpress.Test.Domain.Entities;

//    public class CustomerCheckPropertiesExceptionSpecification : Validates<Customer>
//    {
//        public CustomerCheckPropertiesExceptionSpecification()
//        {
//            this.Check(c => c.PrimaryContact.PrimaryAddress).Required();
//        }

//        private bool DoSomethingWithFirstName(Customer arg1, string arg2)
//        {
//            return true;
//        }
//    }
//}
