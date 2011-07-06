using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SpecExpress.MessageStore;

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
            
            foreach (var validationResult in vn.Errors)
            {
                //Don't set the display name because it won't bind on the view if you do
                yield return new ModelValidationResult() { Message = validationResult.Message };
            }
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var clientRulesFactory = new SpecExpressClientRuleFactory();
            return clientRulesFactory.Create(PropertyValidator, Metadata);
        }
    }
}