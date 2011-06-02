using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpecExpress.MVC.Example.Models;

namespace SpecExpress.MVC.Example.Specifications
{
    public class AppointmentModelSpecification : Validates<AppointmentModel>
    {
        public AppointmentModelSpecification()
        {
            Check(a => a.Name).Required().MinLength(5).And.MaxLength(7);
            Check(a => a.Location).Required();
            Check(a => a.Start).If(a => a.AllDay).Required();
        }
    }
}