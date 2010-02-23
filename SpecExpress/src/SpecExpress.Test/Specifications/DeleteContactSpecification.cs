using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.Specifications
{
    public class DeleteContactSpecification : Validates<Contact>
    {
        public DeleteContactSpecification()
        {
            Check(c => c.Active).Required().IsFalse();
        }
    }
}
