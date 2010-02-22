using System;
using System.Collections.Generic;

namespace SpecExpress
{
    public class ValidationNotification
    {
        public ValidationNotification()
        {
            Errors = new List<ValidationResult>();
        }

        public List<ValidationResult> Errors { get; set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }
    }
}