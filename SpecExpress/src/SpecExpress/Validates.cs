using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SpecExpress.DSL;
using SpecExpress.Enums;

namespace SpecExpress
{
    /// <summary>
    /// The base class used to define a specification for a given type.
    /// </summary>
    /// <remarks>
    /// This type should be inherited from when defining a specification for type T.
    /// </remarks>
    /// <example>
    /// public class CustomerSpecification : Validates&lt;Customer&gt;
    /// {
    ///     public CustomerSpecification
    ///     {
    ///         Check(c => c.Name).Required().MaxLength(100);
    ///     }
    /// }
    /// </example>
    /// <typeparam name="T">The type to validate.</typeparam>
    public abstract class Validates<T> : Specification
    {
        private Type _forType;
        
        /// <summary>
        /// Returns the type T that is being validated.
        /// </summary>
        /// <remarks>
        /// This property will search the class hierarchy for the class inheriting Validates&lt;T&gt; and return the type of T.
        /// </remarks>
        public override Type ForType
        {
            get
            {
                if (_forType == null)
                {
                    bool found = false;
                    Type currentType = this.GetType();
                    
                    while(!found)
                    {
                        //Look for base class by name
                        if (currentType.BaseType.Name == "Validates`1")
                        {
                            found = true;
                        }
                        else
                        {
                            currentType = currentType.BaseType;
                        }
                    }

                    //not found in this class, so look in the base class
                    _forType = currentType.BaseType.GetGenericArguments().FirstOrDefault();
                    
                }
                return _forType;
            }
        }

        #region Check

        /// <summary>
        /// Defines a set of rules for a property of T as expressed in the expression parameter.  The name of the property
        /// specified in the propertyNameOverride property.  The set of rules will be enforced in order for the specification
        /// to be valid.
        /// </summary>
        /// <typeparam name="T">The type of entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="expression">Lambda expression returning the property to evaluate (i.e. "c => c.FirstName").</param>
        /// <param name="propertyNameOverride">Name of property to use in resulting error messages.</param>
        /// <returns><see cref="ActionOptionConditionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionConditionBuilder<T, TProperty> Check<TProperty>(Expression<Func<T, TProperty>> expression,
                                                                  string propertyNameOverride)
        {
            lock (this)
            {
                PropertyValidator<T, TProperty> validator = registerValidator(expression);
                validator.PropertyNameOverride = propertyNameOverride;
                validator.Level = ValidationLevelType.Error;
                return new ActionOptionConditionBuilder<T, TProperty>(validator);
            }
        }

