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
    public class ForEachSpecificationRule<T, TProperty, TCollectionType, TSpecification>  : ForEachSpecificationRule<T, TProperty, TCollectionType>  where TSpecification : Validates<TCollectionType>, new() 
    {
        public ForEachSpecificationRule()
        {
            
        }

        public ForEachSpecificationRule(string itemName) :base(itemName)
        {
            
        }
        
        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            //Resolve the Specification
            Specification = specificationContainer.TryGetSpecification<TSpecification>() as Validates<TCollectionType>  ??
                       new TSpecification();

            return base.Validate(context, specificationContainer, notification);
        } 
        
    }

    public class ForEachSpecificationRule<T, TProperty, TCollectionType> : RuleValidator<T, TProperty> 
    {
        protected string ItemName;

        protected Validates<TCollectionType> Specification;
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
            Specification = specification;
        }

        public ForEachSpecificationRule(Validates<TCollectionType> specification, string itemName) : this(specification)
        {
            ItemName = itemName;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public ForEachSpecificationRule()
        {
        }

        public ForEachSpecificationRule(string itemName): this()
        {
            ItemName = itemName;
        }



        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (Specification == null)
            {
                Specification = specificationContainer.GetSpecification<TCollectionType>();
            }

            ValidationResult collectionValidationResult = null;

            //Check if the Collection is null/default
            if (context.PropertyValue.IsNullOrDefault())
            {
                return true;
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
                var innerNotfication = new ValidationNotification();
                if (!Specification.Validate(item, specificationContainer, innerNotfication))
                {
                    var propertyName = String.IsNullOrEmpty(ItemName) ? item.GetType().Name : ItemName;

                    Message = String.Format("{0} {1} in {2} is invalid.", propertyName, index, context.PropertyName);

                    var childContext = new RuleValidatorContext(item, propertyName, item, context.Level,
                                                                item.GetType() as MemberInfo, context);

                    var itemError = ValidationResultFactory.Create(this, childContext, Parameters, MessageKey);

                    //var itemError = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                    itemError.NestedValidationResults = innerNotfication.Errors;
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
                notification.Errors.Add(collectionValidationResult);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
