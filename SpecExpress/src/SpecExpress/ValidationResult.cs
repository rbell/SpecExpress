using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SpecExpress
{
    public class ValidationResult
    {
        private readonly String _message;
        private readonly MemberInfo _property;
        private readonly object _target;
      
        public ValidationResult(MemberInfo property, string errorMessage, object target)
        {
            _property = property;
            _message = errorMessage;
            _target = target;
            NestedValidationResults = new List<ValidationResult>();
        }

        public ValidationResult(MemberInfo property, string message,  object target, IEnumerable<ValidationResult> nestedValidationResults)
        {
            _property = property;
            _message = message;
            _target = target;
            NestedValidationResults = nestedValidationResults;
        }

        public MemberInfo Property
        {
            get { return _property; }
        }

        public string Message
        {
            get { return _message; }
        }

        public object Target
        {
            get { return _target; }
        }
        

        public IEnumerable<ValidationResult> NestedValidationResults {get;set;}

        public IEnumerable<string> AllErrorMessages()
        {
            All().SelectMany(e => e.Message);
            //return (new[] { this.Message }).Union(NestedValidationResults.SelectMany(x => x.AllErrorMessages()));
        }

        public IEnumerable<ValidationResult> All()
        {
            return (new[] { this }).Union(NestedValidationResults.SelectMany(x => x.All()));
        }

        internal string PrintNode(string prefix)
        {
           return prefix + Message + "\r\n" +
                               NestedValidationResults.Select(vr => vr.PrintNode(prefix + "\t")).DefaultIfEmpty().
                                   Aggregate((a, b) => a + b);
           
        }

        public override string ToString()
        {
            return Message;
        }
        
    }


}