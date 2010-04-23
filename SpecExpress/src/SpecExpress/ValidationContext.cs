using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public abstract class ValidationContext
    {
        private SpecificationContainer _specContainer = new SpecificationContainer();

        public ValidationContext()
        {
            
        }
        
        public void AddSpecifications(Func<IEnumerable<Specification>,IEnumerable<Specification>>  selectedSpecs)
        {  
            //Add Specification returned by the function to the Context
            _specContainer.Add(selectedSpecs(ValidationCatalog.CatalogSpecificationContainer.GetAllSpecifications()));
        }

        public void AddSpecification<TSpecType>() where TSpecType : Specification, new()
        {
            _specContainer.Add(new TSpecType());
        }

        public SpecificationContainer SpecificationContainer
        {
            get { return _specContainer; }
        }
    }
}
