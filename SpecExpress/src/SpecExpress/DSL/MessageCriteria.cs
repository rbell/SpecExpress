using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.DSL
{
    public class MessageCriteria
    {
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }

    }

    public class MessageCriteria<TProperty>
    {
        public string PropertyName { get; set; }
        public TProperty PropertyValue { get { return (TProperty)PropertyValue; } set { PropertyValue = value; } }

    }
}
