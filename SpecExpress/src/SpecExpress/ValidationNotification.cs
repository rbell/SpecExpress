using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SpecExpress;
using SpecExpress.Enums;

namespace SpecExpress
{
#if !SILVERLIGHT
    [Serializable]
#endif
   
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
                if (!Errors.Any())
                {
                    //empty collection of errors
                    return true;
                }

                //Check if there are any validation results with a Level of Error
                return All().All(e => e.Level == ValidationLevelType.Warn);
            }
        }


        public IEnumerable<ValidationResult> FindDescendents(Func<ValidationResult, bool> predicate)
        {
            foreach (var vr in this.Errors)
            {
                var foundNodes = vr.FindDescendents(predicate);

                if (foundNodes.Any())
                {
                    foreach (var validationResult in foundNodes)
                    {
                        yield return validationResult;
                    }
                }
            }
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

public static class ValidationNotificationExtensions
{
    public static ValidationNotification ToNotification(this IEnumerable<ValidationResult> validationResults)
    {
        var notification = new ValidationNotification()
        {
            Errors = validationResults.ToList()
        };
        return notification;
    }
}