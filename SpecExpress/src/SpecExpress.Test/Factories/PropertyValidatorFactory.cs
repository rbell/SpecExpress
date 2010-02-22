using SpecExpress.Test.Entities;

namespace SpecExpress.Test.Factories
{
    public static class PropertyValidatorFactory
    {
        public static PropertyValidator<Customer, string> DefaultCustomerNameValidator()
        {
            return new PropertyValidator<Customer, string>(c => c.Name);
        }
    }
}