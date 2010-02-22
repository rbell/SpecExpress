using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Test.Domain.Specifications
{
    public class USWidgetSpecification : WidgetSpecificationBase
    {
        public USWidgetSpecification() : base()
        {
            Check(w => w.Name).Required().IsAlpha();
            IsDefaultForType();
          
        }
    }
}
