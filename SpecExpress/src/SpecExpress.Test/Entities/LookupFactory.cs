using System.Collections.Generic;

namespace SpecExpress.Test.Entities
{
    public static class LookupFactory
    {
        public static List<Country> GetCountries()
        {
            return new List<Country>()
                       {
                           new Country(){ Id = "US", Name = "United States"},
                           new Country(){ Id = "CA", Name = "Canada"},
                           new Country(){ Id = "DE", Name = "Germany"},
                           new Country(){ Id = "FR", Name = "France"}
                       };
        }
    }
}
