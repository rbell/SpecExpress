using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SpecExpress.MVC
{
    public class SpecExpressClientRuleFactory
    {

        public IEnumerable<ModelClientValidationRule> Create(PropertyValidator propertyValidator, ModelMetadata modelMetadata)
        {
            var clientRules = new List<ModelClientValidationRule>();

            //TODO: iterate through each Rule in the PropertyValidator and define client side rule
            //TODO: Missing way to get all the rules, besides Required
            if (propertyValidator.PropertyValueRequired)
            {
                //TODO: Format message clientside because we don't have the property value here
                var rule = new ModelClientValidationRule()
                {
                    ErrorMessage =
                        FormatErrorMessageTemplateWithName(
                            propertyValidator.RequiredRule.ErrorMessageTemplate, modelMetadata),
                    ValidationType = "specrequired"
                };

                clientRules.Add(rule);

            }

            return clientRules;
        }

        private string FormatErrorMessageTemplateWithName(string errorMessageTemplate, ModelMetadata modelMetadata)
        {
            return errorMessageTemplate.Replace("{PropertyName}", modelMetadata.GetDisplayName());
        }
    }
}
