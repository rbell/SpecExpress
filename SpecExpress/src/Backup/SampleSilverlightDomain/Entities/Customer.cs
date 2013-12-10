using System;
using System.Collections.Generic;
using SpecExpress.Test.Domain.Values;

namespace SpecExpress.Test.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Contact PrimaryContact { get; set; }
        public Address MailingAddress { get; set; }
        public DateTime CreatedDate { get; private set; }
        public List<Contact> Employees { get; set; }
        public bool Active { get; set; }
    }
}