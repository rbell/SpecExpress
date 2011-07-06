using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Rules.StringValidators;
using SpecExpress.RuleTree;

namespace SpecExpress.MVC
{
    public class SpecExpressClientRuleFactory
    {
        private ModelMetadata ModelMetaData;

        public IEnumerable<ModelClientValidationRule> Create(PropertyValidator propertyValidator, ModelMetadata modelMetadata)
        {
            ModelMetaData = modelMetadata;

            var clientRules = new List<ModelClientValidationRule>();

            if (propertyValidator.RuleTree.Root is RuleNode)
            {

                var rules = GetAllRuleValidatorsForProperty(propertyValidator.RuleTree.Root as RuleNode);

                //Map RuleValidator to Client Rules
                foreach (var ruleValidator in rules)
                {
                    var clientRule = RuleValidatorClientRuleRegistry.Instance.Create(ruleValidator);

                    //If a client rule isn't found for the RuleValidator then NULL is returned
                    if (clientRule != null)
                    {
                        clientRule.ErrorMessage = FormatErrorMessageTemplateWithName(clientRule.ErrorMessage);
                        clientRules.Add(clientRule);
                    }
                }
            }

            return clientRules;
        }


        private List<RuleValidator> GetAllRuleValidatorsForProperty(RuleNode node)
        {
            //TODO: Don't add anything with stuff we can't handle (OR,GROUP)
            var rules = new List<RuleValidator>();
            rules.Add(node.Rule);

            var childRuleNode = node.ChildNode as RuleNode;

            if (childRuleNode != null)
            {
                //rules.Add(childRuleNode.Rule);
                rules.AddRange(GetAllRuleValidatorsForProperty(childRuleNode));
            }

            return rules;
        }
        
        private string FormatErrorMessageTemplateWithName(string errorMessageTemplate)
        {
            return errorMessageTemplate.Replace("{PropertyName}", ModelMetaData.GetDisplayName());
        }
    }
}
