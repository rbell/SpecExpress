using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{ 
    public class SpecExpressConfigurationException : Exception
    {
        public SpecExpressConfigurationException(string message)
            : base(message)
        {

        }

        public SpecExpressConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
