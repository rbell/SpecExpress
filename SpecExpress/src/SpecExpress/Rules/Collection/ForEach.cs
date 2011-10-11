using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using SpecExpress.MessageStore;
using SpecExpress.Util;

namespace SpecExpress.Rules.Collection
{
    public class ForEach<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private Predicate<object> _forEachPredicate;
        private string _errorMessageTemplate;

        public ForEach(Predicate<object> forEachPredicate, string errorMessageTemplate)
        {
            _forEachPredicate = forEachPredicate;
            _errorMessageTemplate = errorMessageTemplate;
        }


        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            StringBuilder sb = new StringBuilder();

            if (context.PropertyValue.IsNullOrDefault() )
            {
                return true;
            }

            foreach (var value in context.PropertyValue)
            {
                if (!_forEachPredicate(value))
                {
                    sb.AppendLine(CreateErrorMessage(value));
                }
            }

            if (sb.Length > 0)
            {
                Message = sb.ToString();
                notification.Errors.Add(ValidationResultFactory.Create(this, context, null, null));
                return false;                            
            }
            else
            {
                return true;
            }
        }

        private string CreateErrorMessage(object value)
        {
            string message = _errorMessageTemplate;
            Type valueType = value.GetType();
            var valueProperties = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in valueProperties)
            {
                string propertySearchString = "{" + property.Name + "}";
                if (message.Contains(propertySearchString))
                {
                    message = message.Replace(propertySearchString, property.GetValue(value, null).ToString());
                }
            }

            return message;
        }
    }
}