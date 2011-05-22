using System.Collections.Generic;
using System.Web.Mvc;

namespace SpecExpress.MVC
{
    public class SpecExpressModelValidator : ModelValidator
    {
        public SpecExpressModelValidator(ModelMetadata metadata, ControllerContext controllerContext)
            : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var results = ValidationCatalog.ValidateProperty(container, Metadata.PropertyName);

            return new List<ModelValidationResult> { new ModelValidationResult() { Message = results.ToString() } };
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new List<ModelClientValidationRule>()
                       {
                           new ModelClientValidationRule()
                               {
                                   ErrorMessage = "Required!",
                                   ValidationType = "title"
                               }
                       };
        }

    }
}