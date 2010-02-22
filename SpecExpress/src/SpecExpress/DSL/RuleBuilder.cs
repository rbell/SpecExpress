using System;
using System.Collections;
using System.Linq;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Interface that Rule Extensions will extend.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IRuleBuilder<T, TProperty>
    {
        ActionJoinBuilder<T, TProperty> JoinBuilder { get; }
        RuleBuilder<T, TProperty> RegisterValidator(RuleValidator<T, TProperty> validator);
    }

    

    /// <summary>
    /// TODO: Document what this does in detail and how Rule Extensions extend it.
    /// Builds a rule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class RuleBuilder<T, TProperty> : IRuleBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;
        private readonly ActionJoinBuilder<T, TProperty> JoinBuilder;
        private bool _negate = false;

        public RuleBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
            JoinBuilder = new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        }
        
        public RuleBuilder<T, TProperty> Not
        {
            get
            {
                _negate = !_negate;
                return this;
            }
        }

        #region IRuleBuilder<T,TProperty> Members

        RuleBuilder<T, TProperty> IRuleBuilder<T, TProperty>.RegisterValidator(RuleValidator<T, TProperty> validator)
        {
            validator.Negate = _negate;    
            _propertyValidator.AddRule(validator);
            return this;
        }

        ActionJoinBuilder<T, TProperty> IRuleBuilder<T, TProperty>.JoinBuilder
        {
            get { return JoinBuilder; }
        }

        #endregion

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOr<T, TProperty> Specification()
        {
            var specRule = new SpecificationRule<T, TProperty>();
            return AddRuleAndReturnActionJoin(specRule);


        }

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOr<T, TProperty> Specification<TSpecType>() where TSpecType : Validates<TProperty>, new()
        {
            var specification = new TSpecType();
            var specRule = new SpecificationRule<T, TProperty>(specification);
            return AddRuleAndReturnActionJoin(specRule);
        }



        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOr<T, TProperty> Specification(Action<Validates<TProperty>> rules)
        {
            var specification = new SpecificationExpression<TProperty>();
            rules(specification);
            var specRule = new SpecificationRule<T, TProperty>(specification);
            return AddRuleAndReturnActionJoin(specRule);
        }


        /// <summary>
        /// ForEachSpecification<< Contact >> ( spec => spec.Check(c => c.LastName).Required(); );
        /// </summary>
        /// <typeparam name="TCollectionType"></typeparam>
        /// <param name="rules"></param>
        /// <returns></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>(Action<Validates<TCollectionType>> rules, string itemName)
        {
            var specification = new SpecificationExpression<TCollectionType>();
            rules(specification);
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>(specification, itemName);
            return AddRuleAndReturnActionJoin(specRule);
        }

        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>(Action<Validates<TCollectionType>> rules)
        {
            return ForEachSpecification<TCollectionType>(rules, string.Empty);
        }

        /// <summary>
        /// ForEachSpecification<< Contact, ContactSpecification >>(); //explictly use supplied specification
        /// </summary>
        /// <typeparam name="TCollectionType"></typeparam>
        /// <typeparam name="TCollectionSpecType"></typeparam>
        /// <returns></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType, TCollectionSpecType>(string itemName)
            where TCollectionSpecType : Validates<TCollectionType>, new()
        {
            var specification = new TCollectionSpecType();
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>(specification, itemName);
            return AddRuleAndReturnActionJoin(specRule);
        }

        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType, TCollectionSpecType>()
           where TCollectionSpecType : Validates<TCollectionType>, new()
        {
            return ForEachSpecification<TCollectionType, TCollectionSpecType>(string.Empty);
        }

        /// <summary>
        /// ForEachSpecification<< Contact >>(); //Default Specification
        /// </summary>
        /// <typeparam name="TCollectionType"></typeparam>
        /// <param name="rules"></param>
        /// <returns></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>()
        {
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>();
            return AddRuleAndReturnActionJoin(specRule);
        }

        private ActionJoinBuilder<T, TProperty> AddRuleAndReturnActionJoin(RuleValidator specRule)
        {
            _propertyValidator.AddRule(specRule);
            return new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        }

        
    }

    /// <summary>
    /// Interface that Rule Extensions will extend.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IRuleBuilderForCollections<T, TProperty> where TProperty : IEnumerable 
    {
        ActionJoinBuilderForCollections<T, TProperty> JoinBuilder { get; }
        RuleBuilderForCollections<T, TProperty> RegisterValidator(RuleValidator<T, TProperty> validator);
    }

    /// <summary>
    /// TODO: Document what this does in detail and how Rule Extensions extend it.
    /// Builds a rule
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class RuleBuilderForCollections<T, TProperty> : IRuleBuilderForCollections<T, TProperty> where TProperty : IEnumerable 
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;
        private readonly ActionJoinBuilderForCollections<T, TProperty> JoinBuilder;

        public RuleBuilderForCollections(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
            JoinBuilder = new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }

        #region IRuleBuilder<T,TProperty> Members

        RuleBuilderForCollections<T, TProperty> IRuleBuilderForCollections<T, TProperty>.RegisterValidator(RuleValidator<T, TProperty> validator)
        {
            _propertyValidator.AddRule(validator);
            return this;
        }

        ActionJoinBuilderForCollections<T, TProperty> IRuleBuilderForCollections<T, TProperty>.JoinBuilder
        {
            get { return JoinBuilder; }
        }

        #endregion

        public IAndOrForCollections<T, TProperty> MessageKey<TMessage>(TMessage messageKey)
        {
            //set error message for last rule added
            RuleValidator rule = _propertyValidator.Rules.Last();
            rule.MessageKey = messageKey;
            return new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOrForCollections<T, TProperty> Specification()
        {
            var specRule = new SpecificationRule<T, TProperty>();

            _propertyValidator.AddRule(specRule);
            return new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOrForCollections<T, TProperty> Specification<TSpecType>() where TSpecType : Validates<TProperty>, new()
        {
            TSpecType specification = new TSpecType();
            var specRule = new SpecificationRule<T, TProperty>(specification);

            _propertyValidator.AddRule(specRule);
            return new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }



        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns></returns>
        public IAndOrForCollections<T, TProperty> Specification(Action<Validates<TProperty>> rules)
        {
            var specification = new SpecificationExpression<TProperty>();
            rules(specification);
            var specRule = new SpecificationRule<T, TProperty>(specification);

            _propertyValidator.AddRule(specRule);
            return new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }

        ///// <summary>
        ///// ForEachSpecification<< Contact >>();
        ///// </summary>
        ///// <typeparam name="TCollectionType"></typeparam>
        ///// <typeparam name="TCollectionSpecType"></typeparam>
        ///// <returns></returns>
        //public IAndOr<T, TProperty> ForEachSpecification<TCollectionType, TCollectionSpecType>()
        //    where TCollectionSpecType : SpecificationBase<TCollectionType>, new()
        //     //where TProperty : IEnumerable
        //{
        //    var specification = new TCollectionSpecType();
        //    var specRule = new  ForEachSpecificationRule<T, TCollectionType>(specification);
        //    _propertyValidator.AddRule(specRule);

        //    return new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        //}

        /// <summary>
        /// ForEachSpecification<<Contact>>( spec => 
        /// {
        ///     spec.Check(r.Property).Required();
        /// });
        /// </summary>
        /// <typeparam name="TCollectionType"></typeparam>
        /// <param name="rules"></param>
        /// <returns></returns>
        public IAndOrForCollections<T, TProperty> ForEachSpecification<TCollectionType>(Action<Validates<TCollectionType>> rules)
        {
            var specification = new SpecificationExpression<TCollectionType>();
            rules(specification);
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>();


            _propertyValidator.AddRule(specRule);
            return new ActionJoinBuilderForCollections<T, TProperty>(_propertyValidator);
        }

    }

}