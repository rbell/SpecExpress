using System;
using System.Collections;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;

namespace SpecExpress.DSL
{
    /// <summary>
    /// Used to filter out Methods from RuleBuilder that aren't valid for next State
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IWith<T, TProperty>
    {
        IAndOr<T, TProperty> With(Action<WithBuilder<T, TProperty>> w);
    }

    public interface IAndOr<T, TProperty>
    {
        RuleBuilder<T, TProperty> And { get; }
        RuleBuilder<T, TProperty> Or { get; }
    }

    public interface IAndOrForCollections<T, TProperty> where TProperty : IEnumerable
    {
        RuleBuilderForCollections<T, TProperty> And { get; }
        RuleBuilderForCollections<T, TProperty> Or { get; }
    }

    public class ActionJoinBuilder<T, TProperty> : IWith<T, TProperty>, IAndOr<T, TProperty>
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        public ActionJoinBuilder(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        #region IAndOr<T,TProperty> Members

        public RuleBuilder<T, TProperty> And
        {
            get { return new RuleBuilder<T, TProperty>(_propertyValidator); }
        }

        public RuleBuilder<T, TProperty> Or
        {
            get
            {
                var orExpression = new PropertyValidator<T, TProperty>(_propertyValidator);
                _propertyValidator.Child = orExpression;
                return new RuleBuilder<T, TProperty>(_propertyValidator.Child);
            }
        }

        #endregion

        #region IWith<T,TProperty> Members

        public IAndOr<T,TProperty> With(Action<WithBuilder<T,TProperty>> w)
        {
            w(new WithBuilder<T, TProperty>(_propertyValidator));
            return this;
        }
        #endregion

    }

    public class ActionJoinBuilderForCollections<T, TProperty> : IAndOrForCollections<T, TProperty> where TProperty :IEnumerable
    {
        private readonly PropertyValidator<T, TProperty> _propertyValidator;

        public ActionJoinBuilderForCollections(PropertyValidator<T, TProperty> propertyValidator)
        {
            _propertyValidator = propertyValidator;
        }

        #region IAndOr<T,TProperty> Members

        public RuleBuilderForCollections<T, TProperty> And
        {
            get { return new RuleBuilderForCollections<T, TProperty>(_propertyValidator); }
        }

        public RuleBuilderForCollections<T, TProperty> Or
        {
            get
            {
                var orExpression = new PropertyValidator<T, TProperty>(_propertyValidator);
                _propertyValidator.Child = orExpression;
                return new RuleBuilderForCollections<T, TProperty>(_propertyValidator.Child);
            }
        }
        #endregion

    }
}