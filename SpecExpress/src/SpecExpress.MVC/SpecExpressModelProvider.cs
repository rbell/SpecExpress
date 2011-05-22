using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SpecExpress.MVC
{
    public class SpecExpressModelProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            bool catalogCanValidate =
                (from specification in ValidationCatalog.SpecificationContainer.GetAllSpecifications()
                 where specification.DefaultForType == true && specification.ForType == metadata.ContainerType &&
                 specification.PropertyValidators.Exists(v => v.PropertyName == metadata.PropertyName)
                 select specification).Any();

            var validators = new List<ModelValidator>();
            if (catalogCanValidate)
            {
                validators.Add(new SpecExpressModelValidator(metadata, context));
            }

            return validators;
        }

    }
}
