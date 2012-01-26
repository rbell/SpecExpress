using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SpecExpress
{
    public class SpecificationScanner
    {
        private readonly List<Type> _specifications = new List<Type>();

        internal IList<Type> FoundSpecifications
        {
            get { return _specifications; }
        }
        
        public void TheCallingAssembly()
        {
            Assembly callingAssembly = findTheCallingAssembly();

            if (callingAssembly != null)
            {
                AddAssembly(callingAssembly);
            }
        }

        public void AddAssembly(Assembly assembly)
        {
            scanAssembliesForSpecifications(new List<Assembly>() { assembly });            
        }

        public void AddAssemblies(List<Assembly> assemblies)
        {
            scanAssembliesForSpecifications(assemblies);
        }

        public void AddAssembliesFromPath(string path)
        {
            var r = Directory.EnumerateFiles(path).Where(file =>
                                                                               Path.GetExtension(file).Equals(
                                                                                   ".exe",
                                                                                   StringComparison.OrdinalIgnoreCase)
                                                                               ||
                                                                               Path.GetExtension(file).Equals(
                                                                                   ".dll",
                                                                                   StringComparison.OrdinalIgnoreCase))
                                                                                   .Select(assemblyPath => Assembly.LoadFrom(assemblyPath));

           
            List<Assembly> assemblies = r.Where<Assembly>(assembly => assembly != null && assembly != typeof(ValidationCatalog).Assembly)
                .ToList<Assembly>();

            scanAssembliesForSpecifications(assemblies);
        }

        private Assembly findTheCallingAssembly()
        {
            var trace = new StackTrace(false);

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                Assembly assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != thisAssembly)
                {
                    callingAssembly = assembly;
                    break;
                }
            }
            return callingAssembly;
        }

        private void scanAssembliesForSpecifications(List<Assembly> assemblies)
        {
            var thisAssembly = this.GetType().Assembly.FullName;

            //Find all types in all assemblies that inherit from Specification
            IEnumerable<Type> specs = from a in assemblies where a.FullName != thisAssembly
                        select a.GetExportedTypes() into types
                        from type in types
                        where typeof(SpecificationBase).IsAssignableFrom(type) && !type.IsAbstract
                        select type;

            _specifications.AddRange(specs);
        }
    }
}