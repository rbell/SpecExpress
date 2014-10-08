using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SpecExpress.Rules.GeneralValidators
{
    public class SpecificationRule<T, TProperty, TSpecification> : SpecificationRule<T, TProperty> where TSpecification : Validates<TProperty>, new()
    {
        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            SpecificationBase = specificationContainer.TryGetSpecification<TSpecification>() as Validates<TProperty> ??
                     new TSpecification();

            return base.Validate(context, specificationContainer, notification);
        }
    }

    public class SpecificationRule<T, TProperty> : RuleValidator<T, TProperty>
    {
        protected SpecificationBase SpecificationBase;

        /// <summary>
        /// Validate using designated specification
        /// </summary>
        /// <param name="specificationBase"></param>
        public SpecificationRule(Validates<TProperty> specificationBase) 
        {
            SpecificationBase = specificationBase;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public SpecificationRule()
        {
           
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            SpecificationBase specificationBase;

            if (SpecificationBase == null)
            {
                //Spec not defined, so get from the container, ie .Specification()
                specificationBase = specificationContainer.GetSpecification(context.PropertyValue.GetType());
            }
            else
            {
                //Specification explicity defined by DSL .Specification<SomeSpecification>()
                specificationBase = SpecificationBase;
            }

            var innerNotification = new ValidationNotification();
            foreach (var validator in specificationBase.PropertyValidators)
            {
                validator.Validate(context.PropertyValue, context, specificationContainer, innerNotification);
            }

            ValidationResult result = null;

            if (innerNotification.Errors.Any())
            {   
                result = ValidationResultFactory.Create(this, context, new List<object>(), MessageKey, innerNotification.Errors);
                //result.NestedValidationResults = innerNotification.Errors;
                notification.Errors.Add(result);
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
