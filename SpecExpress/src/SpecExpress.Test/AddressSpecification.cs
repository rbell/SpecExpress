using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpressTest.Entities;

namespace SpecExpress.Test
{
    public class AddressSpecification : Validates<Address>
    {
        public AddressSpecification()
        {
            Check(a => a.City).Required().MaxLength(50).And.IsAlpha();
            Check(a => a.Street).Required().MaxLength(100);
            Check(a => a.Province).Required();
            Check(a => a.PostalCode).Required().IsNumeric();
        }
    }
}
