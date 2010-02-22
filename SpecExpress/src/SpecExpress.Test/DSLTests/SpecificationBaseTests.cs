using NUnit.Framework;
using SpecExpress.DSL;
using SpecExpress.Enums;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.DSLTests
{
    /// <summary>
    /// Test Fixture for the Validates which confirms that it modifies the PropertyValidator appropriatly and
    /// returns the next appropriate builder in the DSL.
    /// </summary>
    [TestFixture]
    public class ValidatesTests : Validates<Customer>
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            PropertyValidators.Clear();
        }

        #endregion

        [Test]
        public void Check_Name_RegistersPropertyValidatorWithErrorLevel_ReturnsActionOptionBuilder()
        {
            ActionOptionBuilder<Customer, string> checkReturnObj = Check(C => C.Name);

            Assert.That(PropertyValidators, Is.Not.Empty);
            Assert.That(PropertyValidators[0],Is.InstanceOf<PropertyValidator<Customer, string>>());
            Assert.That(PropertyValidators[0].Level, Is.EqualTo(ValidationLevelType.Error));
            Assert.That(checkReturnObj,Is.InstanceOf<ActionOptionBuilder<Customer, string>>());
        }

        [Test]
        public void Check_NameAndMessage_RegistersPropertyValidatorWithErrorLevel_ReturnsActionOptionBuilder()
        {
            ActionOptionBuilder<Customer, string> checkReturnObj = Check(C => C.Name, "Formal Name");

            Assert.That(PropertyValidators, Is.Not.Empty);
            Assert.That(PropertyValidators[0], Is.InstanceOf<PropertyValidator<Customer, string>>());
            Assert.That(PropertyValidators[0].Level, Is.EqualTo(ValidationLevelType.Error));
            Assert.That(PropertyValidators[0].PropertyNameOverride, Is.EqualTo("Formal Name"));
            Assert.That(checkReturnObj, Is.InstanceOf<ActionOptionBuilder<Customer, string>>());
        }


        [Test]
        public void Warn_Name_RegistersPropertyValidatorWithWarnLevel_ReturnsActionOptionBuilder()
        {
            ActionOptionBuilder<Customer, string> checkReturnObj = Warn(C => C.Name);

            Assert.That(PropertyValidators, Is.Not.Empty);
            Assert.That(PropertyValidators[0], Is.InstanceOf<PropertyValidator<Customer, string>>());
            Assert.That(PropertyValidators[0].Level, Is.EqualTo(ValidationLevelType.Warn));
            Assert.That(checkReturnObj, Is.InstanceOf<ActionOptionBuilder<Customer, string>>());
        }

        [Test]
        public void Warn_NameAndMessage_RegistersPropertyValidatorWithWarnLevel_ReturnsActionOptionBuilder()
        {
            ActionOptionBuilder<Customer, string> checkReturnObj = Warn(C => C.Name, "Formal Name");

            Assert.That(PropertyValidators, Is.Not.Empty);
            Assert.That(PropertyValidators[0], Is.InstanceOf<PropertyValidator<Customer, string>>());
            Assert.That(PropertyValidators[0].Level, Is.EqualTo(ValidationLevelType.Warn));
            Assert.That(PropertyValidators[0].PropertyNameOverride, Is.EqualTo("Formal Name"));
            Assert.That(checkReturnObj, Is.InstanceOf<ActionOptionBuilder<Customer, string>>());            
        }
    }
}