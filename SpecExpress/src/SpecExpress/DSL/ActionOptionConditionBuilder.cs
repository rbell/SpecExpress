using System;
using System.Linq.Expressions;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Facilitates the ability to place a condition on a property rule.
    /// </summary>
    /// <example>
    /// Check(c => c.LastName).Required().If(c => c.ContactType == ContactType.Primary)
    /// </example>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class ActionOptionConditionBuilder<T, TProperty> : ActionOptionBuilder<T,TProperty>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOptionConditionBuilder&gt;T, TProperty&lt;" /> class.
        /// </summary>
        /// <param name="propertyValidator">The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is being build by the DSL.</param>
        public ActionOptionConditionBuilder(PropertyValidator<T, TProperty> propertyValidator)
            : base(propertyValidator)
        {
        }

        /// <summary>
        /// Defines the condition on which must be met before any defined rules for the property are
        /// enforced.
        /// </summary>
        /// <param name="conditionalExpression">An <see cref="Expression&lt;Predicate&lt;T&gt;&gt;>"/> that will determine when the rules should be enforced.</param>
        /// <returns><see cref="ActionOptionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionBuilder<T, TProperty> If(Expression<Predicate<T>> conditionalExpression)
        {
            _propertyValidator.Condition = conditionalExpression.Compile();
            return new ActionOptionBuilder<T, TProperty>(_propertyValidator);
        }

    }
}