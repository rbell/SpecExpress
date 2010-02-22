using NUnit.Framework;
using SpecExpress.DSL;
using SpecExpress.Test.Entities;
using SpecExpress.Test.Factories;

namespace SpecExpress.Test.DSLTests.Functional
{
    /// <summary>
    /// Test Fixture for the ActionOptionConditionBuilder which confirms that it modifies the PropertyValidator appropriatly and
    /// returns the next appropriate builder in the DSL.
    /// </summary>
    [TestFixture]
    public class ActionOptionConditionBuilderTests
    {
        [Test]
        public void
            If_NameLengtGreaterThan10_SetsPropertyValidatorCondition_ReturnsActionOptionBuilder()
        {
            // Create Dependancies
            PropertyValidator<Customer, string> validator = PropertyValidatorFactory.DefaultCustomerNameValidator();

            // Test
            var actionOptionConditionBuilder = new ActionOptionConditionBuilder<Customer, string>(validator);
            ActionOptionBuilder<Customer, string> ifResult = actionOptionConditionBuilder.If(c => c.Name.Length > 10);


            // Assert
            Assert.That(validator.Condition, Is.Not.Null);
        }
    }
}