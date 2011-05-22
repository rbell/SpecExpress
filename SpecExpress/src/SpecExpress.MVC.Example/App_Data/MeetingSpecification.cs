using SpecExpress.MVC.Example.Models;

namespace SpecExpress.MVC.Example.App_Data
{
    public class MeetingSpecification : Validates<Meeting>
    {
        public MeetingSpecification()
        {
            IsDefaultForType();
            //Check(m => m.EndDate).Required().GreaterThan(m => m.StartDate);
            //Check(m => m.StartDate).Required().LessThan(m => m.EndDate);
            Check(m => m.Title).Required();
        }

    }
}