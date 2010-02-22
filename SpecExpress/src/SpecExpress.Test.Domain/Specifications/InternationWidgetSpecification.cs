using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    public class InternationalWidgetSpecification : WidgetSpecificationBase
    {
        public InternationalWidgetSpecification()
            : base()
        {
            Check(w => w.Name).Required().IsNumeric();
        }
    }
}
