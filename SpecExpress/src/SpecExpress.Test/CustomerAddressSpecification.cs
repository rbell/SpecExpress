using SpecExpress;
using SpecExpress.Test;
using SpecExpress.Test.Entities;
using System.Linq;


namespace SpecExpress.Test
{
    public class CustomerAddressSpecification : Validates<Customer>
    {
        public CustomerAddressSpecification()
        {
            Check(c => c.Name).Required();
            Check(c => c.Address).Required().Specification<AddressSpecification>();

            //Check(c => c.Contacts).Required().With.ForEachSpecification<Contact>(spec =>
            //                                                                         {
            //                                                                             spec.Check(c => c.FirstName).
            //                                                                                 Required();
            //                                                                             spec.Check(c => c.LastName).
            //                                                                                 Required();
            //                                                                         });
                                                                                      

            Check(c => c.Contacts.First()).Required().Specification(spec =>
                {
                    spec.Check(contact => contact.LastName).Required();
                    spec.Check(contact => contact.FirstName).Required();
                });
        }
    }
}