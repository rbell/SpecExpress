using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Specifications
{
    using SpecExpress.Test.Domain.Entities;

    public class ContactFirstNameWarningRequiredSpecification : Validates<Contact>
    {
        public ContactFirstNameWarningRequiredSpecification()
        {
            this.Warn(c => c.FirstName).Required();
        }
    }
}
