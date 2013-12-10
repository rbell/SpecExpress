using System;
using System.Linq.Expressions;

namespace SpecExpress
{
    /// <summary>
    /// Contains a LambdaExpression that has been compiled. 
    /// This is to optimize performance by guaranteeing that the expression gets compiled only once.
    /// </summary>
    public class CompiledExpression
    {
        private LambdaExpression _expression;
        private Delegate _func;

        public CompiledExpression(LambdaExpression expression)
        {
            _expression = expression;
            _func = _expression.Compile();
        }

        public LambdaExpression Expression
        {
            get { return _expression; }
        }

        public object Invoke(object param)
        {
            return _func.DynamicInvoke(new object[] { param });
        }
    }

    /// <summary>
    /// A Generic implementation of CompiledExpression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class CompiledFunctionExpression<T, TResult> : CompiledExpression
    {
        public CompiledFunctionExpression(Expression<Func<T, TResult>> expression) : base(expression)
        {}

        public TResult Invoke(T param)
        {
            return (TResult)base.Invoke(param);
        }
    }
}