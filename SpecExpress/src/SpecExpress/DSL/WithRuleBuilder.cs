using System;
using System.Linq;
using SpecExpress.Rules;

namespace SpecExpress.DSL
{
    public class WithRuleBuilder<T, TProperty> : RuleBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        public WithRuleBuilder(PropertyValidator<T, TProperty> propertyValidator)
            : base(propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        public RuleBuilder<T, TProperty> With(Action<WithBuilder<T, TProperty>> w)
        {
            w(new WithBuilder<T, TProperty>(_propertyValidator));
            return new RuleBuilder<T,TProperty>(_propertyValidator);
        }

    }
}