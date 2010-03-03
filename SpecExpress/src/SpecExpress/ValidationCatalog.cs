using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SpecExpress.MessageStore;

namespace SpecExpress
{
    public static class ValidationCatalog<TContext> where TContext : ValidationContext, new()
    {
        #region Validate Object

        public static ValidationNotification Validate(object instance)
        {
            var context = new TContext();
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, null);
        }

        public static ValidationNotification Validate(object instance, Specification specification)
        {
            var context = new TContext();
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, specification);
        }

        public static ValidationNotification Validate<TSpec>(object instance) where TSpec : Specification, new()
        {
            var context = new TContext();
            var spec = new TSpec() as Specification;
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, spec);
        }

        public static SpecificationContainer SpecificationContainer
        {
            get { return new TContext().SpecificationContainer;}
        }

        #endregion

        #region Validate Property
        
        public static ValidationNotification ValidateProperty(object instance, string propertyName)
        {
           var context = new TContext();
           return ValidationCatalog.ValidateProperty(instance, propertyName, null, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty(object instance, string propertyName,
                                                              Specification specification)
        {
            var context = new TContext();
            return ValidationCatalog.ValidateProperty(instance, propertyName, specification, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property)
        {
            var context = new TContext();
            var prop = new PropertyValidator<T, object>(property);
            return ValidationCatalog.ValidateProperty(instance, prop.PropertyInfo.Name, null, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property,
                                                              Specification specification)
        {
            var context = new TContext();
            var prop = new PropertyValidator<T, object>(property);
            return ValidationCatalog.ValidateProperty(instance, prop.PropertyInfo.Name, specification, context.SpecificationContainer);
        }


        #endregion

        #region Assert Property

        public static void AssertValidProperty(object value, string propertyName,
                                                              Specification specification)
        {
            var context = new TContext();
            ValidationCatalog.AssertValidProperty(value, propertyName, specification, context.SpecificationContainer);
        }

        public static void AssertValidProperty<T, TSpec>(object value, Expression<Func<T, object>> property) where TSpec : Specification, new()
        {
            var spec = new TSpec() as Specification;
            var prop = new PropertyValidator<T, object>(property);
            ValidationCatalog.AssertValidProperty(value, prop.PropertyInfo.Name, spec);
        }

        public static void AssertValidProperty<T>(object value, Expression<Func<T, object>> property,
                                                              Specification specification)
        {
            var context = new TContext();
            var prop = new PropertyValidator<T, object>(property);
            ValidationCatalog.AssertValidProperty(value, prop.PropertyInfo.Name, specification, context.SpecificationContainer);

        }
        #endregion

    }

    public static class ValidationCatalog
    {
        private static object _syncLock = new object();
        
        public static bool ValidateObjectGraph { get; set; }
        public static ValidationCatalogConfiguration Configuration { get; private set;}
        public static  SpecificationContainer SpecificationContainer = new SpecificationContainer();

        static ValidationCatalog()
        {
            Configuration = buildDefaultValidationConfiguration();
        }

        #region Configuration
        /// <summary>
        /// Add Specifications dynamically without a SpecificationBase
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="rules"></param>
        public static void AddSpecification<TEntity>(Action<Validates<TEntity>> rules)
        {
            //Should these rules be "disposable"? ie, not added to registry?
            var specification = new SpecificationExpression<TEntity>();
            rules(specification);

            SpecificationContainer.Add(specification);
        }

        /// <summary>
        /// Configure the scanning of Assemblies containing Specifications used by Validate(object)
        /// </summary>
        /// <param name="configuration"></param>
        public static void Scan(Action<SpecificationScanner> configuration)
        {
            var specificationRegistry = new SpecificationScanner();
            configuration(specificationRegistry);
            SpecificationContainer.Add(specificationRegistry.FoundSpecifications);
        }
        
        public static void Configure(Action<ValidationCatalogConfiguration> action)
        {
            action(Configuration);
        }

        public static void Reset()
        {
            lock (_syncLock)
            {
                SpecificationContainer.Reset();
            }
        }

        public static void ResetConfiguration()
        {
            Configuration = buildDefaultValidationConfiguration();
        }

        

        public static void AssertConfigurationIsValid()
        {
            lock (_syncLock)
            {

                //Look for multiple specifications for a type where no default is defined.
                //TODO: Implement multispec check

                // RB 20091014: Allow a Property Validator with no rules defined to be valid (i.e. "Check(c => c.Name).Optional();" ).
                ////Look for PropertyValidators with no Rules
                //var invalidPropertyValidators = from r in _registry
                //                                from v in r.PropertyValidators
                //                                where v.Rules == null || !v.Rules.Any()
                //                                select
                //                                    r.GetType().Name + " is invalid because it has no rules defined for property '" +
                //                                    v.PropertyName + "'.";

                //if (invalidPropertyValidators.Any())
                //{
                //    var errorString = invalidPropertyValidators.Aggregate(string.Empty, (x, y) => x + "\n" + y);
                //    throw new SpecExpressConfigurationException(errorString);
                //}
            }
        }

        private static ValidationCatalogConfiguration buildDefaultValidationConfiguration()
        {
            lock (_syncLock)
            {
                var config = new ValidationCatalogConfiguration()
                {
                    DefaultMessageStore =
                        new ResourceMessageStore(
                        RuleErrorMessages.ResourceManager),
                    ValidateObjectGraph = false
                };
                return config;
            }
        }
        #endregion

        #region Object Validation

        internal static ValidationNotification Validate(object instance, SpecificationContainer container, Specification specification)
        {
            //Guard for null
            if (instance == null)
            {
                throw new ArgumentNullException("Validate requires a non-null instance.");
            }

            //Initialize Parameters if required
            if (container == null)
            {
                //Default container from ValidationCatalog
                container = SpecificationContainer;
            }


            if (specification == null)
            {
                specification = container.TryGetSpecification(instance.GetType());

                //Check if a Specification wasn't found for the Type
                if (specification == null)
                {
                    //No spec found for type, try for Collection
                    if (instance is IEnumerable)
                    {
                        return ValidateCollection((IEnumerable)instance, SpecificationContainer);
                    }
                    else
                    {
                        //Unable to find specification, so call GetSpecification to generate an error message
                        SpecificationContainer.GetSpecification(instance.GetType());
                        return null;
                    }
                }
            }

            //We've either found a valid Specification or we've thrown an exception
            //The Specification may have been explicitly defined
            //check if the Specification and instance type match up the use them
            if (specification.ForType == instance.GetType())
            {
                return new ValidationNotification { Errors = specification.Validate(instance, container) };
            }
            else
            {
                throw new SpecExpressConfigurationException("Specification is invalid for the instance. Specification is for type " + specification.ForType.ToString() + " and instance is type " + instance.GetType().ToString() + ".");
            }
        }

        /// <summary>
        /// Evaluate an object against it's matching Specification and returns any broken rules.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static ValidationNotification Validate(object instance)
        {
            return Validate(instance, null, null);
        }

        public static ValidationNotification Validate(object instance, Specification specification)
        {
            return Validate(instance, null, specification);
        }

        public static ValidationNotification Validate<TSpec>(object instance) where TSpec : Specification, new()
        {
            var spec = new TSpec() as Specification;
            return Validate(instance, null, spec);
        }


        #region ValidationContext
        //internal static ValidationNotification ValidateContext(object instance, ValidationContext context)
        //{
        //    //try to find a specification for the type
        //    Specification specification = context.SpecificationContainer.TryGetSpecification(instance.GetType());

        //    if (specification != null)
        //    {
        //        //Specification for this type found
        //        return Validate(instance, specification);
        //    }
        //    else
        //    {
        //        //No spec found for type, try for Collection
        //        if (instance is IEnumerable)
        //        {
        //            return ValidateCollection((IEnumerable)instance, context.SpecificationContainer);
        //        }
        //        else
        //        {
        //            //Unable to find specification, so call GetSpecification to generate an error message
        //            context.SpecificationContainer.GetSpecification(instance.GetType());
        //            return null;
        //        }
        //    }
        //}
        
        #endregion

        private static ValidationNotification ValidateCollection(IEnumerable instance, SpecificationContainer specificationContainer)
        {
            //assume that the first item in the collection is the same for all items in the collection and get the specification for that type
            IEnumerator enumerator = ((IEnumerable)instance).GetEnumerator();

            //move to the first item in the collection if it's not empty
            if (enumerator.MoveNext())
            {
                var specification = specificationContainer.GetSpecification(enumerator.Current.GetType());
                return ValidateCollection((IEnumerable)instance, specification, specificationContainer);
            }
            else
            {
                //Collection was empty, return default ValidationNotification
                return new ValidationNotification();
            }
        }

        private static ValidationNotification ValidateCollection(IEnumerable instance, Specification specification, SpecificationContainer specificationContainer)
        {
            //Guard for null
            if (instance == null)
            {
                throw new ArgumentNullException("Validate requires a non-null instance.");
            }

            var collectionResult = new List<ValidationResult>();

            //Object being validated is a collection.
            //Check if the type in the collection has a Specification
            IEnumerator enumerator = instance.GetEnumerator();

            while (enumerator.MoveNext())
            {
                //validate the object with the given specification
                collectionResult.AddRange(specification.Validate(enumerator.Current, specificationContainer));
            }

            return new ValidationNotification { Errors = collectionResult };
            
        }

       

        #endregion

        #region Property Validation

        public static ValidationNotification ValidateProperty(object instance, string propertyName)
        {
            return ValidateProperty(instance, propertyName, null);
        }

        public static ValidationNotification ValidateProperty(object instance, string propertyName,
                                                              Specification specification)
        {
            return ValidateProperty(instance, propertyName, specification, SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T,object>> property)
        {
            return ValidateProperty(instance, property, null);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property,
                                                              Specification specification)
        {
            var prop = new PropertyValidator<T, object>(property);
            return ValidateProperty(instance, prop.PropertyInfo.Name, specification, SpecificationContainer);
           
        }

        internal static ValidationNotification ValidateProperty(object instance, string propertyName, Specification specification, SpecificationContainer container)
        {
            if (specification == null)
            {
                specification = container.TryGetSpecification(instance.GetType());
            }

            var validators = from validator in specification.PropertyValidators
                             where validator.PropertyInfo!= null && validator.PropertyInfo.Name == propertyName
                             select validator;

            if (!validators.Any())
            {
                throw new ArgumentException(string.Format("There are no validation rules defined for {0}.{1}.", instance.GetType().FullName, propertyName));
            }

            var results =
                (from propertyValidator in validators
                 select propertyValidator.Validate(instance, container))
                .SelectMany(x => x)
                .ToList();

            return new ValidationNotification() { Errors = results };
        }

        #region Assert Property

        public static void AssertValidProperty(object value, string propertyName,
                                                              Specification specification)
        {
            AssertValidProperty(value, propertyName, specification, SpecificationContainer);
        }

        public static void AssertValidProperty<T, TSpec>(object value, Expression<Func<T, object>> property) where TSpec : Specification, new()
        {
            var spec = new TSpec() as Specification;
            var prop = new PropertyValidator<T, object>(property);
            AssertValidProperty(value, prop.PropertyInfo.Name, spec);
        }

        public static void AssertValidProperty<T>(object value, Expression<Func<T, object>> property,
                                                              Specification specification)
        {
            var prop = new PropertyValidator<T, object>(property);
            AssertValidProperty(value, prop.PropertyInfo.Name, specification, SpecificationContainer);

        }

        internal static void AssertValidProperty(object value, string propertyName, Specification specification, SpecificationContainer container)
        {
            if (specification == null)
            {
                throw new ArgumentException("Specification is required");
            }

            //Create a placeholder object for the type we are validating
            var proxy = Activator.CreateInstance(specification.ForType, true);

            //set the value on the proxy object
            specification.ForType.GetProperty(propertyName).SetValue(proxy, value, null);

            var vn = ValidateProperty(proxy, propertyName, specification, container);

            if (!vn.IsValid)
            {
                throw new ValidationException(vn);
            }

        }

        #endregion

        #endregion

    }

    
}