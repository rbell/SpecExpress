using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;


namespace SpecExpress.Rules.Collection
{
    public class ItemsAreUnique<T, TProperty> : RuleValidator<T, TProperty> where TProperty : IEnumerable
    {
        private string _dups;

        public override IList<RuleParameter> Params
        {
            get { return new List<RuleParameter>() { new RuleParameter("duplicates", _dups) }; }
        }

        public override bool Validate(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            _dups = duplicateItems(context.PropertyValue);

            return Evaluate(_dups == string.Empty, context, notification);
        }

        private string duplicateItems(IEnumerable collection)
        {
            var tmp = new ArrayList();
            var dups = new List<string>();

            foreach (object instance in collection)
            {
                if (tmp.Contains(instance))
                {
                    dups.Add(instance.ToString());
                }
                tmp.Add(instance);
            }

            var uniqueDups = (from dup in dups select dup).Distinct();

            if (uniqueDups.Any())
            {
                var sb = new StringBuilder();
                foreach (var uniqueDup in uniqueDups)
                {
                    sb.AppendLine(uniqueDup);
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}