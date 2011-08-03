using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public static class AssertSpecification
    {
        public static void Assert(Action<Validates<object>> rules)
        {
            var spec = new SpecificationExpression<object>(rules);

            var vn = spec.Validate(spec.Instance);
            if (!vn.IsValid)
            {
                throw new ValidationException("Invalid " + spec.Instance.GetType().ToString(), vn);
            }
        }
    }
}
