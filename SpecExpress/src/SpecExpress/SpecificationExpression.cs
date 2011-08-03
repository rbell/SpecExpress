using System;

namespace SpecExpress
{

    public class SpecificationExpression<T> : Validates<T>
    {
        private T _instance;

        public SpecificationExpression()
        {

        }

        public SpecificationExpression(Action<Validates<T>> rules)
        {
            _instance = (T)rules.Target;
            rules(this);
        }

        public T Instance
        {
            get { return _instance; }
        }

    }
}