using System.Collections;
using System.Linq;
using SpecExpress.MessageStore;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using System;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Facilitates the ability to stipulate additional options for the prior rule.
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class WithBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="WithBuilder&lt;T, TProperty&gt;"/>
        /// </summary>
        /// <param name="propertyValidator">The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is being build by the DSL.</param>
        public WithBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        /// <summary>
        /// Specifies an override to the default error message if the rule fails.
        /// </summary>
        public string Message
        {
            get
            {
                //get message for last rule added
                RuleValidator rule = getLastRuleValidator();
                return rule.Message;
            }
            set
            {
                //set message for last rule added
                RuleValidator rule = getLastRuleValidator();
                rule.Message = value;
            }
        }

        /// <summary>
        /// Specifies a key to be used to look up the error message from the MessageStore.
        /// </summary>
        public object MessageKey
        {
            get
            {
                //get error message for last rule added
                RuleValidator rule = getLastRuleValidator();
                return rule.MessageKey;
            }
            set
            {
                //set error message for last rule added
                RuleValidator rule = getLastRuleValidator();
                rule.MessageKey = value;
            }
        }

        /// <summary>
        /// Returns a Func<TProperty, string> that is used to format the value of the property
        /// as displayed in an error message (i.e. Date format string).
        /// </summary>
        public Func<TProperty, string> FormatProperty
        {
            get
            {
                RuleValidator rule = getLastRuleValidator();
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

    	public Func<T, TProperty, bool> Condition
    	{
    		get 
			{ 
				RuleValidator<T, TProperty> rule = (RuleValidator<T, TProperty>) getLastRuleValidator();
				return rule.Condition;
			}
			set
			{
				RuleValidator<T, TProperty> rule = (RuleValidator<T, TProperty>)getLastRuleValidator();
				rule.Condition = value;
			}
    	}

        private RuleValidator getLastRuleValidator()
        {
            return _propertyValidator.RuleTree.LastRuleNode.Rule;
        }
    }
}