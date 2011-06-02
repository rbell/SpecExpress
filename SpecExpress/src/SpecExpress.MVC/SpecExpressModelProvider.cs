using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SpecExpress.MVC
{
    public class SpecExpressModelProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            var validators = new List<ModelValidator>();

            var spec = ValidationCatalog.SpecificationContainer.TryGetSpecification(metadata.ContainerType);
            if (spec != null)
            {
                var propertyValidator = spec.PropertyValidators.FirstOrDefault(v => v.PropertyName == metadata.PropertyName);

                if (propertyValidator != null)
                {
                    validators.Add(new SpecExpressModelPropertyValidator(metadata, context, propertyValidator));
                }
            }
            

            //bool catalogCanValidate =
            //    (from specification in ValidationCatalog.SpecificationContainer.GetAllSpecifications()
            //     where specification.DefaultForType == true && specification.ForType == metadata.ContainerType &&
            //     specification.PropertyValidators.Exists(v => v.PropertyName == metadata.PropertyName)
            //     select specification).Any();

           
          
            return validators;
        }

    }
}
