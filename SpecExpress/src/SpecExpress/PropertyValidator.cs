using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SpecExpress.Enums;
using SpecExpress.Rules;
using SpecExpress.Rules.GeneralValidators;
using SpecExpress.Util;

namespace SpecExpress
{
    public abstract class PropertyValidator
    {
        protected PropertyValidator(Type entityType, Type propertyType)
        {
            EntityType = entityType;
            PropertyType = propertyType;
        }


        public abstract void AddRule(RuleValidator ruleValidator);
        public abstract List<ValidationResult> Validate(object instance, SpecificationContainer specificationContainer);
        public abstract List<ValidationResult> Validate(object instance, RuleValidatorContext parentRuleContexts, SpecificationContainer specificationContainer);
        public abstract List<RuleValidator> Rules { get; }

        public Type PropertyType { get; private set; }
        public Type EntityType { get; private set; }
        public Specification CustomSpecification { get; set; }

        public MemberInfo PropertyInfo
        {
            get
            {
                //ToDo: Are all bases covered for ExpressionType (MemberAccess / Call)?  What are we missing?
                var bodyExp = Property.Body;

                if (bodyExp.NodeType == ExpressionType.MemberAccess )
                {
                    return ((MemberExpression) (bodyExp)).Member;
                }

                if (bodyExp.NodeType == ExpressionType.Call)
                {
                    MethodCallExpression exp = (MethodCallExpression) Property.Body;

                    //GetValueOrDefault
                    if (exp.Object == null)
                    {
                        return GetFirstMemberCallFromCallArguments(exp);
                    }
                    else
                    {
                        return ((System.Linq.Expressions.MemberExpression)(exp.Object)).Member;
                    }
                }

                return null;
            }

            protected set { }
        }

        public string PropertyNameOverride { get; set; }
        public string PropertyName
        {
            get
            {  
                if (!String.IsNullOrEmpty(PropertyNameOverride))
                {
                    return PropertyNameOverride;
                }

                Expression body = Property.Body;
                var propertyNameNode = new List<string>();

                //The expression is a function, so use the return value type as the Property Name
                //ie, Contacts.First() would return Contact
                if (body.NodeType == ExpressionType.Call)
                {   
                    propertyNameNode.Add(body.Type.Name); 
                }
                else
                {
                    //Expression is a list of Properties, so get each into a string List
                    while (body is MemberExpression)
                    {
                        var member = ((MemberExpression)(body)).Member;
                        propertyNameNode.Add(member.Name);
                        body = ((MemberExpression)(body)).Expression;
                    }
                }

                return propertyNameNode.ToReverseString();
            }
        }

        public ValidationLevelType Level { get; set; }
        public virtual bool PropertyValueRequired { get; set; }

        public abstract RuleValidator RequiredRule { get;}


        public PropertyValidator Child { get; set; }
        public PropertyValidator Parent { get; set; }
        public LambdaExpression Property { get; set; }
        
        public object GetValueForProperty(object instance)
        {
            if (instance == null)
            {
                return null;
            }

            try
            {
                return Property.Compile().DynamicInvoke(new[] {instance});
            }
            catch (TargetInvocationException err)
            {
                if (err.InnerException is NullReferenceException || err.InnerException is ArgumentNullException)
                {
                    return null;  
                }
                else
                {
                    throw;
                }
            }
        }

        private MemberInfo GetFirstMemberCallFromCallArguments(MethodCallExpression exp)
        {
            foreach (var argument in exp.Arguments)
            {
                if (argument.NodeType == ExpressionType.MemberAccess)
                {
                    return ((MemberExpression)(argument)).Member;
                    break;
                }
                else if (argument.NodeType == ExpressionType.Call)
                {
                    MemberInfo info = GetFirstMemberCallFromCallArguments(argument as MethodCallExpression);
                    if (info != null)
                    {
                        return info;
                    }
                }
            }
            return null;
        }
    }

    public abstract class PropertyValidator<T> : PropertyValidator
    {
        protected PropertyValidator(Type propertyType)
            : base(typeof(T), propertyType)
        {
        }

        public abstract List<ValidationResult> Validate(T instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer);

        public List<ValidationResult> Validate(T instance, SpecificationContainer specificationContainer)
        {
            return Validate(instance, null, specificationContainer);
        }

        public override List<ValidationResult> Validate(object instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer)
        {
            return Validate((T) instance, parentRuleContext, specificationContainer);
        }

        public override List<ValidationResult> Validate(object instance, SpecificationContainer specificationContainer)
        {
            return Validate((T) instance, null, specificationContainer);
        }
    }

    public class PropertyValidator<T, TProperty> : PropertyValidator<T>
    {
        private readonly bool _propertyValueRequired;
        private List<RuleValidator<T, TProperty>> _rules = new List<RuleValidator<T, TProperty>>();

        public PropertyValidator(Expression<Func<T, TProperty>> targetExpression)
            : base(targetExpression.Body.Type)
        {
            Property = targetExpression;
        }

        internal PropertyValidator(PropertyValidator<T, TProperty> parent)
            : base(parent.Property.Body.Type)
        {
            Parent = parent;
            _propertyValueRequired = Parent._propertyValueRequired;
            PropertyInfo = Parent.PropertyInfo;
            PropertyNameOverride = Parent.PropertyNameOverride;
            Property = parent.Property;
        }

