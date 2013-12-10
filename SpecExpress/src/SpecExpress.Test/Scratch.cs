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

        public class Range
        {
            public int From { get; set; }
            public int To { get; set; }
        }

        [Test]
        public void Test_WithMessageFormat()
        {
            var range = new Range()
                {
                    From = 3,
                    To = 4
                };

            int value = 2;
            var vn = Specification.Validate(spec => spec.Check(c => value).Required()
                                               .Between(range.From, range.To)
                                               .With(
                                                   m =>
                                                   m.MessageFormat("Floor: {floor} Ceiling: {ceiling} Values: {0}, {1}",
                                                                   new object[] {range.From, range.To})));
            Assert.That(vn.IsValid, Is.False);
        }

        [Test]
        public void Specification_Assert_ThrowsException()
        {
            string localString = "hello";

            Assert.Throws<ValidationException>(() =>
                                                      {
                                                          Specification.Assert(sp => sp.Check(s => localString).Required().EqualTo(PString));
                                                      });
        }

        [Test]
        public void Specification_Validate_IsInvalid()
        {
            var localString = "hello";
            var vn = Specification.Validate(sp => sp.Check(s => localString).Required().EqualTo(PString));

            Assert.That(vn.IsValid, Is.False);
        }
    }
}
