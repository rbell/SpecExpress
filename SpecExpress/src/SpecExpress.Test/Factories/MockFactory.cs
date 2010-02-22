using System;
using System.Linq.Expressions;
using Rhino.Mocks;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.Factories
{
    public static class MockFactory
    {
        public static PropertyValidator<Customer, string> MockCustomerNamePropertyValidator(MockRepository mocks)
        {
            Expression<Func<Customer, string>> expression = c => c.Name;
            return mocks.StrictMock<PropertyValidator<Customer, string>>(new object[] {expression});
        }
    }
}