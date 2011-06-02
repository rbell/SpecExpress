using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpecExpress.MVC.Example.Models
{
    public class AppointmentModel //: IValidatableObject 
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        //public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        //{
        //    var vn = ValidationCatalog.Validate(validationContext.ObjectInstance);
            

        //    //return error message the the property name so it can display it next tot he field
        //    return vn.All().Select(
        //        v =>
        //        new System.ComponentModel.DataAnnotations.ValidationResult(v.ToString(),new List<String>() {v.Property.Name}));
        //}
    }
}