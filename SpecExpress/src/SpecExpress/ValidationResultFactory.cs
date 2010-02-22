using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.MessageStore;
using SpecExpress.Rules;

namespace SpecExpress
{
    internal static class ValidationResultFactory
    {
        public static ValidationResult Create(RuleValidator validator, RuleValidatorContext context, object[] parameters, object messageKey)
        {
            string message = string.Empty;
            var messageService = new MessageService();

            
            if (String.IsNullOrEmpty(validator.Message))
            {
                var messageContext = new MessageContext(context, validator.GetType(), validator.Negate, validator.MessageStoreName, messageKey, validator.MessageFormatter);
                message = messageService.GetDefaultMessageAndFormat(messageContext, parameters);
            }
            else
            {
                //Since the message was supplied, don't get the default message from the store, just format it
                message = messageService.FormatMessage(validator.Message, context, parameters, validator.MessageFormatter);
            }
           
            return new ValidationResult(context.PropertyInfo, message, context.PropertyValue);
        }
    }
}
