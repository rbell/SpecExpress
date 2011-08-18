using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SpecExpress.MessageStore;
using SpecExpress.Util;

namespace SpecExpress
{
    /// <summary>
    /// Validate an object using a ValidationContext
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public static class ValidationCatalog<TContext> where TContext : ValidationContext, new()
    {
        #region Validate Object

        public static ValidationNotification Validate(object instance)
        {
            var context = new TContext();
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, null);
        }

        public static ValidationNotification Validate(object instance, SpecificationBase specificationBase)
        {
            var context = new TContext();
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, specificationBase);
        }

        public static ValidationNotification Validate<TSpec>(object instance) where TSpec : SpecificationBase, new()
        {
            var context = new TContext();
            var spec = context.SpecificationContainer.TryGetSpecification<TSpec>() ?? new TSpec() as SpecificationBase;
            return ValidationCatalog.Validate(instance, context.SpecificationContainer, spec);
        }

        public static SpecificationContainer SpecificationContainer
        {
            get { return new TContext().SpecificationContainer; }
        }

        #endregion

        #region Validate Property
        public static ValidationNotification ValidateProperty(object instance, string propertyName)
        {
            var context = new TContext();
            return ValidationCatalog.ValidateProperty(instance, propertyName, null, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty(object instance, string propertyName,
                                                              SpecificationBase specificationBase)
        {
            var context = new TContext();
            return ValidationCatalog.ValidateProperty(instance, propertyName, specificationBase, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property)
        {
            var context = new TContext();
            var prop = new PropertyValidator<T, object>(property);
            return ValidationCatalog.ValidateProperty(instance, prop.PropertyInfo.Name, null, context.SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property,
                                                              SpecificationBase specificationBase)
        {
            var context = new TContext();
            var prop = new PropertyValidator<T, object>(property);
            return ValidationCatalog.ValidateProperty(instance, prop.PropertyInfo.Name, specificationBase, context.SpecificationContainer);
        }


        #endregion

    }

    public static class ValidationCatalog
    {
        private static object _syncLock = new object();

        public static bool ValidateObjectGraph { get; set; }
        public static ValidationCatalogConfiguration Configuration { get; private set; }
        public static SpecificationContainer SpecificationContainer = new SpecificationContainer();

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
            lock (_syncLock)
            {
                //Should these rules be "disposable"? ie, not added to registry?
                var specification = new SpecificationExpression<TEntity>();
                rules(specification);

                SpecificationContainer.Add(specification);
            }
        }


        public static void AddSpecification<TSpec>() where TSpec : SpecificationBase, new()
        {
            lock (_syncLock)
            {
                SpecificationContainer.Add<TSpec>();
            }
        }
            
        /// <summary>
        /// Configure the scanning of Assemblies containing Specifications used by Validate(object)
        /// </summary>
        /// <param name="configuration"></param>
        public static void Scan(Action<SpecificationScanner> configuration)
        {
            lock(_syncLock)
            {
                var specificationRegistry = new SpecificationScanner();
                configuration(specificationRegistry);
                SpecificationContainer.Add(specificationRegistry.FoundSpecifications);

                if (!ValidationCatalog.SpecificationContainer.GetAllSpecifications().Any())
                {
                    throw new SpecExpressConfigurationException("No specifications are registered with ValidationCatalog. Check if Scan has been run.");
                }  
            }
        }

        public static void Configure(Action<ValidationCatalogConfiguration> action)
        {
            lock (_syncLock)
            {
                action(Configuration);
            }
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
            lock (_syncLock)
            {
                Configuration = buildDefaultValidationConfiguration();
            }
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

                if (!ValidationCatalog.SpecificationContainer.GetAllSpecifications().Any())
                {
                    throw new SpecExpressConfigurationException("No specifications are registered with ValidationCatalog. Check if Scan has been run.");
                }


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

        internal static ValidationNotification Validate(object instance, SpecificationContainer container, SpecificationBase specificationBase)
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


            if (specificationBase == null)
            {
                specificationBase = container.TryGetSpecification(instance.GetType());

                //Check if a Specification wasn't found for the Type
                if (specificationBase == null)
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


            if (instance.GetType().CanBeCastTo(specificationBase.ForType))
            {
                var notification = new ValidationNotification();
                specificationBase.Validate(instance, container, notification);
                return notification;
            }
            else
            {
                throw new SpecExpressConfigurationException("Specification is invalid for the instance. Specification is for type " + specificationBase.ForType.ToString() + " and instance is type " + instance.GetType().ToString() + ".");
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

        public static ValidationNotification Validate(object instance, SpecificationBase specificationBase)
        {
            return Validate(instance, null, specificationBase);
        }

        public static ValidationNotification Validate<TSpec>(object instance) where TSpec : SpecificationBase, new()
        {
            var spec = SpecificationContainer.GetAllSpecifications()
                .FirstOrDefault(s => s.GetType() == typeof(TSpec)) ?? new TSpec() as SpecificationBase;

            return Validate(instance, SpecificationContainer, spec);
        }

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

        private static ValidationNotification ValidateCollection(IEnumerable instance, SpecificationBase specificationBase, SpecificationContainer specificationContainer)
        {
            //Guard for null
            if (instance == null)
            {
                throw new ArgumentNullException("Validate requires a non-null instance.");
            }

            //Object being validated is a collection.
            //Check if the type in the collection has a Specification
            IEnumerator enumerator = instance.GetEnumerator();

            var notification = new ValidationNotification();
            while (enumerator.MoveNext())
            {
                //validate the object with the given specification
                specificationBase.Validate(enumerator.Current, specificationContainer, notification);
            }
            return notification;
        }

        #endregion

        #region Property Validation

        public static ValidationNotification ValidateProperty(object instance, string propertyName)
        {
            return ValidateProperty(instance, propertyName, null);
        }

        public static ValidationNotification ValidateProperty(object instance, string propertyName,
                                                                SpecificationBase specificationBase)
        {
            return ValidateProperty(instance, propertyName, specificationBase, SpecificationContainer);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property)
        {
            return ValidateProperty(instance, property, null);
        }

        public static ValidationNotification ValidateProperty<T>(T instance, Expression<Func<T, object>> property,
                                                                SpecificationBase specificationBase)
        {
            var prop = new PropertyValidator<T, object>(property);
            return ValidateProperty(instance, prop.PropertyInfo.Name, specificationBase, SpecificationContainer);

        }

        internal static ValidationNotification ValidateProperty(object instance, string propertyName, SpecificationBase specificationBase, SpecificationContainer container)
        {
            if (specificationBase == null)
            {
                specificationBase = container.TryGetSpecification(instance.GetType());
            }

            var validators = from validator in specificationBase.PropertyValidators
                                where validator.PropertyInfo != null && validator.PropertyInfo.Name == propertyName
                                select validator;

            if (!validators.Any())
            {
                throw new ArgumentException(string.Format("There are no validation rules defined for {0}.{1}.", instance.GetType().FullName, propertyName));
            }

            var notification = new ValidationNotification();

            foreach (var validator in validators)
            {
                validator.Validate(instance, container, notification);
            }
      
            return notification;
        }

        #endregion

    }

}