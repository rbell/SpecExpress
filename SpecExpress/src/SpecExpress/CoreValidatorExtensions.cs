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
        public static ActionJoinBuilder<T, DateTime> IsInFuture<T>(this IRuleBuilder<T, DateTime> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInFuture<T>());
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, System.Nullable<DateTime>> IsInFuture<T>(this IRuleBuilder<T, System.Nullable<DateTime>> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInFutureNullable<T>());
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, DateTime> IsInPast<T>(this IRuleBuilder<T, DateTime> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInPast<T>());
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, System.Nullable<DateTime>> IsInPast<T>(this IRuleBuilder<T, System.Nullable<DateTime>> expression)
        {
            expression.RegisterValidator(new Rules.DateValidators.IsInPastNullable<T>());
            return expression.JoinBuilder;
        }

        #endregion

        #region String

        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, int min,
                                                                    int max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min,
                                                            int max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, int min,
                                                            Expression<Func<T, int>> max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> LengthBetween<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min,
                                                            Expression<Func<T, int>> max)
        {
            expression.RegisterValidator(new LengthBetween<T>(min, max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> LengthEqualTo<T>(this IRuleBuilder<T, string> expression, int length)
        {
            expression.RegisterValidator(new LengthEqualTo<T>(length));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> LengthEqualTo<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> length)
        {
            expression.RegisterValidator(new LengthEqualTo<T>(length));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> MinLength<T>(this IRuleBuilder<T, string> expression, int min)
        {
            expression.RegisterValidator(new MinLength<T>(min));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> MinLength<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, int>> min)
        {
            expression.RegisterValidator(new MinLength<T>(min));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> MaxLength<T>(this IRuleBuilder<T, string> expression, int max)
        {
            expression.RegisterValidator(new MaxLength<T>(max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> MaxLength<T>(this IRuleBuilder<T, string> expression, Expression<Func<T,int>> max)
        {
            expression.RegisterValidator(new MaxLength<T>(max));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> Matches<T>(this IRuleBuilder<T, string> expression, string regexPattern)
        {
            expression.RegisterValidator(new Matches<T>(regexPattern));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> Matches<T>(this IRuleBuilder<T, string> expression, Expression<Func<T,string>> regexPattern)
        {
            expression.RegisterValidator(new Matches<T>(regexPattern));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> MaxLength<T>(this IRuleBuilder<T, string> expression, Expression<Func<T, string>> regexPattern)
        {
            expression.RegisterValidator(new Matches<T>(regexPattern));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> IsNumeric<T>(this IRuleBuilder<T, string> expression)
        {
            expression.RegisterValidator(new Numeric<T>());
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, string> IsAlpha<T>(this IRuleBuilder<T, string> expression)
        {
            expression.RegisterValidator(new Alpha<T>());
            return expression.JoinBuilder;
        }

        #endregion

        #region Collection
        public static ActionJoinBuilder<T, TProperty> Contains<T, TProperty>(this IRuleBuilder<T, TProperty> expression, object valueToLookFor) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.Contains<T, TProperty>(valueToLookFor));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> Contains<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, IEnumerable>> valueToLookFor) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.Contains<T, TProperty>(valueToLookFor));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> ForEach<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Predicate<object> predicate, string messageTemplate) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.ForEach<T, TProperty>(predicate, messageTemplate));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> IsEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> expression) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.IsEmpty<T, TProperty>());
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountGreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int greaterThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountGreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> greaterThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountGreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int greaterThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountGreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> greaterThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountGreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountLessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int lessThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountLessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> lessThan) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountLessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int lessThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountLessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> lessThanEqualTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountLessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, int equalTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountEqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> CountEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, int>> equalTo) where TProperty : IEnumerable
        {
            expression.RegisterValidator(new Rules.Collection.CountEqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        #endregion

        #region IComparable
        public static ActionJoinBuilder<T, TProperty> GreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty greaterThan)
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> GreaterThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> greaterThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThan<T, TProperty>(greaterThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> GreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty greaterThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> GreaterThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> greaterThanEqualTo)
        {
            expression.RegisterValidator(new Rules.IComparableValidators.GreaterThanEqualTo<T, TProperty>(greaterThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> LessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty lessThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> LessThan<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> lessThan) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThan<T, TProperty>(lessThan));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> LessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty lessThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> LessThanEqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> lessThanEqualTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.LessThanEqualTo<T, TProperty>(lessThanEqualTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> EqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty equalTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.EqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> EqualTo<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> equalTo) 
        {
            expression.RegisterValidator(new Rules.IComparableValidators.EqualTo<T, TProperty>(equalTo));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty floor, TProperty ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> floor, TProperty ceiling)
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, TProperty floor, Expression<Func<T, TProperty>> ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> Between<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Expression<Func<T, TProperty>> floor, Expression<Func<T, TProperty>> ceiling) 
        {
            expression.RegisterValidator(new Between<T, TProperty>(floor, ceiling));
            return expression.JoinBuilder;
        }
        #endregion

        #region Boolean
        public static ActionJoinBuilder<T, bool> IsTrue<T>(this IRuleBuilder<T, bool> expression)
        {
            expression.RegisterValidator(new Rules.Boolean.IsTrue<T>());
            return expression.JoinBuilder;
        }
        
        public static ActionJoinBuilder<T, bool> IsFalse<T>(this IRuleBuilder<T, bool> expression)
        {
            expression.RegisterValidator(new Rules.Boolean.IsFalse<T>());
            return expression.JoinBuilder;
        }
        
        #endregion

        #region Custom
        public static ActionJoinBuilder<T, TProperty> Expect<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Func<T, TProperty, bool> rule, string message)
        {
            expression.RegisterValidator(new CustomRule<T, TProperty>(rule));
            //Custom messages can't derive what the Error Message is because each case is so generic
            expression.JoinBuilder.With(m => m.Message = message);
            return expression.JoinBuilder;
        }
        #endregion

        #region General
        public static ActionJoinBuilder<T, TProperty> IsInSet<T, TProperty>(this IRuleBuilder<T, TProperty> expression, IEnumerable<TProperty> set)
        {
            expression.RegisterValidator(new IsInSet<T, TProperty>(set));
            return expression.JoinBuilder;
        }

        public static ActionJoinBuilder<T, TProperty> IsInSet<T, TProperty>(this IRuleBuilder<T, TProperty> expression, Func<T, IEnumerable<TProperty>> set)
        {
            expression.RegisterValidator(new IsInSet<T, TProperty>(set));
            return expression.JoinBuilder;
        }
        #endregion

    }
}