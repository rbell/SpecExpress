namespace SpecExpress.DSL
{
    /// <summary>
    /// Facilitates the requirement rule on a given property - "Required" or "Optional"
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class ActionOptionBuilder<T, TProperty>
    {
        protected readonly PropertyValidator<T, TProperty> _propertyValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="ActionOptionBuilder&lt;T, TProperty&gt;"/>
        /// </summary>
        /// <param name="propertyValidator">The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is being build by the DSL.</param>
        public ActionOptionBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        /// <summary>
        /// Mark property as required, creating a ValidationResult if this property has no value. 
        /// Additional rules will only be evaluated if this rule is valid.
        /// </summary>
        /// <returns><see cref="WithRuleBuilder&lt;T, TProperty&gt;"/></returns>
        public WithRuleBuilder<T, TProperty> Required()
        {
            _propertyValidator.PropertyValueRequired = true;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Mark property as required, creating a ValidationResult if this property has no value.
        /// Additional rules will only be evaluated if this rule is valid.
        /// </summary>
        /// <param name="errorMessage">Custom error message if property fails the Required rule</param>
        /// <returns><see cref="WithRuleBuilder&lt;T, TProperty&gt;"/></returns>
        public WithRuleBuilder<T, TProperty> Required(string errorMessage)
        {
            _propertyValidator.PropertyValueRequired = true;
            _propertyValidator.RequiredRule.Message = errorMessage;

            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Mark property as optional. If a value of the property is specified, additional rules will be enforced,
        /// otherwise additional rules will be ignored.
        /// </summary>
        /// <returns><see cref="WithRuleBuilder&lt;T, TProperty&gt;"/></returns>
        public WithRuleBuilder<T, TProperty> Optional()
        {
            _propertyValidator.PropertyValueRequired = false;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }
    }
}