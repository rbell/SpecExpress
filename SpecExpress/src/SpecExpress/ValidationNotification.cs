using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SpecExpress.Enums;

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
            get
            {
                //Check if there are any validation results with a Level of Error
                return All().All(e => e.Level == ValidationLevelType.Warn);
            }
        }

        /// <summary>
        /// Creates a ValidationNotification object containing all the errors for the specified property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ValidationNotification GetNotificationForProperty(string propertyName)
        {
            var propertyErrors = All().Where(e => e.Property.Name == propertyName).ToList();
            var notification = new ValidationNotification() {Errors = propertyErrors};
            return notification;

        }

        public IEnumerable<ValidationResult> All()
        {
            return Errors.SelectMany(error => error.All());
        }

        public override string ToString()
        {
            return  Errors.Select( a=> a.PrintNode(string.Empty)).Aggregate( (a, b) => a + b);
        }
    }
}