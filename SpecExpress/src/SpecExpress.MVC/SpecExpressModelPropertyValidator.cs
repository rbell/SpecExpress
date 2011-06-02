using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SpecExpress.MVC
{
    public class SpecExpressModelPropertyValidator : ModelValidator
    {
        protected PropertyValidator PropertyValidator { get; set; }

        public SpecExpressModelPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyValidator propertyValidator)
            : base(metadata, controllerContext)
        {
            PropertyValidator = propertyValidator;
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var vn = ValidationCatalog.ValidateProperty(container, Metadata.PropertyName);
            return vn.All().Select(
                v =>
                new ModelValidationResult() {Message = v.ToString(), MemberName = v.Property.Name});
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
           var clientRules = new List<ModelClientValidationRule>();

            if (PropertyValidator.PropertyValueRequired)
            {
                //TODO: Format message clientside because we don't have the property value here
                clientRules.Add(new ModelClientValidationRequiredRule(PropertyValidator.RequiredRule.ErrorMessageTemplate));
            }

            return clientRules;
        }

    }
}