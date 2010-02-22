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

        public WithRuleBuilder<T, TProperty> Required()
        {
            _propertyValidator.PropertyValueRequired = true;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        public WithRuleBuilder<T, TProperty> Required(string errorMessage)
        {
            _propertyValidator.PropertyValueRequired = true;
            _propertyValidator.RequiredRule.Message = errorMessage;

            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }

        public WithRuleBuilder<T, TProperty> Optional()
        {
            _propertyValidator.PropertyValueRequired = false;
            return new WithRuleBuilder<T, TProperty>(_propertyValidator);
        }
    }
}