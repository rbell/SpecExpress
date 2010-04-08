using System;
using System.Collections.Generic;

namespace SpecExpress.Test.Entities
{
    public class Contact
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual int NumberOfDependents { get; set; }
        public virtual int NumberOfChildren { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual List<string> Aliases { get; set; }
        public virtual bool Active { get; set; }
        public virtual string NamePattern { get; set; }
        
    }
}