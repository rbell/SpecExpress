using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress;

namespace SpecExpressTest
{
   public class DeleteValidationContext : ValidationContext
   {
       public DeleteValidationContext()
       {
           AddSpecifications(cfg => cfg.Where(s => s.GetType().Name.StartsWith("Delete") == true));
           //AddSpecification<ContactSpecification>();
       }
   }
}
