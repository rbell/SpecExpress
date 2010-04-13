using System.Collections.Generic;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test.Domain.Specifications
{
    public class AddressSpecification : Validates<Address>
    {
        public AddressSpecification()
        {
            Check(address => address.Street).Required();
            Check(address => address.City).Required();

            Check(address => address.Country).Required();
            
            Check(address => address.Province).If(address => new List<string> {"US", "GB", "AU"}.Contains(
                                                                            address.Country)).Required();
        }
    }
}