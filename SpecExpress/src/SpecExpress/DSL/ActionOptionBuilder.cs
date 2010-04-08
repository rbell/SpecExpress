namespace SpecExpress.DSL
{
    /// <summary>
    /// Changes:
    ///     Removed ValidationLevelType from constructor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class ActionOptionBuilder<T, TProperty>
    {
        protected readonly PropertyValidator<T, TProperty> _propertyValidator;

        public ActionOptionBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        /// <summary>
        /// Mark property as required, creating a ValidationResult if this property has no value. 
        /// </summary>
        /// <returns></returns>
        public WithRuleBuilder<T, TProperty> Required()
        {
            _propertyValidator.PropertyValueRequired = true;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Mark property as required, creating a ValidationResult if this property has no value. Additional rules will only be evaluated if a value is found.
        /// </summary>
        /// <param name="errorMessage">Custom error message if property fails the Required rule</param>
        /// <returns></returns>
        public WithRuleBuilder<T, TProperty> Required(string errorMessage)
        {
            _propertyValidator.PropertyValueRequired = true;
            _propertyValidator.RequiredRule.Message = errorMessage;

            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Mark property as optional. If a value is found, rules will be run, otherwise they will be ignored.
        /// </summary>
        /// <returns></returns>
        public WithRuleBuilder<T, TProperty> Optional()
        {
            _propertyValidator.PropertyValueRequired = false;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }
    }
}