using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SpecExpress.Rules.Collection
{
    public class IsEmpty<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() { }; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            int count = 0;

            if (context.PropertyValue != null)
            {
                foreach (var value in context.PropertyValue)
                {
                    count++;
                    break;
                }
            }

            return Evaluate(count == 0, context, notification);
        }
    }
}