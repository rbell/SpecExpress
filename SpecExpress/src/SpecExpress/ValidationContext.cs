using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress
{
    public abstract class ValidationContext
    {
        //private ISpecificationContainer _specContainer = new SpecificationContainer();
        private ISpecificationContainer _specContainer = new NoInstanceSpecificiationContainer();


        public ValidationContext()
        {
            
        }
        
        public void AddSpecifications(Func<IEnumerable<SpecificationBase>,IEnumerable<SpecificationBase>>  selectedSpecs)
        {  
            //Add Specification returned by the function to the Context
            _specContainer.Add(selectedSpecs(ValidationCatalog.SpecificationContainer.GetAllSpecifications()));
        }

        public void AddSpecification<TSpecType>() where TSpecType : SpecificationBase, new()
        {
            _specContainer.Add(new TSpecType());
        }

        public ISpecificationContainer SpecificationContainer
        {
            get { return _specContainer; }
        }
    }
}
