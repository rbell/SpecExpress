using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Util;

namespace SpecExpress
{
    public class SpecificationContainer
    {
        private List<Specification> _registry = new List<Specification>();

        public void Add<TEntity>(Validates<TEntity> expression)
        {
            Add((Specification) expression);
        }

        public void Add(Specification spec)
        {
            if (spec != null)
            {
                _registry.Add(spec);
            }
        }

        public void Add<TSpec>() where TSpec : Specification, new()
        {
            _registry.Add(new TSpec());
        }

        public void Add(IEnumerable<Specification> specs)
        {
            if (specs != null)
            {
                _registry.AddRange(specs);
            }
        }

        public void Add(IEnumerable<Type> specs)
        {
            int counter = 0;
            int max = 10;

            var delayedSpecs = createAndRegisterSpecificationsWithRegistryIterator(specs, counter, max);

            while (delayedSpecs.Any())
            {
                counter++;
                delayedSpecs = createAndRegisterSpecificationsWithRegistryIterator(specs, counter, max);
            }

        }

        public void Reset()
        {
            _registry.Clear();
        }

        #region Container

        public Specification TryGetSpecification(Type type)
        {
            //Attempt to find Specification where the Types are equal
           var specs = from r in _registry
                                where type == r.ForType
                                select r;

            //If nothing was found, then try where the specification type can be cast to the type
            if (specs.IsNullOrDefault())
            {
                specs = from r in _registry
                            where type.CanBeCastTo(r.ForType)
                            select r;
            }
           

            //TODO: Join with Validation Catalog Registry

            //No Specs found for type
            if (!specs.ToList().Any())
            {
                return null;
            }

            //If more than one spec was found for type
            if (specs.ToList().Count > 1)
            {
                //try to return the default
                var defaultSpecs = from s in specs
                                   where s.DefaultForType
                                   select s;

                //No default specs defined
                if (!defaultSpecs.Any())
                {
                    throw new SpecExpressConfigurationException("Multiple Specifications found and none are defined as default.");
                }

                //Multiple specs defined as Default
                if (defaultSpecs.Count() > 1)
                {
                    throw new SpecExpressConfigurationException("Multiple Specifications found and multiple are defined as default.");
                }

                return defaultSpecs.First();
            }

            return specs.First();
        
        }

        public Specification GetSpecification(Type type)
        {
            var spec = TryGetSpecification(type);

            if (spec == null)
            {
                throw new SpecExpressConfigurationException("No Specification for type " + type + " was found.");
            }

            return spec;
        }

        public Validates<TType> GetSpecification<TType>()
        {
            return GetSpecification(typeof(TType)) as Validates<TType>;
        }

        public Validates<TType> TryGetSpecification<TType>()
        {
            return TryGetSpecification(typeof(TType)) as Validates<TType>;
        }

        public IList<Specification> GetAllSpecifications()
        {
            // For thread safety, return a copy of the registry
            return new List<Specification>(_registry);
        }

        #endregion

        private List<Type> createAndRegisterSpecificationsWithRegistryIterator(IEnumerable<Type> specs, int counter, int max)
        {
            //TODO: This can result in a stackoverflow if a ForEachSpecification<Type> never finds a default spec for Type

            var delayedSpecs = new List<Type>();


            //For each type, instantiate it and add it to the collection of specs found
            specs.ToList<Type>().ForEach(spec =>
            {
                // Prevent two of the same specification from being registered
                if (!(from specification in _registry where specification.GetType().FullName == spec.FullName select specification).Any())
                {
                    try
                    {
                        var s = Activator.CreateInstance(spec) as Specification;

                        Add(s);
                    }
                    catch (System.Reflection.TargetInvocationException te)
                    {
                        if (counter > max)
                        {
                            throw new SpecExpressConfigurationException(
                                string.Format("Exception thrown while trying to register {0}.", spec.FullName), te);
                        }
                        else
                        {
                            //Can't create the object because it has a specification that hasn't been loaded yet
                            //save it for the next pass
                            delayedSpecs.Add(spec);

                        }
                    }
                    catch (Exception err)
                    {
                        throw new SpecExpressConfigurationException(
                          string.Format("Exception thrown while trying to register {0}.", spec.FullName), err);
                    }
                }
            });

            return delayedSpecs;

            //Process any specification that couldn't be reloaded
            //if (delayedSpecs.Any())
            //{
            //    Add(delayedSpecs);
            //}
        }


    }
}
