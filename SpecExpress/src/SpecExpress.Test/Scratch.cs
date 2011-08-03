using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using SpecExpress.DSL;
using SpecExpress.Enums;
using SpecExpress.Test.Entities;
using SpecExpress.Test.Polymorphism;

namespace SpecExpress.Test
{
  

    [TestFixture]
    public class Scratch
    {

        private string s0 = "not hello";
        public string PString
        {
            get { return s0; }   
        }

        public bool DoWorkAssert(string s1, string s2)
        {
            string str = "hello";
          

            AssertSpecification.Assert(sp =>
            {
                sp.Check(s => s1).Required().EqualTo(PString);
            });

            return true;
        }

        [Test]
        public void Do()
        {
            Assert.Throws<ValidationException>(() =>
                                                      {
                                                          DoWorkAssert("hello", string.Empty);
                                                      });


        }
    }
}
