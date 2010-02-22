using System;
using System.Collections.Generic;
using SpecExpress.Rules.DateValidators;
using SpecExpressTest.Entities;

namespace SpecExpress.Test.Entities
{
    public class ContactWithIsValid
    {
        public ValidationNotification Errors;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int NumberOfDependents { get; set; }
        public List<Address> Addresses { get; set; }

        public bool IsValid()
        {
            ValidationCatalog.AddSpecification<Contact>(x =>
                                                              {
                                                                  x.Check(contact => contact.LastName).Required();
                                                                  x.Check(contact => contact.FirstName).Required();
                                                                  x.Check(contact => contact.DateOfBirth).Optional().
                                                                      GreaterThan(new DateTime(1950, 1, 1));
                                                              });

            //Validate
            Errors = ValidationCatalog.Validate(this);
            return Errors.IsValid;
        }
    }
}