        public override List<RuleValidator> Rules
        {
            get { return _rules.Select(x => x as RuleValidator).ToList(); }
        }

        public Predicate<T> Condition { get; set; }

        public override bool PropertyValueRequired
        {
            get { return base.PropertyValueRequired; }
            set
            {
                // Ensure only one Required rule is in list by Removing all Required Rules
                //Alan: Required<T, TProperty> can be moved to base class and the type genrically constructed and created
                List<RuleValidator<T, TProperty>> newList =
                    new List<RuleValidator<T, TProperty>>(from validator in _rules
                                                          where validator.GetType() != typeof (Required<T, TProperty>)
                                                          select validator);

                _rules = newList;

                if (value)
                {
                    // Add Required Rule as first rule if RropertyValueRequired set to true
                    _rules.Insert(0, new Required<T, TProperty>());
                }

                base.PropertyValueRequired = value;
            }
        }

        public override RuleValidator RequiredRule
        { 
            get
            {
                return _rules.OfType<Required<T, TProperty>>().FirstOrDefault();
            }

        }

        public new PropertyValidator<T, TProperty> Child
        {
            get { return (PropertyValidator<T, TProperty>) base.Child; }
            set { base.Child = value; }
        }

        public new PropertyValidator<T, TProperty> Parent
        {
            get { return (PropertyValidator<T, TProperty>) base.Parent; }
            set { base.Parent = value; }
        }

        public override void AddRule(RuleValidator ruleValidator)
        {
            _rules.Add(ruleValidator as RuleValidator<T, TProperty>);
        }

        public void AddRule(RuleValidator<T, TProperty> ruleValidator)
        {
            _rules.Add(ruleValidator);
        }

        public override List<ValidationResult> Validate(T instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer)
        {
            // RB 20091014: Allow a Property Validator with no rules defined to be valid (i.e. "Check(c => c.Name).Optional();" ).
            //if (_rules == null || !_rules.Any())
            //{
            //    throw new SpecExpressConfigurationException(
            //        "No rules exist for this Property" + PropertyName + " for type " + typeof(T).Name + ". This is because the rules are improperly configured.");
            //}

            if ( Condition == null || (Condition != null && Condition(instance)))
            {

                var context = new RuleValidatorContext<T, TProperty>(instance, this, parentRuleContext);

                if (PropertyValueRequired || (!PropertyValueRequired && !context.PropertyValue.IsNullOrDefault()))
                {
                    List<ValidationResult> list;

                    //If Property is Required and the Required Rule fails, short-circuit the remaining rules
                    if (PropertyValueRequired && context.PropertyValue.IsNullOrDefault())
                    {
                        //Required failed, return only a list of broken required rules
                        list =  
                            _rules.OfType<Required<T, TProperty>>().Select(rule => rule.Validate(context, specificationContainer)).Where(
                                result => result != null).ToList();
                    }
                    else
                    {
                        list = _rules.Select(rule => rule.Validate(context, specificationContainer)).Where(result => result != null).ToList();

                        if (ValidationCatalog.ValidateObjectGraph)
                        {
                            //Check if this Property Type has a Registered specification to validate with and the instance of the property
                            //isn't already invalid. For example if a property is required and the object is null, then 
                            //don't continue validating the object
                            ValidateObjectGraph(context, list, specificationContainer);
                        }

                        // If there is an "_or" ValidationExpression and if it validates fine, then clear list, else, add notifications to list.
                        if (list.Any() && Child != null)
                        {
                            List<ValidationResult> orList = Child.Validate(instance, specificationContainer);
                            if (orList.Any())
                            {
                                list.AddRange(orList);
                            }
                            else
                            {
                                list.Clear();
                            }
                        }
                    }
                    

                    return list;

                }
                else
                {
                    return new List<ValidationResult>();
                }
            }

            return new List<ValidationResult>();
        }

        private void ValidateObjectGraph(RuleValidatorContext<T, TProperty> context, List<ValidationResult> list, SpecificationContainer specificationContainer)
        {
            if (!list.Any() && specificationContainer.TryGetSpecification(typeof(TProperty)) != null)
            {
                //Spec found, use it to validate
                Specification specification = specificationContainer.GetSpecification(typeof(TProperty));
                //Add any errors to the existing list of errors
                list.AddRange(
                    specification.PropertyValidators.SelectMany(x => x.Validate(context.PropertyValue, context, specificationContainer)).ToList());
            }

            //Validate each item in a Collection if a registered specification is found
            //if there aren't already errors, the value is a collection and it's not a string, then iterate over
            //each item, looking for a registered specification
            if (!list.Any() && context.PropertyValue is IEnumerable && !(context.PropertyValue is string))
            {
                //Object being validated is a collection.
                //Check if the type in the collection has a Specification
                IEnumerator enumerator = ((IEnumerable)context.PropertyValue).GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (specificationContainer.TryGetSpecification(enumerator.Current.GetType()) != null)
                    {
                        //Spec found, use it to validate
                        var specification = specificationContainer.GetSpecification(enumerator.Current.GetType());
                        //Add any errors to the existing list of errors
                        list.AddRange(
                            specification.PropertyValidators.SelectMany(x => x.Validate(enumerator.Current, context, specificationContainer)).ToList());
                    }
                }
            }
        }
    }
}