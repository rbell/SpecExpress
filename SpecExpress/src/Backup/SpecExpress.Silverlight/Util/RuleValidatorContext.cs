using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SpecExpress.Enums;
using SpecExpress.Util;

namespace SpecExpress.Rules
{
    public class RuleValidatorContext
    {
        public RuleValidatorContext Parent { get; internal set; }
        public string PropertyName { get; protected set; }
        public object PropertyValue { get; protected set; }
        public MemberInfo PropertyInfo { get; set; }
        public Object Instance { get; set; }
        public ValidationLevelType Level { get; protected set; }

        public RuleValidatorContext(object instance, string propertyName, object propertyValue, ValidationLevelType level, MemberInfo propertyInfo, RuleValidatorContext parentContext)
        {
            Instance = instance;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            PropertyInfo = propertyInfo;
            Parent = parentContext;
            Level = level;
        }

        public RuleValidatorContext(object instance, PropertyValidator validator, RuleValidatorContext parentContext)
        {
            PropertyValue = validator.GetValueForProperty(instance);
            
            if (validator.PropertyNameOverride == null)
            {
                SelectPropertyName(instance, validator);
            }
            else
            {
                PropertyName = validator.PropertyNameOverride;
            }

            PropertyInfo = validator.PropertyInfo;
            Parent = parentContext;
            Instance = instance;
            Level = validator.Level;
        }

        private static readonly IDictionary<Type, IDictionary<string, PropertyInfo>> _propertyBag = new Dictionary<Type, IDictionary<string, PropertyInfo>>();
        private static readonly IDictionary<PropertyInfo, DisplayAttribute> _attributeBag = new Dictionary<PropertyInfo, DisplayAttribute>();
        private void SelectPropertyName(object instance, PropertyValidator validator)
        {
            var instanceType = instance.GetType();
            
            var validationProperty = GetValidationProperty(validator, instanceType);

            if(validationProperty != null)
            {
                DisplayAttribute displayAttribute = null;
                if (!_attributeBag.ContainsKey(validationProperty))
                {
                    var displayAttributeQry = (from attribute in validationProperty.GetCustomAttributes(true)
                                               where attribute is DisplayAttribute
                                               select attribute).ToList();

                    if (displayAttributeQry.Any())
                    {
                        displayAttribute = displayAttributeQry.First() as DisplayAttribute;
                    }
                    _attributeBag[validationProperty] = displayAttribute;
                }
                else
                {
                    displayAttribute = _attributeBag[validationProperty];
                }

                if (displayAttribute != null)
                {
                    PropertyName = displayAttribute.GetName();
                }
            }

            if (String.IsNullOrEmpty(PropertyName))
            {
                PropertyName = validator.PropertyName.SplitPascalCase();
            }
        }

        private static PropertyInfo GetValidationProperty(PropertyValidator validator, Type instanceType)
        {
            if (!_propertyBag.ContainsKey(instanceType))
            {
                _propertyBag.Add(instanceType, new Dictionary<string, PropertyInfo>());
            }
            var propertyBag = _propertyBag[instanceType];
            if (!propertyBag.ContainsKey(validator.PropertyName))
            {
                propertyBag.Add(validator.PropertyName, instanceType.GetProperty(validator.PropertyName));
            }
            var validationProperty = propertyBag[validator.PropertyName];
            return validationProperty;
        }
    }



    /// <summary>
    /// Retrieves the name and value of the Property given an Expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class RuleValidatorContext<T, TProperty> : RuleValidatorContext
    {
        public RuleValidatorContext(T instance, PropertyValidator<T, TProperty> validator,
                                    RuleValidatorContext parentContext)
            : base(instance, validator, parentContext)
        {
            if (validator.PropertyNameOverrideExpression == null)
            {
                SelectPropertyName(validator);
                if (String.IsNullOrEmpty(PropertyName))
                {
                    PropertyName = validator.PropertyName.SplitPascalCase();
                }
            }
            else
            {
                PropertyName = validator.PropertyNameOverrideExpression(instance);
            }

        }

        private static readonly IDictionary<Type, IDictionary<string, PropertyInfo>> _propertyBag = new Dictionary<Type, IDictionary<string, PropertyInfo>>();
        private static readonly IDictionary<PropertyInfo, DisplayAttribute> _attributeBag = new Dictionary<PropertyInfo, DisplayAttribute>();
        private void SelectPropertyName(PropertyValidator validator)
        {
            var instanceType = typeof (T);
            var validationProperty = GetValidationProperty(validator, instanceType);

            if(validationProperty != null)
            {
                DisplayAttribute displayAttribute = null;
                if (!_attributeBag.ContainsKey(validationProperty))
                {
                    var displayAttributeQry = (from attribute in validationProperty.GetCustomAttributes(true)
                                               where attribute is DisplayAttribute
                                               select attribute).ToList();

                    if (displayAttributeQry.Any())
                    {
                        displayAttribute = displayAttributeQry.First() as DisplayAttribute;
                    }
                    _attributeBag[validationProperty] = displayAttribute;
                }
                else
                {
                    displayAttribute = _attributeBag[validationProperty];
                }

                if (displayAttribute != null)
                {
                    PropertyName = displayAttribute.GetName();
                }
            }

            if (String.IsNullOrEmpty(PropertyName))
            {
                PropertyName = validator.PropertyName.SplitPascalCase();
            }
        }

        private static PropertyInfo GetValidationProperty(PropertyValidator validator, Type instanceType)
        {
            if (!_propertyBag.ContainsKey(instanceType))
            {
                _propertyBag.Add(instanceType, new Dictionary<string, PropertyInfo>());
            }
            var propertyBag = _propertyBag[instanceType];
            if (!propertyBag.ContainsKey(validator.PropertyName))
            {
                propertyBag.Add(validator.PropertyName, instanceType.GetProperty(validator.PropertyName));
            }
            var validationProperty = propertyBag[validator.PropertyName];
            return validationProperty;
        }

        public RuleValidatorContext(T instance, string propertyName, TProperty propertyValue, MemberInfo propertyInfo, ValidationLevelType level,
                                    RuleValidatorContext parentContext)
            : base(instance, propertyName, propertyValue, level, propertyInfo, parentContext) { }

        public new TProperty PropertyValue
        {
            get { return (TProperty)base.PropertyValue; }
            set { base.PropertyValue = value; }
        }

        public new T Instance
        {
            get { return (T)base.Instance; }
            set { base.Instance = value; }
        }
    }
}