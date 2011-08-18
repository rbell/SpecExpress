using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SpecExpress.Test
{
    [TestFixture]
    public class InlineSpecificationTests
    {
        private string s0 = "not hello";
        public string PString
        {
            get { return s0; }
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
