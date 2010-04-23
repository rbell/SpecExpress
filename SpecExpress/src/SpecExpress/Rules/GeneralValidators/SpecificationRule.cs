using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Rules.GeneralValidators
{
    public class SpecificationRule<T, TProperty, TSpecification> : SpecificationRule<T, TProperty> where TSpecification : Validates<TProperty>, new()
    {
        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {
            Specification = specificationContainer.TryGetSpecification<TSpecification>() as Validates<TProperty> ??
                     new TSpecification();

            return base.Validate(context, specificationContainer);
        }
    }

    public class SpecificationRule<T, TProperty> : RuleValidator<T, TProperty>
    {
        protected Validates<TProperty> Specification;
        public override object[] Parameters
        {
            get { return new object[] { }; }
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

        public override ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer)
        {

            if (Specification == null)
            {
                Specification = specificationContainer.GetSpecification<TProperty>();
            }

            var list =  Specification.PropertyValidators.SelectMany(x => x.Validate(context.PropertyValue, context, specificationContainer)).ToList();
            ValidationResult result = null;

            if (list.Any())
            {
                result = ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                result.NestedValidationResults = list;
            }

            return result;
        }
    }
}
