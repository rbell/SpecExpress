using SpecExpress.Test.Domain.Entities;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test.Domain.Specifications
{
    public class ContactSpecification : Validates<Contact>
    {
        public ContactSpecification()
        {
            IsDefaultForType();

            Check(c => c.FirstName).Required();
            Check(c => c.LastName).Required();
            Check(c => c.Addresses).Required().ForEachSpecification<Address, AddressSpecification>();
            Check(c => c.PrimaryAddress).Required();
            Check(c => c.DefaultProject).Required();
            Check(c => c, "Credit Score").Required().Expect((c, d) => 1 == 1, "Error");
        }
    }
}