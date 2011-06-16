using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace SpecExpress.Rules.Collection
{
    public class Contains<T, TProperty> : RuleValidator<T, TProperty> where TProperty:IEnumerable
    {
        private object _contains;

        public Contains(object contains)
        {
            _contains = contains;
        }

        public Contains(Expression<Func<T,object>> expression)
        {
            SetPropertyExpression(expression);
        }

        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() {{"", _contains }}; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            bool contains = false;

            if (PropertyExpressions.Any())
            {
                _contains = GetExpressionValue(context);
            }

            foreach (var value in context.PropertyValue)
            {
                if (value.Equals(_contains))
                {
                    contains = true;
                    break;
                }
            }

            return Evaluate(contains, context, notification);
        }
    }
}