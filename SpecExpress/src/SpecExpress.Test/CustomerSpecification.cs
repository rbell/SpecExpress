using SpecExpress;
using SpecExpress.Test;
using SpecExpress.Test.Entities;

namespace SpecExpressTest
{
    public class CustomerSpecification : Validates<Customer>
    {
        public CustomerSpecification()
        {
            //Check(c => c.Address).Required().With.Specification<AddressSpecification>();
            //Check(c => c.Name).Required();
        }
    }
}