using NUnit.Framework;
using SpecExpress.Util;
using System.Collections.Generic;

namespace SpecExpress.Test
{
    public enum TestEnum
    {
        Red, Green
    }

    [TestFixture]
    public class StringExtensions
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

        [TestCase("FirstName", "First Name", Result = true)]
        [TestCase("PrimaryContact FirstName", "Primary Contact First Name", Result = true)]
        [TestCase("Customer CompanyURL", "Customer Company URL", Result = true)]
        [TestCase("Customer CompanyURL", "Customer Company URL", Result = true)]
        [TestCase("IBM", "IBM", Result = true)]
        public bool SplitPascalCase(string input, string output)
        {
            return input.SplitPascalCase() == output;
        }

        [Test]
        public void AppendPropertyNames()
        {
            var names = new List<string>() {"Name", "Country", "Address"};

            Assert.That(names.ToReverseString(), Is.EqualTo("Address Country Name"));
        }

        /// <summary>
        /// Test for fix for http://specexpress.codeplex.com/WorkItem/View.aspx?WorkItemId=4451
        /// </summary>
        [Test]
        public void AppendPropertyNames_Empty_Returns_NULL()
        {
            var names = new List<string>();
            Assert.That(names.ToReverseString(), Is.Null);
        }

    }
    
}