        /// <summary>
        /// Defines a set of rules for a property of T as expressed in the expression parameter. The name of the property
        /// specified by a <see cref="Func&lt;T,string&gt;"/> passed in to the propertyNameOverride property.  The set of 
        /// rules will be enforced in order for the specification to be valid.
        /// </summary>
        /// <typeparam name="T">The type of entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="expression">Lambda expression returning the property to evaluate (i.e. "c => c.FirstName").</param>
        /// <param name="propertyNameOverride">A <see cref="Func&lt;T,string&gt;"/> that results in the name of the property 
        /// to use in resulting error messages</param>
        /// <returns><see cref="ActionOptionConditionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionConditionBuilder<T, TProperty> Check<TProperty>(Expression<Func<T, TProperty>> expression,
                                                                 Func<T, string> propertyNameOverride)
        {
            lock (this)
            {
                PropertyValidator<T, TProperty> validator = registerValidator(expression);


                validator.PropertyNameOverrideExpression = propertyNameOverride;
                validator.Level = ValidationLevelType.Error;
                return new ActionOptionConditionBuilder<T, TProperty>(validator);
            }
        }

        /// <summary>
        /// Defines a set of rules for a property of T as expressed in the expression parameter. The set of rules will be 
        /// enforced in order for the specification to be valid.
        /// </summary>
        /// <typeparam name="T">The type of entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="expression">Lambda expression returning the property to evaluate (i.e. "c => c.FirstName").</param>
        /// <returns><see cref="ActionOptionConditionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionConditionBuilder<T, TProperty> Check<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return Check(expression, null as string); //need to cast null to a type to know which overload to use
        }

        #endregion

        #region Warn
        /// <summary>
        /// Defines a set of rules for a property of T as expressed in the expression parameter.  The name of the property
        /// specified in the propertyNameOverride property.  The results of the rules will be treated as warnings and will
        /// not make the specification invalid.
        /// </summary>
        /// <typeparam name="T">The type of entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="expression">Lambda expression returning the property to evaluate (i.e. "c => c.FirstName").</param>
        /// <param name="propertyNameOverride">Name of property to use in resulting error messages.</param>
        /// <returns><see cref="ActionOptionConditionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionConditionBuilder<T, TProperty> Warn<TProperty>(Expression<Func<T, TProperty>> expression,
                                                                 string propertyNameOverride)
        {
            lock (this)
            {
                PropertyValidator<T, TProperty> validator = registerValidator(expression);
                validator.Level = ValidationLevelType.Warn;
                validator.PropertyNameOverride = propertyNameOverride;
                return new ActionOptionConditionBuilder<T, TProperty>(validator);
            }
        }

        /// <summary>
        /// Defines a set of rules for a property of T as expressed in the expression parameter.  The name of the property
        /// specified in the propertyNameOverride property.  The results of the rules will be treated as warnings and will
        /// not make the specification invalid.
        /// </summary>
        /// <typeparam name="T">The type of entity being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="expression">Lambda expression returning the property to evaluate (i.e. "c => c.FirstName").</param>
        /// <returns><see cref="ActionOptionConditionBuilder&lt;T, TProperty&gt;"/></returns>
        public ActionOptionConditionBuilder<T, TProperty> Warn<TProperty>(Expression<Func<T, TProperty>> expression)
        {
                return Warn(expression, null);
        }

        #endregion

        /// <summary>
        /// Ensures that when validating an instance of T, that another specification also be enforced.
        /// </summary>
        /// <typeparam name="U">The type of the object the specification to enforce is validating.</typeparam>
        /// <typeparam name="TSpecType">The type of the specification to enforce.</typeparam>
        public void Using<U, TSpecType>() where TSpecType : Validates<U>, new()
        {
            //Get the PropertyValidators from the Using Spec and add it to this specification
            var usingSpec = ValidationCatalog.SpecificationContainer.GetAllSpecifications().First( x => x.GetType() == typeof(TSpecType));

            PropertyValidators.AddRange(usingSpec.PropertyValidators);
        }

        /// <summary>
        /// Validates an instance of T against the rules defined in the specification.
        /// </summary>
        /// <param name="instance">Instance of T to validate.</param>
        /// <returns><see cref="ValidationNotification"/></returns>
        public ValidationNotification Validate(T instance)
        {
            lock (this)
            {
                var notification = new ValidationNotification();
                foreach (var propertyValidator in PropertyValidators)
                {
                    propertyValidator.Validate(instance, null, notification);
                }
                return notification;
            }
        }

        /// <summary>
        /// Validates an instance of T against the rules defined in the specification and a SpecificationContainer.
        /// </summary>
        /// <remarks>
        /// This is useful when using a specification leverages other specifications for referenced types.  The referenced
        /// types will be validated against their specifications contained in the SpecificationContainer.
        /// </remarks>
        /// <param name="instance">Instance of T to validate.</param>
        /// <param name="specificationContainer">The <see cref="SpecificationContainer"/></param>
        /// <returns><see cref="ValidationNotification"/></returns>
        public ValidationNotification Validate(T instance, SpecificationContainer specificationContainer)
        {
            lock (this)
            {
                var notification = new ValidationNotification();
                PropertyValidators.Select(x => x.Validate(instance, specificationContainer, notification));
                return notification;
            }
        }

        private PropertyValidator<T, TProperty> registerValidator<TProperty>(Expression<Func<T, TProperty>> expression)
        {
                var propertyValidator = new PropertyValidator<T, TProperty>(expression);
                PropertyValidators.Add(propertyValidator);
                return propertyValidator;
        }
    }
}