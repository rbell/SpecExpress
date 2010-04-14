using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SpecExpress.Util;

namespace SpecExpress.Rules.GeneralValidators
{
    public class ForEachSpecificationRule<T, TProperty, TCollectionType> : RuleValidator<T, TProperty> 
    {
        private string _itemName;

        private Validates<TCollectionType> _specification;
        public override object[] Parameters
        {
            get { return new object[] { }; }
        }

        /// <summary>
        /// Validate using designated specification
        /// </summary>
        /// <param name="specification"></param>
        public ForEachSpecificationRule(Validates<TCollectionType> specification)
        {
            _specification = specification;
        }

        public ForEachSpecificationRule(Validates<TCollectionType> specification, string itemName) : this(specification)
        {
            _itemName = itemName;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public ForEachSpecificationRule()
        {
        }

        public ForEachSpecificationRule(string itemName): this()
        {
            _itemName = itemName;
        }

       

        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {
            if (_specification == null)
            {
                _specification = specificationContainer.GetSpecification<TCollectionType>();
            }

            ValidationResult collectionValidationResult = null;

            //Check if the Collection is null/default
            if (context.PropertyValue.IsNullOrDefault())
            {
                return collectionValidationResult;
            }
            
            var itemsNestedValidationResult = new List<ValidationResult>();


            var propertyEnumerable = ( (IEnumerable)(context.PropertyValue));

            if (propertyEnumerable == null)
            {
                throw new ArgumentException("Property must be IEnumerable");
            }

            int index = 1;
            foreach (var item in propertyEnumerable)
            {  
                var itemErrors = _specification.Validate(item, specificationContainer);
                if (itemErrors.Any())
                {
                    var propertyName = String.IsNullOrEmpty(_itemName) ? item.GetType().Name : _itemName;

                    Message = String.Format("{0} {1} in {2} is invalid.", propertyName, index, context.PropertyName);

                    var childContext = new RuleValidatorContext(item, propertyName, item, context.Level,
                                                                item.GetType() as MemberInfo, context);

                    var itemError = ValidationResultFactory.Create(this, childContext, Parameters, MessageKey);

                    //var itemError = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                    itemError.NestedValidationResults = itemErrors;
                    itemsNestedValidationResult.Add(itemError);
                }
                index++;
            }

            if (itemsNestedValidationResult.Any())
            {
                //Errors were found on at least one item in the collection to return a ValidationResult for the Collection property
                Message = "{PropertyName} is invalid.";
                collectionValidationResult = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                collectionValidationResult.NestedValidationResults = itemsNestedValidationResult;
            }

            return collectionValidationResult;

            //if (sb.Length > 0)
            //{
            //    listValidationResult = ValidationResultFactory.Create(this, context, Parameters, "{PropertyName} is invalid.", MessageStoreName, MessageKey);
            //    listValidationResult.NestedValidationResults = list;
            //}
            //else
            //{
            //    return null;
            //}
        }

        //private string CreateErrorMessage(object value)
        //{
        //    string message = _errorMessageTemplate;
        //    Type valueType = value.GetType();
        //    var valueProperties = valueType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (var property in valueProperties)
        //    {
        //        string propertySearchString = "{" + property.Name + "}";
        //        if (message.Contains(propertySearchString))
        //        {
        //            message = message.Replace(propertySearchString, property.GetValue(value, null).ToString());
        //        }
        //    }

        //    return message;
        //}
    }
}
