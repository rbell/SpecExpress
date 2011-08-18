//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using NUnit.Framework;
//using SpecExpress.DSL;
//using SpecExpress.Enums;
//using SpecExpress.Test.Entities;
//using SpecExpress.Test.Polymorphism;

//namespace SpecExpress.Test
//{
  

//    [TestFixture]
//    public class Scratch
//    {

//        private string s0 = "not hello";
//        public string PString
//        {
//            get { return s0; }   
//        }

//        [Test]
//        public void Specification_Assert_ThrowsException()
//        {
//            string localString = "hello";

//            Assert.Throws<ValidationException>(() =>
//                                                      {
//                                                          Specification.Assert(sp => sp.Check(s => localString).Required().EqualTo(PString));
//                                                      });
//        }

//        [Test]
//        public void Specification_Validate_IsInvalid()
//        {
//            var localString = "hello";
//            var vn = Specification.Validate(sp => sp.Check(s => localString).Required().EqualTo(PString));

//            Assert.That(vn.IsValid, Is.False);
//        }
//    }
//}
