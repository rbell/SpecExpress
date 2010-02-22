using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.Domain.Specifications
{
    public class InvalidWidgetSpecification : Validates<Widget>
    {
        public InvalidWidgetSpecification()
        {
            Check(w => w.Name);
        }
    }
}
