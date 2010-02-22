using System;

namespace SpecExpress.Test.Entities.EntityBuilders
{
    public class CustomerBuilder
    {
        private readonly Customer _customer = new Customer();

        public CustomerBuilder()
        {
            _customer = new Customer();
        }

        public Customer Customer
        {
            get { return _customer; }
        }

        public CustomerBuilder Name(string name)
        {
            _customer.Name = name;
            return this;
        }

        public CustomerBuilder CustomerDate(DateTime dateTime)
        {
            _customer.CustomerDate = dateTime;
            return this;
        }
    }
}