using System;
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

        public RuleValidatorContext(object instance, string propertyName, object propertyValue,ValidationLevelType level, MemberInfo propertyInfo, RuleValidatorContext parentContext)
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
            PropertyName = String.IsNullOrEmpty(validator.PropertyNameOverride)
                              ? validator.PropertyName.SplitPascalCase()
                              : validator.PropertyNameOverride;
            PropertyValue = validator.GetValueForProperty(instance);
            PropertyInfo = validator.PropertyInfo;
            Parent = parentContext;
            Instance = instance;
            Level = validator.Level;
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
            : base(instance, validator, parentContext) {}

        public RuleValidatorContext(T instance, string propertyName, TProperty propertyValue, MemberInfo propertyInfo, ValidationLevelType level,
                                    RuleValidatorContext parentContext) : base(instance, propertyName, propertyValue, level, propertyInfo, parentContext ){}

        public new TProperty PropertyValue
        {
            get { return (TProperty) base.PropertyValue; }
            set { base.PropertyValue = value; }
        }

        public new T Instance
        {
            get { return (T) base.Instance; }
            set { base.Instance = value; }
        }
    }
}