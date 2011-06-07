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
            Specification = specificationContainer.TryGetSpecification<TSpecification>() as Validates<TProperty> ??
                     new TSpecification();

            return base.Validate(context, specificationContainer, notification);
        }
    }

    public class SpecificationRule<T, TProperty> : RuleValidator<T, TProperty>
    {
        protected Specification Specification;
        public override OrderedDictionary Parameters
        {
            get { return new OrderedDictionary() { }; }
        }

        /// <summary>
        /// Validate using designated specification
        /// </summary>
        /// <param name="specification"></param>
        public SpecificationRule(Validates<TProperty> specification) 
        {
            Specification = specification;
        }

        /// <summary>
        /// Validation Property with default Specification from Registry
        /// </summary>
        public SpecificationRule()
        {
           
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            Specification specification;

            if (Specification == null)
            {
                //Spec not defined, so get from the container, ie .Specification()
                specification = specificationContainer.GetSpecification(context.PropertyValue.GetType());
            }
            else
            {
                //Specification explicity defined by DSL .Specification<SomeSpecification>()
                specification = Specification;
            }

            var innerNotification = new ValidationNotification();
            foreach (var validator in specification.PropertyValidators)
            {
                validator.Validate(context.PropertyValue, context, specificationContainer, innerNotification);
            }

            ValidationResult result = null;

            if (innerNotification.Errors.Any())
            {
                var parameters = new List<object>();
                foreach (var parameter in Parameters)
                {
                    parameters.Add(parameter);
                }
                result = ValidationResultFactory.Create(this, context, parameters.ToArray(), MessageKey);
                result.NestedValidationResults = innerNotification.Errors;
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
