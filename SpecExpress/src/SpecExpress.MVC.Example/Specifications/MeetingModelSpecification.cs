using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpecExpress.MVC.Example.Models;

namespace SpecExpress.MVC.Example.Specifications
{
    public class MeetingModelSpecification : Validates<Meeting>
    {
        public MeetingModelSpecification()
        {
            IsDefaultForType();
            Check(m => m.Title).Required().MinLength(m => m.MinTitleLength);
        }
    }
}