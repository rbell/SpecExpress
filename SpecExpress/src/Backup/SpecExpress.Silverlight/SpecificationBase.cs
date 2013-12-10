using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public abstract class SpecificationBase
    {
        private List<PropertyValidator> _propertyValidators = new List<PropertyValidator>();

        public abstract Type ForType { get; }

        public bool DefaultForType { get; private set; }

        public List<PropertyValidator> PropertyValidators
        {
            get { return _propertyValidators; }
            set
            {
                lock (this)
                {
                    _propertyValidators = value;
                }
            }
        }

        public bool Validate(object instance, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            lock (this)
            {
                foreach (var validator in PropertyValidators)
                {
                    validator.Validate(instance, specificationContainer, notification);
                }
                return notification.IsValid;
            }
        }

        public void IsDefaultForType()
        {
            DefaultForType = true;
        }
    }

}
