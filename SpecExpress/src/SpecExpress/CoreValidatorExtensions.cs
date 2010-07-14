using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SpecExpress.DSL;
using SpecExpress.Rules;
using SpecExpress.Rules.Boolean;
using SpecExpress.Rules.Collection;
using SpecExpress.Rules.DateValidators;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Rules.IComparableValidators;
using SpecExpress.Rules.StringValidators;

namespace SpecExpress
{
    /// <summary>
    /// Changed return from RuleBuilder to ActionJoin so displays AND/WITH after a Rule.
    /// </summary>
    public static class CoreValidatorExtensions
    {

        #region DateTime
        /// <summary>
        /// Enforces that a DateTime is in the future.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, DateTime&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, DateTime&gt;"/></returns>
        public static ActionJoinBuilder<T, DateTime> IsInFuture<T>(this IRuleBuilder<T, DateTime> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInFuture<T>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a Nullable DateTime is in the future.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, System.Nullable&lt;DateTime&gt;&gt;&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, System.Nullable&lt;DateTime&gt;&gt;"/></returns>
        public static ActionJoinBuilder<T, System.Nullable<DateTime>> IsInFuture<T>(this IRuleBuilder<T, System.Nullable<DateTime>> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInFutureNullable<T>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a DateTime is in the past.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, DateTime&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, DateTime&gt;"/></returns>
        public static ActionJoinBuilder<T, DateTime> IsInPast<T>(this IRuleBuilder<T, DateTime> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInPast<T>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a Nullable DateTime is in the past.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, System.Nullable&lt;DateTime&gt;&gt;&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, System.Nullable&lt;DateTime&gt;&gt;"/></returns>
        public static ActionJoinBuilder<T, System.Nullable<DateTime>> IsInPast<T>(this IRuleBuilder<T, System.Nullable<DateTime>> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInPastNullable<T>());
            return expression.JoinBuilder;
        }

        #endregion

        #region String

        /// <summary>
        /// Enforces that a string's length is between a min and max length.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min">Minimum length of the string.</param>
        /// <param name="max">Maximum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, int min,
                                                                    int max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length is between a min and max length.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the min length.</param>
        /// <param name="max">Maximum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min,
                                                            int max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length is between a min and max length.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min">Minimum length of the string.</param>
        /// <param name="max"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the max length.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, int min,
                                                            Expression<Func<T, int>> max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length is between a min and max length.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the min length.</param>
        /// <param name="max"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the max length.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min,
                                                            Expression<Func<T, int>> max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length be equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="length">The length that the string must be equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthEqualTo<T>(this IRuleBuilder<T, string> expression, int length)
        {
            expression.RegisterValidator(new LengthEqualTo<T>(length));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length be equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="length"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the string must be equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> LengthEqualTo<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> length)
        {
            expression.RegisterValidator(new LengthEqualTo<T>(length));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be longer than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min">Minimum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> MinLength<T>(this IRuleBuilder<T, string> expression, int min)
        {
            expression.RegisterValidator(new MinLength<T>(min));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be longer than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="min"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the minimum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> MinLength<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min)
        {
            expression.RegisterValidator(new MinLength<T>(min));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be shorter than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="max">Maximum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> MaxLength<T>(this IRuleBuilder<T, string> expression, int max)
        {
            expression.RegisterValidator(new MaxLength<T>(max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be shorter than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="max"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the maximum length of the string.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> MaxLength<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> max)
        {
            expression.RegisterValidator(new MaxLength<T>(max));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be shorter than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="regexPattern">The regex that the string must match.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> Matches<T>(this IRuleBuilder<T, string> expression, string regexPattern)
        {
            expression.RegisterValidator(new Matches<T>(regexPattern));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string's length must be shorter than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <param name="regexPattern"><see cref="Expression&lt;Func&lt;T, string&gt;&gt;"/> that resolves to another property on T that will be used for the regex that the string must match.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> Matches<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, string>> regexPattern)
        {
            expression.RegisterValidator(new Matches<T>(regexPattern));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string contains only numeric characters.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> IsNumeric<T>(this IRuleBuilder<T, string> expression)
        {
            expression.RegisterValidator(new Numeric<T>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a string contains only alpha characters.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, string&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, string&gt;"/></returns>
        public static ActionJoinBuilder<T, string> IsAlpha<T>(this IRuleBuilder<T, string> expression)
        {
            expression.RegisterValidator(new Alpha<T>());
            return expression.JoinBuilder;
        }

        #endregion

        #region Collection
        /// <summary>
        /// Enforces that a collection contain an insance of a specific object.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="valueToLookFor">The object to look for in the collection.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Contains<T, TProperty>(this IRuleBuilder<T, TProperty> expression, object valueToLookFor) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.Contains<T, TProperty>(valueToLookFor));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a collection contain an insance of a specific object.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="valueToLookFor"><see cref="Expression&lt;Func&lt;T, IEnumerable&gt;&gt;"/> that resolves to another property on T that will be used to specify the object to look for in the collection.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Contains<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, IEnumerable>> valueToLookFor) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.Contains<T, TProperty>(valueToLookFor));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Applies a predicate to each instance in the collection and enforces that the predicate must return true for each instance.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="predicate">The Predicate to be applied to each instance in the collection.</param>
        /// <param name="messageTemplate">The message to use when the predicate returns false.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> ForEach<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Predicate<object> predicate, string messageTemplate) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.ForEach<T, TProperty>(predicate, messageTemplate));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection is empty.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> IsEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> expression) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.IsEmpty<T, TProperty>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a minimum number of instances.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThan">The minimum number of instances that the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountGreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int greaterThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a minimum number of instances.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThan"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the minimum number of instances that the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountGreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> greaterThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances greater than or equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThanEqualTo">The number of instances that the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountGreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int greaterThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances greater than or equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThanEqualTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the minimum number of instances that the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountGreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> greaterThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances less than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThan">The quantity of instances the collection must contain less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountLessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int lessThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances less than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThan"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the quantity of instances the collection must contain less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountLessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> lessThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances less than or equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThanEqualTo">The quantity of instances the collection must contain less than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountLessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int lessThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must contain a number of instances less than or equal to a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThanEqualTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the quantity of instances the collection must contain less than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountLessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> lessThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collectin must conatain an exact number of instances.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="equalTo">The number of instances the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int equalTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountEqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the collection must conatain an exact number of instances.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Collection which must implement IEnumerable.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="equalTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the exact quantity of instances the collection must contain.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> CountEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> equalTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountEqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        #endregion

        #region IComparable
        /// <summary>
        /// Enforces that a property must be Greater Than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThan">The value that the property must be greater than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> GreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty greaterThan)
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Greater Than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThan"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be greater than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> GreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> greaterThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Greater Than or Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThanEqualTo">The value that the property must be greater than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> GreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty greaterThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Greater Than or Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="greaterThanEqualTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be greater than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> GreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> greaterThanEqualTo)
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Less Than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThan">The value that the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> LessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty lessThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Less Than a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThan"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> LessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> lessThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Less Than or Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThanEqualTo">The value that the property must be less than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> LessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty lessThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Less Than or Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="lessThanEqualTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be less than or equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> LessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> lessThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="equalTo">The value that the property must be equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> EqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty equalTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.EqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be Equal To a value.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="equalTo"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be equal to.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> EqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> equalTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.EqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be between to values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="floor">The minimum value that the property must be greater than.</param>
        /// <param name="ceiling">The maximum value tha the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty floor, TProperty ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be between to values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="floor"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be greater than.</param>
        /// <param name="ceiling">The maximum value tha the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> floor, TProperty ceiling)
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be between to values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="floor">The minimum value that the property must be greater than.</param>
        /// <param name="ceiling"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty floor, Expression<Func<T, TProperty>> ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be between to values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="floor"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be greater than.</param>
        /// <param name="ceiling"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the value that the property must be less than.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> floor, Expression<Func<T, TProperty>> ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }
        #endregion

        #region Boolean
        /// <summary>
        /// Enforces that a property must be true.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, bool&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, bool&gt;"/></returns>
        public static ActionJoinBuilder<T, bool> IsTrue<T>(this IRuleBuilder<T, bool> expression)
        {
            expression.RegisterValidator(new Rules.Boolean.IsTrue<T>());
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that a property must be false.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, bool&gt;"/> that is this method is extending.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, bool&gt;"/></returns>
        public static ActionJoinBuilder<T, bool> IsFalse<T>(this IRuleBuilder<T, bool> expression)
        {
            expression.RegisterValidator(new Rules.Boolean.IsFalse<T>());
            return expression.JoinBuilder;
        }
        
        #endregion

        #region Custom
        /// <summary>
        /// Allows the definition of a customized rule that may enforce more complex business rules.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="rule">A <see cref="Func&lt;T, TProperty, bool&gt;"/> that is executed when the validation takes place.</param>
        /// <param name="message">A message that is used when the rule is not valid.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> Expect<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Func<T, TProperty, bool> rule, string message)
        {
            expression.RegisterValidator(new CustomRule<T, TProperty>(rule));
            //Custom messages can't derive what the Error Message is because each case is so generic
            expression.JoinBuilder.With(m => m.Message = message);
            return expression.JoinBuilder;
        }
        #endregion

        #region General
        /// <summary>
        /// Enforces that the value of a property is in a set of valid values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="set">Instance of <see cref="IEnumerable&lt;TProperty&gt;"/> that defines the set of valid values.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> IsInSet<T, TProperty>(this IRuleBuilder<T, TProperty> expression, IEnumerable<TProperty> set)
        {
            expression.RegisterValidator(new IsInSet<T, TProperty>(set));
            return expression.JoinBuilder;
        }

        /// <summary>
        /// Enforces that the value of a property is in a set of valid values.
        /// </summary>
        /// <typeparam name="T">Type that Specification is being defined for.</typeparam>
        /// <typeparam name="TProperty">Type of the Property.</typeparam>
        /// <param name="expression">Instance of <see cref="IRuleBuilder&lt;T, TProperty&gt;"/> that is this method is extending.</param>
        /// <param name="set"><see cref="Expression&lt;Func&lt;T, int&gt;&gt;"/> that resolves to another property on T that will be used for the instance of <see cref="IEnumerable&lt;TProperty&gt;"/> that defines the set of valid values.</param>
        /// <returns><see cref="ActionJoinBuilder&lt;T, TProperty&gt;"/></returns>
        public static ActionJoinBuilder<T, TProperty> IsInSet<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Func<T, IEnumerable<TProperty>> set)
        {
            expression.RegisterValidator(new IsInSet<T, TProperty>(set));
            return expression.JoinBuilder;
        }
        #endregion

    }
}