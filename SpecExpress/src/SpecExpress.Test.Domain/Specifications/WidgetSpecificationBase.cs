using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Test.Domain.Entities;

namespace SpecExpress.Test.Domain.Specifications
{
    public abstract class  WidgetSpecificationBase: Validates<Widget>
    {
        public WidgetSpecificationBase()
        {
            Check(w => w.Name).Required().MaxLength(5);
        }
    }
}
