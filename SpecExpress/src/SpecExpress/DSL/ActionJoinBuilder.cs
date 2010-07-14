using System;
using System.Collections;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;

namespace SpecExpress.DSL
{
    public interface IWith<T, TProperty>
    {
        IAndOr<T, TProperty> With(Action<WithBuilder<T, TProperty>> w);
    }

    public interface IAndOr<T, TProperty>
    {
        RuleBuilder<T, TProperty> And { get; }
        RuleBuilder<T, TProperty> Or { get; }
    }

    /// <summary>
    /// Provides the ability to transition from a Rule definition to a WithBuilder and to
    /// define "And / Or" relations to other rules.
    /// </summary>
    /// <typeparam name="T">Type of entity being validated.</typeparam>
    /// <typeparam name="TProperty">Type of property on the entity being validated.</typeparam>
    public class ActionJoinBuilder<T, TProperty> : IWith<T, TProperty>, IAndOr<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        /// <summary>
        /// Initializes a new instance of a <see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/>
        /// </summary>
        /// <param name="propertyValidator">
        /// The <see cref="PropertyValidator&lt;T, TProperty&gt;"/> that is to be built.
        ///  </param>
        public ActionJoinBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        #region IAndOr<T,TProperty> Members

        /// <summary>
        /// Define an And relation between two Rules
        /// </summary>
        public RuleBuilder<T, TProperty> And
        {
            get { return new RuleBuilder<T, TProperty>(_propertyValidator); }
        }

        /// <summary>
        /// Define an Or relation between two Rules
        /// </summary>
        public RuleBuilder<T, TProperty> Or
        {
            get
            {                
                var builder = new RuleBuilder<T, TProperty>(_propertyValidator);
                builder.OrNextRule = true;
                return builder;
            }
        }

        #endregion

        #region IWith<T,TProperty> Members
        /// <summary>
        /// Supply additional options for the prior rule.
        /// </summary>
        /// <param name="w"><see cref="Action&lt;WithBuilder&lt;T,TProperty&gt;&gt;"/></param>
        /// <returns></returns>
        public IAndOr<T,TProperty> With(Action<WithBuilder<T,TProperty>> w)
        {
            w(new WithBuilder<T, TProperty>(_propertyValidator));
            return this;
        }
        #endregion

    }
}