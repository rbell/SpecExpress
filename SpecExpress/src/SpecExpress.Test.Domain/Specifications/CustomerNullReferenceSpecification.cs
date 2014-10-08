//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SpecExpress.Test.Domain.Specifications
//{
//    using SpecExpress.Test.Domain.Entities;

//    public class CustomerNullReferenceSpecification : Validates<Customer>
//    {
//        public CustomerNullReferenceSpecification()
//        {
//            this.Check(c => c.Employees).Optional().ForEachSpecification();
//        }
//    }
//}
