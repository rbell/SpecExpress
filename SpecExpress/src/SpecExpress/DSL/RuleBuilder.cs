using System;
using System.Collections;
using System.Linq;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Interface for a RuleBuilder.  The DSL may be extended by defining Extension methods to this interface
    /// to define Rules to enforce for a given property.
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public interface IRuleBuilder<T, TProperty>
    {
        /// <summary>
        /// Gets a JoinBuilder
        /// </summary>
        ActionJoinBuilder<T, TProperty> JoinBuilder { get; }

        /// <summary>
        /// Facilitates Registering a RuleValidator
        /// </summary>
        /// <param name="validator"><see cref="RuleValidator&lt;T, TProperty&gt;"/></param>
        /// <returns><see cref="IRuleBuilder&lt;T, TProperty&gt;"/></returns>
        IRuleBuilder<T, TProperty> RegisterValidator(RuleValidator<T, TProperty> validator);
    }

    /// <summary>
    /// Facilitates the expression of an individual Rule to apply to a property.
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class RuleBuilder<T, TProperty> : IRuleBuilder<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;
        private readonly ActionJoinBuilder<T, TProperty> JoinBuilder;
        private bool _negate = false;

        /// <summary>
        /// Initializes a new instance of <see cref="RuleBuilder&lt;T, TProperty&gt;"/>
        /// </summary>
        /// <param name="propertyValidator">The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is being build by the DSL.</param>
        public RuleBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
            if (_propertyValidator.RuleTree.Root != null)
            {
                NextRuleIsConditional = true;
            }
            JoinBuilder = new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        }

        /// <summary>
        /// Negates the current Rule being built.
        /// </summary>
        /// <example>
        /// Check(c => c.ActiveDate).Required().Not.LessThan(DateTime.Now);
        /// </example>
        public RuleBuilder<T, TProperty> Not
        {
            get
            {
                _negate = !_negate;
                return this;
            }
        }

        internal bool OrNextRule { get; set; }
        internal bool NextRuleIsConditional { get; set; }

        #region IRuleBuilder<T,TProperty> Members

        /// <summary>
        /// Facilitates Registering a RuleValidator on the PropertyValidator
        /// </summary>
        /// <param name="validator"><see cref="RuleValidator&lt;T, TProperty&gt;"/></param>
        /// <returns><see cref="IRuleBuilder&lt;T, TProperty&gt;"/></returns>
        IRuleBuilder<T, TProperty> IRuleBuilder<T, TProperty>.RegisterValidator(RuleValidator<T, TProperty> validator)
        {
            validator.Negate = _negate;
            if (OrNextRule)
            {
                _propertyValidator.OrRule(validator);
                OrNextRule = false;
            }
            else
            {
                _propertyValidator.AndRule(validator);
            }
            return this;
        }

        /// <summary>
        /// Gets a JoinBuilder
        /// </summary>
        ActionJoinBuilder<T, TProperty> IRuleBuilder<T, TProperty>.JoinBuilder
        {
            get { return JoinBuilder; }
        }

        #endregion

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> Specification()
        {
            var specRule = new SpecificationRule<T, TProperty>();
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> Specification<TSpecType>() where TSpecType : Validates<TProperty>, new()
        {
            var specRule = new SpecificationRule<T, TProperty, TSpecType>();
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Sets Specification used to validate this Property to the Default
        /// </summary>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> Specification(Action<Validates<TProperty>> rules)
        {
            var specification = new SpecificationExpression<TProperty>();
            rules(specification);
            var specRule = new SpecificationRule<T, TProperty>(specification);
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Allows the definition of an inline specification to be applied to each item in a collection.
        /// </summary>
        /// <example>
        /// Check(cust => cust.ContactCollection).Required()
        ///     .ForEachSpecification&ltContact&gt( spec => spec.Check(c =&gt c.LastName).Required(), "Contacts" );
        /// </example>
        /// <typeparam name="TCollectionType">The type of instances contained in the collection.</typeparam>
        /// <param name="rules"><see cref="Action&lt;Validates&lt;TCollectionType&gt;&gt;"/></param>
        /// <param name="itemName">Name of property to use in notification message.</param>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>(Action<Validates<TCollectionType>> rules, string itemName)
        {
            var specification = new SpecificationExpression<TCollectionType>();
            rules(specification);
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>(specification, itemName);
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Allows the definition of an inline specification to be applied to each item in a collection.
        /// </summary>
        /// <example>
        /// Check(cust => cust.Contacts).Required()
        ///     .ForEachSpecification&ltContact&gt( spec => spec.Check(c =&gt c.LastName).Required() );
        /// </example>
        /// <typeparam name="TCollectionType">The type of instances contained in the collection.</typeparam>
        /// <param name="rules"><see cref="Action&lt;Validates&lt;TCollectionType&gt;&gt;"/></param>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>(Action<Validates<TCollectionType>> rules)
        {
            return ForEachSpecification<TCollectionType>(rules, string.Empty);
        }

        /// <summary>
        /// Allows the enforcement of a specific specification to be applied to each item in a collection.
        /// </summary>
        /// <example>
        /// Check(cust => cust.ContactCollection).Required()
        ///     .ForEachSpecification&lt;Contact, ContactSpecification&gt;("Contacts");
        /// </example>
        /// <param name="itemName">Tne name the property to use when a notification is generated.</param>
        /// <typeparam name="TCollectionType">The type of instances contained in the collection.</typeparam>
        /// <typeparam name="TCollectionSpecType">The Specification type to apply to each item in the collection.</typeparam>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType, TCollectionSpecType>(string itemName)
            where TCollectionSpecType : Validates<TCollectionType>, new()
        {
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType, TCollectionSpecType>(itemName);
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Allows the enforcement of a specific specification to be applied to each item in a collection.
        /// </summary>
        /// <example>
        /// Check(cust => cust.Contacts).Required()
        ///     .ForEachSpecification&lt;Contact, ContactSpecification&gt;(); 
        /// </example>
        /// <typeparam name="TCollectionType">The type of instances contained in the collection.</typeparam>
        /// <typeparam name="TCollectionSpecType">The Specification type to apply to each item in the collection.</typeparam>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType, TCollectionSpecType>()
           where TCollectionSpecType : Validates<TCollectionType>, new()
        {
            return ForEachSpecification<TCollectionType, TCollectionSpecType>(string.Empty);
        }

        /// <summary>
        /// Allows the enforcement of the default specification to be applied to each item in a collection.
        /// </summary>
        /// <example>
        /// Check(cust => cust.Contacts).Required()
        ///     .ForEachSpecification&lt;Contact&gt;(); 
        /// </example>
        /// <typeparam name="TCollectionType">The type of instances contained in the collection.</typeparam>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> ForEachSpecification<TCollectionType>()
        {
            var specRule = new ForEachSpecificationRule<T, TProperty, TCollectionType>();
            return AddRuleAndReturnActionJoin(specRule);
        }

        public IAndOr<T, TProperty> ForEachSpecification()
        {
            var specRule = new ForEachSpecificationRuleUntyped<T, TProperty>();
            return AddRuleAndReturnActionJoin(specRule);
        }

        public IAndOr<T, TProperty> ForEachSpecification(string itemName)
        {
            var specRule = new ForEachSpecificationRuleUntyped<T, TProperty>(itemName);
            return AddRuleAndReturnActionJoin(specRule);
        }

        /// <summary>
        /// Facilitates the grouping of a subset of rules to dictate precidence for And / Or opertations.
        /// </summary>
        /// <example>
        ///    Validate ActiveDate is in a five day window starting 10 days ago OR a five day window starting in 5 days from now.
        ///    spec.Check(c => c.ActiveDate).Required()
        ///        .Group(d => d.GreaterThan(DateTime.Now.AddDays(-10))
        ///                        .And.LessThan(DateTime.Now.AddDays(-5)))
        ///        .Or
        ///        .Group(d => d.GreaterThan(DateTime.Now.AddDays(5))
        ///                        .And.LessThan(DateTime.Now.AddDays(10)));
        /// </example>
        /// <param name="rules"><see cref="Action&lt;RuleBuilder&lt;T, TProperty&gt;&gt;"/></param>
        /// <returns><see cref="IAndOr&lt;T, TProperty&gt;"/></returns>
        public IAndOr<T, TProperty> Group(Action<RuleBuilder<T, TProperty>> rules)
        {
            var innerPropertyValidator = new PropertyValidator<T, TProperty>(_propertyValidator);
            var groupRules = new RuleBuilder<T, TProperty>(innerPropertyValidator);
            rules(groupRules);
            if (OrNextRule)
            {
                if (NextRuleIsConditional)
                {
                    _propertyValidator.ConditionalOrGroup(innerPropertyValidator);
                }
                else
                {
                    _propertyValidator.OrGroup(innerPropertyValidator);
                }
            }
            else
            {
                if (NextRuleIsConditional)
                {
                    _propertyValidator.ConditionalAndGroup(innerPropertyValidator);
                }
                else
                {
                    _propertyValidator.AndGroup(innerPropertyValidator);
                }
            }

            NextRuleIsConditional = false;
            OrNextRule = false;

            return new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        }

        private ActionJoinBuilder<T, TProperty> AddRuleAndReturnActionJoin(RuleValidator specRule)
        {
            if (OrNextRule)
            {
                if (NextRuleIsConditional)
                {
                    _propertyValidator.ConditionalOrRule(specRule);
                }
                else
                {
                    _propertyValidator.OrRule(specRule);
                }
            }
            else
            {
                if (NextRuleIsConditional)
                {
                    _propertyValidator.ConditionalAndRule(specRule);
                }
                else
                {
                    _propertyValidator.AndRule(specRule);
                }
            }

            return new ActionJoinBuilder<T, TProperty>(_propertyValidator);
        }


    }

}