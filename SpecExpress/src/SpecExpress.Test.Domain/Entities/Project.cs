using System;

namespace SpecExpress.Test.Domain.Entities
{
    public class Project
    {
        public Project()
        {
            StartDate = DateTime.Today;
            EndDate = StartDate.AddMonths(1);
        }

        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}