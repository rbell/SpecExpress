using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using SpecExpress.MVC.RuleRegistrations;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress.MVC
{
    /// <summary>
    /// A container for all SpecExpress to Client Side Rule mappings.
    /// </summary>
    public sealed class RuleValidatorClientRuleRegistry
    {
        static readonly RuleValidatorClientRuleRegistry instance = new RuleValidatorClientRuleRegistry();
        static Dictionary<Type, RuleValidatorClientRuleMap> Mapping;
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static RuleValidatorClientRuleRegistry()
        {
        }

        RuleValidatorClientRuleRegistry()
        {
            Mapping = new Dictionary<Type, RuleValidatorClientRuleMap>();

            // TODO: Refactor to allow user to specify additional assemblies to scan for RuleRegistrations

            var thisAssembly = this.GetType().Assembly;

            var registrationTypes = from type in thisAssembly.GetTypes()
                                where type.BaseType == typeof (RuleRegistration)
                                select type;

            foreach (var registrationType in registrationTypes)
            {
                var registration = Activator.CreateInstance(registrationType) as RuleRegistration;
                Mapping.Add(registration.RuleType, registration.ClientRuleMap);
            }
            
        }

        public static RuleValidatorClientRuleRegistry Instance
        {
            get { return instance; }
        }

        public ModelClientValidationRule Create(RuleValidator ruleValidator)
        {
            var clientRule = new ModelClientValidationRule();

            var ruleValidatorType = ruleValidator.GetType().GetGenericTypeDefinition();
            if (!Mapping.ContainsKey(ruleValidatorType))
            {
                return null;
            }

            var rule = Mapping[ruleValidatorType];

            clientRule.ValidationType = rule.JQueryRuleName;
            clientRule.ErrorMessage = ruleValidator.ErrorMessageTemplate;

            //map all the parameters
            foreach (var parameter in rule.Parameters)
            {
                if (ruleValidator.PropertyExpressions.ContainsKey(parameter.Value))
                {
                    // parameter.value is index of the matching expression in the rulevalidator PropertyExpressions collection
                    // TODO: Handle parameters defined as an expression
                    var expression = ruleValidator.PropertyExpressions[parameter.Value].Expression;
                    if (expression.Body.NodeType == ExpressionType.MemberAccess)
                    {
                        var propertyName = ((MemberExpression) expression.Body).Member.Name;
                        clientRule.ValidationParameters.Add(parameter.Key,
                                                            new PropertyExpressionParam() {PropertyName = propertyName});
                    }
                }
                else
                {
                    //parameter.value is the index of the matching value in the rulevalidator parameters collection
                    clientRule.ValidationParameters.Add(parameter.Key, ruleValidator.Parameters[parameter.Value]);
                }
            }

            return clientRule;
        }

        public class PropertyExpressionParam
        {
            public PropertyExpressionParam()
            {
                IsProperty = true;
            }

            public bool IsProperty { get; private set; }

            public string PropertyName { get; set; }

            public override string ToString()
            {
                return "{\"isProperty\":\"true\",\"propertyName\":\"" + PropertyName + "\"}";
            }
        }

    }

}
