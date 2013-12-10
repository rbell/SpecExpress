using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SpecExpress.DSL;
using SpecExpress.Rules;
using SpecExpress.Util;


namespace SpecExpress.MessageStore
{
    public class MessageService
    {
        public string GetDefaultMessageAndFormat(MessageContext context, IList<RuleParameter> parameterValues)
        {
            string messageTemplate = GetMessageTemplate(context);
          
            return FormatMessage(messageTemplate, context.RuleContext, parameterValues, context.PropertyValueFormatter);
        }

        public string GetMessageTemplate(MessageContext context)
        {
           
            var ruleKey =  context.Key.IsNullOrDefault() ? BuildRuleKeyFromContext(context) : context.Key.ToString();

            IMessageStore messageStore;

            if (!String.IsNullOrEmpty(context.MessageStoreName))
            {
                //Explicit request for this Message Store
                messageStore = MessageStoreFactory.GetMessageStore(context.MessageStoreName);
            }
            else
            {
                //Message Store not defined. Look for overrides first, then fall back to default if not found
                //Select the last message store found
                messageStore = MessageStoreFactory.GetAllMessageStores().ToList().Last(
                    x => x.IsMessageInStore(ruleKey));
            }

            var messageTemplate = messageStore.GetMessageTemplate(ruleKey);
            return messageTemplate;
        }


        public string FormatMessage(string message, RuleValidatorContext context, IList<RuleParameter> ruleParameters)
        {
            return FormatMessage(message, context, ruleParameters, null);
        }

        public string FormatMessage(string message, RuleValidatorContext context, IList<RuleParameter> ruleParameters, Func<object, string> propertyValueFormatter)
        {
            //Replace known keywords with actual values

            //PropertyName
            var formattedMessage = message.Replace("{PropertyName}", buildPropertyName(context));

            //PropertyValue
            if (context.PropertyValue == null)
            {
                formattedMessage = formattedMessage.Replace("{PropertyValue}", context.PropertyValue as string);                
            }
            else
            {
                string formattedPropertyValue;

                if (propertyValueFormatter == null)
                {
                    formattedPropertyValue = context.PropertyValue.ToString();
                }
                else
                {
                    formattedPropertyValue = propertyValueFormatter(context.PropertyValue);
                }
                formattedMessage = formattedMessage.Replace("{PropertyValue}", formattedPropertyValue);
            }

            //Parameters
            if (ruleParameters != null)
            {
                //Named Parameters
                var namedParameters = ruleParameters.Where(p => !String.IsNullOrEmpty(p.PropertyName));

                //Replace {PropertyName} with the Value
                foreach (var namedParameter in namedParameters)
                {
                    formattedMessage = formattedMessage.Replace("{" + namedParameter.PropertyName + "}", namedParameter.GetParamValue2(context).ToString());
                }

                //Unnamed Parameters
                var unnamedParameterValues = ruleParameters.Where(p => String.IsNullOrEmpty(p.PropertyName)).Select( p => p.GetParamValue2(context));
                
                if (unnamedParameterValues.Any())
                {
                    formattedMessage = String.Format(formattedMessage, unnamedParameterValues.ToStringEnum().ToArray());
                }
                
            }

            return formattedMessage;
        }

        public string BuildRuleKeyFromContext(MessageContext context)
        {
            //Use Name of the Rule Validator as the Key to get the error message format string
            //RuleValidator types have Generics which return Type Name as LengthValidator`1 and we need to remove that
            string key = context.ValidatorType.Name.Split('`').FirstOrDefault();

            // Remove "Nullable" from end of type name
            if (key.EndsWith("Nullable"))
            {
                key = key.Remove(key.Length - 8);
            }

            // Prefix key with "Not_" for negated rule messages
            if (context.Negate)
            {
                key = "Not_" + key;
            }

            return key;
        }

        private string buildPropertyName(RuleValidatorContext context)
        {
            //Build a string with the graph of property names
            var propertyNameNodes = new List<string>();

            RuleValidatorContext currentContext = context;
            do
            {
                propertyNameNodes.Add(currentContext.PropertyName.SplitPascalCase());
                currentContext = currentContext.Parent;
            } while (currentContext != null);


            //Reverse by putting the top level first
            propertyNameNodes.Reverse();

            //create a string containing the heirarchy flattened out 
            var propertyNameForNestedProperty = new StringBuilder();
            //add a space between nodes
            propertyNameNodes.ForEach(p => propertyNameForNestedProperty.AppendFormat(" {0}", p));

            return propertyNameForNestedProperty.ToString().Trim();
        }
    }
}
