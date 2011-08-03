using System;
using System.ComponentModel.DataAnnotations;

namespace SpecExpress.MVC.Example.Models
{
    public class Meeting
    {
        public string Title { get; set; }

        public int MinTitleLength { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Email { get; set; }

    }
}