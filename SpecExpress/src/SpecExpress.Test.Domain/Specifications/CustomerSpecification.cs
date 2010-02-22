using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.Domain.Specifications
{
    public class CustomerSpecification : Validates<Customer>
    {
        public CustomerSpecification()
        {
            Check(c => c.Name).Required();
            Check(c => c.PrimaryContact).Required();
        }
    }
}