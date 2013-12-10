using SpecExpress.Test.Domain.Entities;
namespace SpecExpress.Test.Domain.Specifications
{
    public class ProjectSpecification : Validates<Project>
    {
        public ProjectSpecification()
        {
            Check(p => p.ProjectName).Required().LengthBetween(0, 30);
            Check(p => p.StartDate).Required().LessThan(p => p.EndDate);
            Check(p => p.EndDate).Required().GreaterThan(p => p.StartDate);
        }
    }
}