using System.Collections;
using System.Linq;
using SpecExpress.MessageStore;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using System;

namespace SpecExpress.DSL
{
    public class WithBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        public WithBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        public string Message
        {
            get
            {
                //get message for last rule added
                RuleValidator rule = _propertyValidator.Rules.Last();
                return rule.Message;
            }
            set
            {
                //set message for last rule added
                RuleValidator rule = _propertyValidator.Rules.Last();
                rule.Message = value;
            }
        }

        public object MessageKey
        {
            get
            {
                //get error message for last rule added
                RuleValidator rule = _propertyValidator.Rules.Last();
                return rule.MessageKey;
            }
            set
            {
                //set error message for last rule added
                RuleValidator rule = _propertyValidator.Rules.Last();
                rule.MessageKey = value;
            }
        }

        public Func<TProperty, string> FormatProperty 
        {
            get
            {
                RuleValidator rule = _propertyValidator.Rules.Last();
                return rule.MessageFormatter as Func<TProperty, string>;
            }
            set
            {
                //RuleValidator rule = _propertyValidator.Rules.Last();
               
                // Func<TProperty, string> func = value;
                //var s = func() ;

                ////var translatedPredicate = x => func( OtherTypeFromSomeType(x))
                //rule.MessageFormatter = value as Func<object, string>;
            }
        }
    }
}