using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public class CustomRuleEvaluationException : Exception
    {
        public CustomRuleEvaluationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
