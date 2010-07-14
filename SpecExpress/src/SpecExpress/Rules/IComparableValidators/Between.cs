using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpecExpress.Rules.IComparableValidators
{
    public class Between<T, TProperty> : RuleValidator<T, TProperty>
    {
        private TProperty _floor;
        private TProperty _ceiling;

        public Between(TProperty floor, TProperty ceiling)
        {
            _floor = floor;
            _ceiling = ceiling;
        }

        public Between(Expression<Func<T, TProperty>> floor, TProperty ceiling)
        {
            SetPropertyExpression("floor", floor);
            _ceiling = ceiling;
        }

        public Between(TProperty floor, Expression<Func<T, TProperty>> ceiling)
        {
            _floor = floor;
            SetPropertyExpression("ceiling", ceiling);
        }

        public Between(Expression<Func<T, TProperty>> floor, Expression<Func<T, TProperty>> ceiling)
        {
            SetPropertyExpression("floor", floor);
            SetPropertyExpression("ceiling",ceiling);
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (PropertyExpressions.ContainsKey("floor"))
            {
                _floor = (TProperty)GetExpressionValue("floor", context);
            }

            if (PropertyExpressions.ContainsKey("ceiling"))
            {
                _ceiling = (TProperty)GetExpressionValue("ceiling", context);
            }

            Comparer<TProperty> comparer = System.Collections.Generic.Comparer<TProperty>.Default;
           
            return Evaluate(comparer.Compare(context.PropertyValue, _ceiling) <= 0 && comparer.Compare(context.PropertyValue, _floor) >= 0 , context, notification);
        }

        public override object[] Parameters
        {
            get { return new object[] {_floor, _ceiling}; }
        }
    }
}