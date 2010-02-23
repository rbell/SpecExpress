using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test
{
    public class InternationalAddressSpecification : Validates<Address>
    {
        public InternationalAddressSpecification()
        {
            Check(a => a.City).Required().MaxLength(50).And.IsAlpha();
            Check(a => a.Street).Required().MaxLength(100);
            Check(a => a.Country.Id).Required().IsInSet(new List<string>() {"CA", "GB", "DE"});
            Check(a => a.Province).Optional().IsAlpha();
            Check(a => a.PostalCode).Optional().MaxLength(50);

        }
    }
}
