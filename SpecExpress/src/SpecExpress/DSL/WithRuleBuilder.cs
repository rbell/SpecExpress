using System;
using System.Linq;
using SpecExpress.Rules;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Facilitates the ability to stipulate additional options for a rule.
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class WithRuleBuilder<T, TProperty> : RuleBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="WithRuleBuilder&lt;T, TProperty&gt;"/>
        /// </summary>
        /// <param name="propertyValidator">The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is being build by the DSL.</param>
        public WithRuleBuilder(PropertyValidator<T, TProperty> propertyValidator)
            : base(propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        /// <summary>
        /// Allows the stipulation of additional options to be applied to the prior rule.
        /// </summary>
        /// <param name="w"><see cref="Action<WithBuilder&lt&ltT, TProperty&gt&gt"/></param>
        /// <returns><see cref="RuleBuilder&ltT, TProperty&gt"/></returns>
        public RuleBuilder<T, TProperty> With(Action<WithBuilder<T, TProperty>> w)
        {
            w(new WithBuilder<T, TProperty>(_propertyValidator));
            return new RuleBuilder<T,TProperty>(_propertyValidator);
        }

    }
}