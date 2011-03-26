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
using SpecExpress.RuleTree;

namespace SpecExpress
{
    public abstract class PropertyValidator
    {
        private string _propertyNameOverride;


        protected PropertyValidator(Type entityType, Type propertyType)
        {
            EntityType = entityType;
            PropertyType = propertyType;
        }

        public abstract bool Validate(object instance, SpecificationContainer specificationContainer, ValidationNotification notification);
        public abstract bool Validate(object instance, RuleValidatorContext parentRuleContexts, SpecificationContainer specificationContainer, ValidationNotification notification);

        public Type PropertyType { get; private set; }
        public Type EntityType { get; private set; }
        public Specification CustomSpecification { get; set; }

        public MemberInfo PropertyInfo
        {
            get
            {
                //ToDo: Are all bases covered for ExpressionType (MemberAccess / Call)?  What are we missing?
                var bodyExp = Property.Body;

                if (bodyExp.NodeType == ExpressionType.MemberAccess)
                {
                    return ((MemberExpression)(bodyExp)).Member;
                }

                if (bodyExp.NodeType == ExpressionType.Call)
                {
                    MethodCallExpression exp = (MethodCallExpression)Property.Body;

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



        public string PropertyNameOverride
        {
            get
            {
                return _propertyNameOverride;
            }

            set { _propertyNameOverride = value; }
        }



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

        public LambdaExpression Property { get; set; }

        public object GetValueForProperty(object instance)
        {
            if (instance == null)
            {
                return null;
            }

            try
            {
                return Property.Compile().DynamicInvoke(new[] { instance });
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

        public RuleValidator RequiredRule { get; protected set; }
        public bool PropertyValueRequired
        {
            get { return RequiredRule != null; }
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
        private Func<T, string> _propertyNameOverrideExpression;

        protected PropertyValidator(Type propertyType)
            : base(typeof(T), propertyType)
        {
        }

        public abstract bool Validate(T instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer, ValidationNotification notification);

        public bool Validate(T instance, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Validate(instance, null, specificationContainer, notification);
        }

        public override bool Validate(object instance, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Validate((T)instance, null, specificationContainer, notification);
        }

        public override bool Validate(object instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            return Validate((T)instance, parentRuleContext, specificationContainer, notification);
        }

        public Func<T, string> PropertyNameOverrideExpression
        {
            get
            {
                //a value was specified for PropertyNameOverride. Convert it to an expression so there's only one property to check
                //instead of two to get a custom property name
                if (!String.IsNullOrEmpty(PropertyNameOverride) && _propertyNameOverrideExpression == null)
                {
                    return new Func<T, string>(o => PropertyNameOverride);
                }
                else
                {
                    return _propertyNameOverrideExpression;
                }
            }
            set { _propertyNameOverrideExpression = value; }
        }

    }

    public class PropertyValidator<T, TProperty> : PropertyValidator<T>
    {
        private bool _propertyValueRequired = true;
        private RuleTree<T, TProperty> _ruleTree = new RuleTree<T, TProperty>();

        public PropertyValidator(Expression<Func<T, TProperty>> targetExpression)
            : base(targetExpression.Body.Type)
        {
            Property = targetExpression;
        }

        internal PropertyValidator(PropertyValidator<T, TProperty> parent)
            : base(parent.Property.Body.Type)
        {
            _propertyValueRequired = parent._propertyValueRequired;
            PropertyInfo = parent.PropertyInfo;
            PropertyNameOverride = parent.PropertyNameOverride;
            Property = parent.Property;
        }

        public bool ValueRequired
        {
            get
            {
                return _propertyValueRequired;
            }
            set
            {
                _propertyValueRequired = value;
            }
        }

        public RuleTree.RuleTree<T, TProperty> RuleTree
        {
            get { return _ruleTree; }
        }

        public Predicate<T> Condition { get; set; }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an Or condition
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void OrRule(RuleValidator ruleValidator)
        {
            OrRule(ruleValidator as RuleValidator<T, TProperty>);
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an OrElse condition
        /// where the ruleValidator is evaluated only if the prior rules in the tree
        /// evaluate to false.
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void ConditionalOrRule(RuleValidator ruleValidator)
        {
            ConditionalOrRule(ruleValidator as RuleValidator<T, TProperty>);
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an Or condition
        /// </summary>
        /// <param name="validator"><see cref="RuleValidator&lt;T, TProperty&gt;"/></param>
        public void OrRule(RuleValidator<T, TProperty> validator)
        {
            var node = new RuleNode<T, TProperty>(validator);
            if (RuleTree.Root == null)
            {
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.OrChild(node);
            }
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an OrElse condition
        /// where the ruleValidator is evaluated only if the prior rules in the tree
        /// evaluate to false.
        /// </summary>
        /// <param name="validator"><see cref="RuleValidator&lt;T, TProperty&gt;"/></param>
        public void ConditionalOrRule(RuleValidator<T, TProperty> validator)
        {
            var node = new RuleNode<T, TProperty>(validator);
            if (RuleTree.Root == null)
            {
                node.ChildRelationshipIsConditional = true;
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.ConditionalOrChild(node);
            }
        }

        /// <summary>
        /// Add the RuleTree defined in innerPropertyValidator to the RuleTree as a group
        /// using an OrElse condition 
        /// </summary>
        /// <param name="innerPropertyValidator"><see cref="PropertyValidator&lt;T, TProperty&gt;"/></param>
        public void OrGroup(PropertyValidator<T, TProperty> innerPropertyValidator)
        {
            var node = new GroupNode<T, TProperty>(innerPropertyValidator._ruleTree.Root);
            if (RuleTree.Root == null)
            {
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.OrChild(node);
            }
        }

        /// <summary>
        /// Add the RuleTree defined in innerPropertyValidator to the RuleTree as a group
        /// using an Or condition where the ruleValidator is evaluated only if the
        /// prior rules in the tree evaluate to false.
        /// </summary>
        /// <param name="innerPropertyValidator"><see cref="PropertyValidator&lt;T, TProperty&gt;"/></param>
        public void ConditionalOrGroup(PropertyValidator<T, TProperty> innerPropertyValidator)
        {
            var node = new GroupNode<T, TProperty>(innerPropertyValidator._ruleTree.Root);
            if (RuleTree.Root == null)
            {
                node.ChildRelationshipIsConditional = true;
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.ConditionalOrChild(node);
            }
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an And condition
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void AndRule(RuleValidator ruleValidator)
        {
            AndRule(ruleValidator as RuleValidator<T, TProperty>);
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an AndAlso condition
        /// where the ruleValidator will only be evaluated if the prior rules
        /// in the tree evaluate to true.
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void ConditionalAndRule(RuleValidator ruleValidator)
        {
            ConditionalAndRule(ruleValidator as RuleValidator<T, TProperty>);
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an And condition
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void AndRule(RuleValidator<T, TProperty> ruleValidator)
        {
            var node = new RuleNode<T, TProperty>(ruleValidator);
            if (RuleTree.Root == null)
            {
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.AndChild(node);
            }
        }

        /// <summary>
        /// Add the RuleValidator to the RuleTree using an AndAlso condition
        /// where the ruleValidator will only be evaluated if the prior rules
        /// in the tree evaluate to true.
        /// </summary>
        /// <param name="ruleValidator"><see cref="RuleValidator"/></param>
        public void ConditionalAndRule(RuleValidator<T, TProperty> ruleValidator)
        {
            var node = new RuleNode<T, TProperty>(ruleValidator);
            if (RuleTree.Root == null)
            {
                node.ChildRelationshipIsConditional = true;
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.ConditionalAndChild(node);
            }

            //Check if the rule is required
            if (ruleValidator.GetType() == typeof(Required<T, TProperty>))
            {
                RequiredRule = ruleValidator;
            }
        }

        /// <summary>
        /// Add the RuleTree defined in innerPropertyValidator to the RuleTree as a group
        /// using an And condition where the ruleValidator will only be evaluated if 
        /// the prior rules in the tree evaluate to true.
        /// </summary>
        /// <param name="innerPropertyValidator"><see cref="PropertyValidator&lt;T, TProperty&gt;"/></param>
        public void AndGroup(PropertyValidator<T, TProperty> innerPropertyValidator)
        {
            var node = new GroupNode<T, TProperty>(innerPropertyValidator._ruleTree.Root);
            if (RuleTree.Root == null)
            {
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.AndChild(node);
            }
        }

        /// <summary>
        /// Add the RuleTree defined in innerPropertyValidator to the RuleTree as a group
        /// using an AndAlso condition
        /// </summary>
        /// <param name="innerPropertyValidator"><see cref="PropertyValidator&lt;T, TProperty&gt;"/></param>
        public void ConditionalAndGroup(PropertyValidator<T, TProperty> innerPropertyValidator)
        {
            var node = new GroupNode<T, TProperty>(innerPropertyValidator._ruleTree.Root);
            if (RuleTree.Root == null)
            {
                RuleTree.Root = node;
            }
            else
            {
                RuleTree.EndNode.ConditionalAndChild(node);
            }
        }

        public override bool Validate(T instance, RuleValidatorContext parentRuleContext, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            var context = new RuleValidatorContext<T, TProperty>(instance, this, parentRuleContext);
            if ((_propertyValueRequired || !context.PropertyValue.IsNullOrDefault()))
            {
                if (Condition == null || (Condition != null && Condition.Try(instance)))
                    //if (Condition == null || (Condition != null && Condition(instance)))
                {

                    var tmpNotification = new ValidationNotification();
                    bool isValid = RuleTree.LambdaExpression(context, specificationContainer, tmpNotification);

                    // if lambda is valid but notification returns with errors, there may have been an Or rule that failed
                    // so we need to clear out the notification errors.
                    if (!isValid || !(isValid && !tmpNotification.IsValid))
                    {
                        notification.Errors.AddRange(tmpNotification.Errors);
                    }

                    if (ValidationCatalog.ValidateObjectGraph)
                    {
                        //Check if this Property Type has a Registered specification to validate with and the instance of the property
                        //isn't already invalid. For example if a property is required and the object is null, then 
                        //don't continue validating the object
                        ValidateObjectGraph(context, specificationContainer, notification);
                    }
                }
            }
            return notification.IsValid;
        }

        private void ValidateObjectGraph(RuleValidatorContext<T, TProperty> context, SpecificationContainer specificationContainer, ValidationNotification notification)
        {
            if (!notification.Errors.Any() && specificationContainer.TryGetSpecification(typeof(TProperty)) != null)
            {
                //Spec found, use it to validate
                Specification specification = specificationContainer.GetSpecification(typeof(TProperty));
                //Add any errors to the existing list of errors
                foreach (var validator in specification.PropertyValidators)
                {
                    validator.Validate(context.PropertyValue, context, specificationContainer, notification);
                }

                //notification.Errors.AddRange(
                //    specification.PropertyValidators.SelectMany(x => x.Validate(context.PropertyValue, context, specificationContainer)).ToList());
            }

            //Validate each item in a Collection if a registered specification is found
            //if there aren't already errors, the value is a collection and it's not a string, then iterate over
            //each item, looking for a registered specification
            if (!notification.Errors.Any() && context.PropertyValue is IEnumerable && !(context.PropertyValue is string))
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
                        foreach (var validator in specification.PropertyValidators)
                        {
                            validator.Validate(enumerator.Current, context, specificationContainer, notification);
                        }
                    }
                }
            }
        }
    }
}