using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SpecExpress.DSL;
using SpecExpress.Enums;

namespace SpecExpress
{
   
    public abstract class Validates<T> : Specification
    {
        private Type _forType;

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
        /// Create a broken rule for this property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">Lambda expression ( c => c.FirstName ) returning the property to evaluate</param>
        /// <param name="propertyNameOverride">Name of property to use in related messages</param>
        /// <returns></returns>
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
        ///  
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">Lambda expression ( c => c.FirstName ) returning the property to evaluate</param>
        /// <returns></returns>
        public ActionOptionConditionBuilder<T, TProperty> Check<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return Check(expression, null);
        }

        public void Using<U, TSpecType>() where TSpecType : Validates<U>, new()
        {
            //Get the PropertyValidators from the Using Spec and add it to this specification
            var usingSpec = ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications().First( x => x.GetType() == typeof(TSpecType));

            PropertyValidators.AddRange(usingSpec.PropertyValidators);
        }

        #endregion

        #region Warn

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

        public ActionOptionConditionBuilder<T, TProperty> Warn<TProperty>(Expression<Func<T, TProperty>> expression)
        {
                return Warn(expression, null);
        }

        #endregion

        public List<ValidationResult> Validate(T instance)
        {
            lock (this)
            {
                return PropertyValidators.SelectMany(x => x.Validate(instance, null)).ToList();
            }
        }

        public List<ValidationResult> Validate(T instance, SpecificationContainer specificationContainer)
        {
            lock (this)
            {
                return PropertyValidators.SelectMany(x => x.Validate(instance, specificationContainer)).ToList();
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