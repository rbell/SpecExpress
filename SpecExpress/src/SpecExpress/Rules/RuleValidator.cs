using System;
using System.Collections.Generic;
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
        public abstract object[] Parameters { get; }
       

        protected ValidationResult Evaluate(bool isValid, RuleValidatorContext context)
        {
            if (Negate)
            {
                if (!isValid)
                {
                    return null;
                }
                else
                {
                    return ValidationResultFactory.Create(this, context, Parameters, MessageKey);
                }
            }
            else
            {
                if (isValid)
                {
                    return null;
                }
                else
                {
                    return ValidationResultFactory.Create(this, context, Parameters,MessageKey);
                }
            }
        }

        protected IDictionary<string, CompiledExpression> PropertyExpressions = new Dictionary<string, CompiledExpression>();

        protected CompiledExpression SetPropertyExpression(LambdaExpression expression)
        {
            return SetPropertyExpression(string.Empty, expression);
        }

        protected CompiledExpression SetPropertyExpression(string key, LambdaExpression expression)
        {
            var compiledExpression = new CompiledExpression(expression);
            PropertyExpressions[key] = compiledExpression;
            return compiledExpression;
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
                return (TProperty)expression.Invoke(context.Instance);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Defaults to first PropertyExpression
        /// </summary>
        /// <param name="key"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected object GetExpressionValue(string key, RuleValidatorContext<T, TProperty> context)
        {
            return GetExpressionValue(PropertyExpressions[key], context);
        }

        protected object GetExpressionValue(RuleValidatorContext<T, TProperty> context)
        {
            return GetExpressionValue(PropertyExpressions.First().Value, context);
        }

        //protected List<CompiledFunctionExpression<T, TProperty>> PropertyExpressions = new List<CompiledFunctionExpression<T, TProperty>>();
        public abstract ValidationResult Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer);
    }
}
