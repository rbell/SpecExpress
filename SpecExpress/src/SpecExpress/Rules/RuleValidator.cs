using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using SpecExpress.DSL;
using SpecExpress.MessageStore;

namespace SpecExpress.Rules
{
    public abstract class RuleValidator
    {
        public string Message { get; set; }
        public Func<object, string> MessageFormatter { get; set; }
        public object MessageKey { get; set; }
        public string MessageStoreName { get; set; }
        public bool Negate { get; set; }

        public virtual IList<RuleParameter> Params { get; private set; }

        protected RuleValidator()
        {
            Params = new List<RuleParameter>();
        }

        public string ErrorMessageTemplate
        {
            get
            {
                string message = string.Empty;
                var messageService = new MessageService();

                if (String.IsNullOrEmpty(Message))
                {
                    var messageContext = new MessageContext(null, this.GetType(), Negate, MessageStoreName, MessageKey);
                    message = messageService.GetMessageTemplate(messageContext);
                }
                else
                {
                    //Since the message was supplied, don't get the default message from the store, just return it
                    message = Message;
                }

                return message;
            }
        }
    }

    public abstract class RuleValidator<T, TProperty> : RuleValidator
    {
        /// <summary>
        /// Executes a Delegate and casts to the return value to the appropriate type
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected object GetExpressionValue(CompiledExpression expression, RuleValidatorContext<T, TProperty> context)
        {
            try
            {
                return expression.Invoke(context.Instance);
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected bool Evaluate(bool isValid, RuleValidatorContext<T, TProperty> context, ValidationNotification notification)
        {
            var paramValues =
                (from ruleParameter in Params select (object) ruleParameter.GetParamValue(context)).ToList();

            if (Negate)
            {
                if (!isValid)
                {
                    return true;
                }
                else
                {
                    notification.Errors.Add(ValidationResultFactory.Create(this, context, paramValues, MessageKey));
                    return false;
                }
            }
            else
            {
                if (isValid)
                {
                    return true;
                }
                else
                {
                    notification.Errors.Add(ValidationResultFactory.Create(this, context, paramValues, MessageKey));
                    return false;
                }
            }
        }

        public abstract bool Validate(RuleValidatorContext<T, TProperty> context,
                                      ISpecificationContainer specificationContainer, ValidationNotification notification);
    }
}
