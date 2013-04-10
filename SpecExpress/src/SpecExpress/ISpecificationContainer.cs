using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public interface ISpecificationContainer
    {
        void Add<TEntity>(Validates<TEntity> expression);
        void Add(SpecificationBase spec);
        void Add<TSpec>() where TSpec : SpecificationBase, new();
        void Add<T>(SpecificationExpression<T> expression);
        void Add(IEnumerable<SpecificationBase> specs);
        void Add(IEnumerable<Type> specs);
        void Reset();
        SpecificationBase TryGetSpecification(Type type);
        SpecificationBase GetSpecification(Type type);
        Validates<TType> GetSpecification<TType>();
        Validates<TType> TryGetSpecification<TType>();
        List<SpecificationBase> GetAllSpecifications();
    }
}
