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
            NestedValdiationResults = new List<ValidationResult>();
        }

        public ValidationResult(MemberInfo property, string message,  object target, IEnumerable<ValidationResult> nestedValidationResults)
        {
            _property = property;
            _message = message;
            _target = target;
            NestedValdiationResults = nestedValidationResults;
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
        

        public IEnumerable<ValidationResult> NestedValdiationResults {get;set;}

        public IEnumerable<string> AllErrorMessages()
        {
            foreach (var error in NestedValdiationResults)
            {
                yield return error.Message;

                foreach (var grandchild in error.AllErrorMessages())
                {
                    yield return error.Property.Name + " " + grandchild;
                }
               
            }
        }

        public IEnumerable<ValidationResult> AllValidationResults()
        {
            foreach (var error in NestedValdiationResults)
            {
                yield return error;

                foreach (var grandchild in error.AllValidationResults())
                {
                    yield return grandchild;
                }
            }
        }

       

        public override string ToString()
        {
            return Message;
        }

        internal string PrintNode(string prefix)
        {
            return prefix + Message + "\r\n" +
                                NestedValdiationResults.Select(vr => vr.PrintNode(prefix + "\t")).DefaultIfEmpty().
                                    Aggregate((a, b) => a + b);

        }
        
    }


}