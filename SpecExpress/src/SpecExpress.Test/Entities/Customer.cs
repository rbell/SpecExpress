using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SpecExpress.Test.Entities;

namespace SpecExpress.Test.Entities
{
    public class Customer
    {
        public string Id { get; set; }
        [Display(Name = "Cust Name")]
        public string Name { get; set; }
        public DateTime CustomerDate { get; set; }
        public DateTime ActiveDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? PromotionDate { get; set; }
        public Address Address { get; set; }
        public int Max { get; set;}
        public int Min { get; set; }
        public string NamePattern { get; set; }
        public List<Contact> Contacts { get; set;}
    }
}