using System;
using System.Linq.Expressions;

namespace SpecExpress.DSL
{
    public class ActionOptionConditionBuilder<T, TProperty> : ActionOptionBuilder<T,TProperty>
    {
        public ActionOptionConditionBuilder(PropertyValidator<T, TProperty> propertyValidator) : base(propertyValidator)
        {
        }

        //public RuleBuilder<T, TProperty> And
        //{
        //    get { return new RuleBuilder<T, TProperty>(_propertyValidator); }
        //}

        //public WithBuilder<T, TProperty> With
        //{
        //    get { return new WithBuilder<T, TProperty>(_propertyValidator); }
        //}

        //public ActionOptionConditionSatisfiedBuilder<T, TProperty> If(Expression<Predicate<T>> conditionalExpression)
        //{
        //    _propertyValidator.Condition = conditionalExpression.Compile();
        //    return new ActionOptionConditionSatisfiedBuilder<T, TProperty>(_propertyValidator);
        //}

        //public ActionOptionConditionSatisfiedBuilder<T, TProperty> If(Expression<Predicate<T>> conditionalExpression, string errorMessage)
        //{
        //    _propertyValidator.Condition = conditionalExpression.Compile();

        //    return new ActionOptionConditionSatisfiedBuilder<T, TProperty>(_propertyValidator);
        //}

        public ActionOptionBuilder<T, TProperty> If(Expression<Predicate<T>> conditionalExpression)
        {
            _propertyValidator.Condition = conditionalExpression.Compile();
            return new ActionOptionBuilder<T, TProperty>(_propertyValidator);
        }

        public ActionOptionBuilder<T, TProperty> If(Expression<Predicate<T>> conditionalExpression, string errorMessage)
        {
            _propertyValidator.Condition = conditionalExpression.Compile();

            return new ActionOptionBuilder<T, TProperty>(_propertyValidator);
        }

        //ActionOptionBuilder


    }
}