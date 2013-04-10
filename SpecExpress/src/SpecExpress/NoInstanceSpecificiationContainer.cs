using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.Util;

namespace SpecExpress
{
    public class NoInstanceSpecificiationContainer : ISpecificationContainer
    {
        private class SpecificationRegistryItem
        {
            private List<Type> _allSpecsForType = new List<Type>();
            private List<SpecificationBase> _allSpecsInstantiated = new List<SpecificationBase>();
            private Type _defaultSpecificationForType;
            private Type _forType;

            public SpecificationRegistryItem(Type forType)
            {
                _forType = forType;
            }

            public Type ForType
            {
                get { return _forType; }
            }

            public Type DefaultSpecificationTypeForType
            {
                get
                {
                    return _defaultSpecificationForType ??
                           (_defaultSpecificationForType = DiscoverDefaultSpecificationFromRegisteredSpecifications());
                }
            }

            public SpecificationBase DefaultSpecificationForType
            {
                get
                {
                    SpecificationBase specification;

                    if (DefaultSpecificationTypeForType != null)
                    {
                        specification = Activator.CreateInstance(_defaultSpecificationForType) as SpecificationBase;
                    }
                    else
                    {
                        //TODO: Handle more than one specification
                        specification = _allSpecsInstantiated.SingleOrDefault();
                    }

                    return specification;
                }
            }

            //public IEnumerable<Type> GetAllSpecificationTypesForType()
            //{
            //    return _allSpecsForType.ToList();
            //}

            public IEnumerable<SpecificationBase> GetAllSpecificationsForType
            {
                get
                {
                    var specs = _allSpecsForType.Select(Activator.CreateInstance).Cast<SpecificationBase>().ToList();
                    specs.AddRange(_allSpecsInstantiated);
                    return specs;
                }
                

            }
            public void AddSpecification(Type type)
            {

                //Check if it's been added already
                if (!_allSpecsForType.Contains(type))
                {
                    //TODO:Add validation 
                    _allSpecsForType.Add(type);
                }
            }

            public void AddSpecification(SpecificationBase specificationBase)
            {
                //Check if it's been added already
                if (!_allSpecsInstantiated.Contains(specificationBase))
                {
                    //TODO:Add validation 
                    _allSpecsInstantiated.Add(specificationBase);
                }
            }

            private Type DiscoverDefaultSpecificationFromRegisteredSpecifications()
            {
                Type returnType;

                //No Specifications
                if (!_allSpecsForType.Any())
                {
                    return null;
                }

                if (_allSpecsForType.Count == 1)
                {
                    //Only 1 
                    returnType = _allSpecsForType.First();
                }
                else
                {
                    //More then 1
                    //try to create all of them and look for DefaultForType value
                    var instaniatedSpecs = new List<SpecificationBase>();
                    foreach (var type in _allSpecsForType)
                    {
                        //try creating with default constructor
                        //TODO: how to deal with no default constructor
                        var s = Activator.CreateInstance(type) as SpecificationBase;
                        instaniatedSpecs.Add(s);
                    }

                    //validate that only one is is registered as default
                    var defaultSpecifications = instaniatedSpecs.Where(s => s.DefaultForType).ToList();

                    //None
                    if (!defaultSpecifications.Any())
                    {
                        throw new SpecExpressConfigurationException(string.Format("Multiple Specifications found and none are defined as default for {0}.", ForType.FullName));
                    }

                    //Multiple
                    if (defaultSpecifications.Count() > 1)
                    {
                        throw new SpecExpressConfigurationException(string.Format("Multiple Specifications found and multiple are defined as default for {0}.", ForType.FullName));
                    }

                    returnType = defaultSpecifications.First().GetType();
                }
                return returnType;
            }

        }

        private Dictionary<Type, SpecificationRegistryItem> _forTypeRegistry = new Dictionary<Type, SpecificationRegistryItem>();

        private Type getForType(Type type)
        {

            bool found = false;
            Type currentType = type;

            while (!found)
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
            var forType = currentType.BaseType.GetGenericArguments().FirstOrDefault();


            return forType;
        }

        private void AddForTypeToRegistry(Type specification)
        {
            var forType = getForType(specification);

            if (!_forTypeRegistry.ContainsKey(forType))
            {
                //Add new Item
                var item = new SpecificationRegistryItem(forType);
                _forTypeRegistry.Add(forType, item);
            }

            _forTypeRegistry[forType].AddSpecification(specification);
        }

        

        public void Add<TEntity>(Validates<TEntity> expression)
        {
            //var forType = typeof (TEntity);

            //if (!_forTypeRegistry.ContainsKey(forType))
            //{
            //    //Add new Item
            //    var item = new SpecificationRegistryItem(forType);
            //    _forTypeRegistry.Add(forType, item);
            //}

            //_forTypeRegistry[forType].AddSpecification(expression);
            AddForTypeToRegistry(expression.GetType());
        }

        public void Add<T>(SpecificationExpression<T> expression)
        {
            var forType = typeof(T);

            if (!_forTypeRegistry.ContainsKey(forType))
            {
                //Add new Item
                var item = new SpecificationRegistryItem(forType);
                _forTypeRegistry.Add(forType, item);
            }

            _forTypeRegistry[forType].AddSpecification(expression);
        }

        public void Add(SpecificationBase spec)
        {
            AddForTypeToRegistry(spec.GetType());
        }

        public void Add<TSpec>() where TSpec : SpecificationBase, new()
        {
            AddForTypeToRegistry(typeof(TSpec));
        }

        public void Add(IEnumerable<SpecificationBase> specs)
        {
            foreach (var spec in specs)
            {
                AddForTypeToRegistry(spec.GetType());
            }
        }

        public void Add(IEnumerable<Type> specs)
        {
            foreach (var spec in specs)
            {
                AddForTypeToRegistry(spec);
            }
        }

        public void Reset()
        {
            _forTypeRegistry = new Dictionary<Type, SpecificationRegistryItem>();
        }

        public SpecificationBase TryGetSpecification(Type type)
        {
            Type typeToUse = null;

            //Check for type in Registry
            if (_forTypeRegistry.ContainsKey(type))
            {
                typeToUse = type;
            }
            else
            {
                //Try and find a type that is registered that can be cast
                try
                {
                    //typeToUse = _forTypeRegistry.Keys.SingleOrDefault(t => t.CanBeCastTo(type));

                    foreach (var key in _forTypeRegistry.Keys)
                    {
                        if (type.CanBeCastTo(key))
                        {
                            typeToUse = key;
                        }
                    }
                }
                catch
                {
                    //more then one type found so we can't determine which to use
                    //typeToUse = null;
                }
            }


            if (typeToUse == null)
            {
                //no type registred
                return null;
            }

            var s = _forTypeRegistry[typeToUse].DefaultSpecificationForType;

            



            //var s = Activator.CreateInstance(specType);

            //if (s == null)
            //{
            //    //TODO: throw not specification exception
            //}

            return s as SpecificationBase ;
        }

        public SpecificationBase GetSpecification(Type type)
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

        public List<SpecificationBase> GetAllSpecifications()
        {
            var all = new List<SpecificationBase>();

            foreach (var forType in _forTypeRegistry.Values)
            {
                all.AddRange(forType.GetAllSpecificationsForType);
            }

            return all;

        }


       
    }
}
