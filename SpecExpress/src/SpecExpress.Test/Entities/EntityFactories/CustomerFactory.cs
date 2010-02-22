using System;
using SpecExpress.Test.Entities.EntityBuilders;

namespace SpecExpress.Test.Entities.EntityFactories
{
    public static class CustomerFactory
    {
        public static CustomerBuilder CreateSampleCustomer()
        {
            return new CustomerBuilder().Name("Sample Customer").CustomerDate(DateTime.Today);
        }
    }
}