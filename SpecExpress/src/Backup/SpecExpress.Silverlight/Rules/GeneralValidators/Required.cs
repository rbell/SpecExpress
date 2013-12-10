using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using SpecExpress.Util;

namespace SpecExpress.Rules.GeneralValidators
{
    public class Required<T, TProperty> : RuleValidator<T, TProperty>
    {
        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Evaluate(!context.PropertyValue.IsNullOrDefault(), context, notification);      
        }
    }


}