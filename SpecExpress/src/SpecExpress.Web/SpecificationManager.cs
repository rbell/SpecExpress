using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: TagPrefix("SpecExpress", "spec")]


namespace SpecExpress.Web
{
    [Designer("System.Web.UI.Design.ControlDesigner,System.Design, Version=2.0.0.0"), DefaultProperty("Scripts"), NonVisualControl, ParseChildren(false)]
    [ToolboxData("<{0}:SpecificationManager runat='server'></{0}:SpecificationManager>")]    
    public class SpecificationManager : CustomValidator 
    {
        private string _specificationType;
        private string _type;

        private Type _resolvedType;
        private SpecificationBase _resolvedSpecificationBase;
        
        public string TypeToValidate { set { _type = value; } }

        //EventHandlers
        public delegate object GetObjectHandler();
        public delegate void ValidationNotificationHandler(object sender, ValidationNotificationEventArgs e);
        //Events
        public event ValidationNotificationHandler ValidationNotification;
        public event GetObjectHandler GetObject;

        public string SpecificationType
        {
            set 
            {
                _specificationType = value;
            }
        }


        public Type GetTypeToValidate()
        {
            if (_resolvedType == null)
            {
                _resolvedType = Type.GetType(_type);
            }

            return _resolvedType;
        }

        public SpecificationBase GetSpecification()
        {
            if (_resolvedSpecificationBase == null)
            {
                //Lazy Load Specification
                if (String.IsNullOrEmpty(_specificationType))
                {
                    //No Specification specified, so get Default Specification For Type from Validation Catalog
                    _resolvedSpecificationBase = ValidationCatalog.SpecificationContainer.TryGetSpecification(GetTypeToValidate());
                }
                else
                {
                    //Get Specification from Type
                    //Create type from string
                    var specType = System.Type.GetType(_specificationType);

                    if (specType == null)
                    {
                        //Type creation failed
                        return null;
                    }
                    else
                    {
                        //Query the Validation Catalog from the specification that matches type in the Catalog
                        _resolvedSpecificationBase = ValidationCatalog.SpecificationContainer.GetAllSpecifications().Where(
                            x => x.GetType() == specType).FirstOrDefault();
                    }
                }
            }

            return _resolvedSpecificationBase;
        }

        protected override bool OnServerValidate(string value)
        {
            object objectToValidate;

            if (GetObject == null)
            {
                //Build object from Validators
                objectToValidate = BuildObjectToValidateFromControls();
            }
            else
            {
                //Get the object to validate from the Page
                objectToValidate = GetObject();
            }

            //Validate the object using the ValidationCatalog
            var vldNotification = ValidationCatalog.Validate(objectToValidate, GetSpecification());

            if (!vldNotification.IsValid)
            {
                //Invalid
                //Raise notification to controls
                Notify(vldNotification);

                //Raise OnValidationNotification Event
                if (ValidationNotification != null)
                {
                    ValidationNotification(this, new ValidationNotificationEventArgs(vldNotification));
                }
            }

            return vldNotification.IsValid;
        }

        //public void ValidateFromClient(SpecManagerServerValidationResult request)
        //{
            
        //}


        public void Notify(ValidationNotification notification)
        {
            //Bind the ValidationNotification to each Proxy Validator
            var specValidators = NamingContainer.Controls.All().OfType<Validator>();
            //Explicitly call Validate to trigger any validation messages
            specValidators.ToList().ForEach(x =>
            {
                x.ValidationNotification = notification;
                x.Validate();
            });

            //Get any Errors that aren't bound to a PropertyValidator and add it to the Validation Summary
            var ufoProperties = (from error in notification.Errors
                                 select error.Property.Name).Except(
               from validators in specValidators select validators.PropertyName).ToList();

            //Group ValidationResults by Property Name so a all results for a Propert can be passed to one
            //DummyValidator which will format the list of results for that Property
            var errorsByPropertyName =
            from error in notification.Errors
            group error by error.Property.Name
                into p
                select new { PropertyName = p.Key, Errors = p };

            foreach (var property in errorsByPropertyName)
            {
                //Check if this PropertyName is in the list of Properties with no Validator, and if so, add one
                if (ufoProperties.Exists(x => x == property.PropertyName))
                {
                    this.Page.Validators.Add(new SpecExpressDummyValidator(property.Errors.ToList()));
                }
            }
        }

        public List<Validator> RelatedValidationControls
        {
            get { return NamingContainer.Controls.All().OfType<Validator>().ToList(); }
        }

        //private object BuildObjectToValidateFromClientRequest(List<SpecManagerServerValidationResult> requests)
        //{
        //    //Create a placeholder object for the type we are validating
        //    var objectToValidate = Activator.CreateInstance(GetSpecification().ForType, true);

        //    //Bind the ValidationNotification to each Proxy Validator
        //    var specValidators = NamingContainer.Controls.All().OfType<Validator>();
        //    //Explicitly call Validate to trigger any validation messages
        //    specValidators.ToList().ForEach(x =>
        //    {
        //        var controlValue = GetControlValidationValue(x.ControlToValidate);
        //        var convertedControlVal = TryConvertControlValue(controlValue, x.PropertyName);
        //        if (convertedControlVal != null)
        //        {
        //            _resolvedSpecification.ForType.GetProperty(x.PropertyName).SetValue(objectToValidate, convertedControlVal, null);
        //        }
        //    });

        //    return objectToValidate;

        //}


        private object BuildObjectToValidateFromControls()
        {
             //Create a placeholder object for the type we are validating
            var objectToValidate = Activator.CreateInstance(GetSpecification().ForType, true);

            //Bind the ValidationNotification to each Proxy Validator
            var specValidators = NamingContainer.Controls.All().OfType<Validator>();
            //Explicitly call Validate to trigger any validation messages
            specValidators.ToList().ForEach(x =>
            {
                var controlValue = GetControlValidationValue(x.ControlToValidate);
                var convertedControlVal = TryConvertControlValue(controlValue, x.PropertyName);
                if (convertedControlVal != null)
                {
                    _resolvedSpecificationBase.ForType.GetProperty(x.PropertyName).SetValue(objectToValidate, convertedControlVal, null);
                }
            });

            return objectToValidate;
          
        }

        private object TryConvertControlValue(string controlValue, string propertyName)
        {
            //Get the Type of the Property represented by PropertyName
            var property = GetSpecification().ForType.GetProperty(propertyName);

            //Convert from string value to the type for the Property
            var foo = TypeDescriptor.GetConverter(property.PropertyType);
            try
            {
                var convertedValue = foo.ConvertFromInvariantString(controlValue.ToString());
                return convertedValue;
            }
            catch (Exception)
            {

                return null;
            }
        }

      


        protected class SpecExpressDummyValidator : IValidator
        {
            private string errorMsg;

            public SpecExpressDummyValidator(string msg)
            {
                errorMsg = msg;
            }

            public SpecExpressDummyValidator(List<ValidationResult> results)
            {
                //TODO: hardcoded ValidationSummaryDisplayMode
                errorMsg = FormatErrorMessage(results, ValidationSummaryDisplayMode.List);
            }

            public string ErrorMessage
            {
                get { return errorMsg; }
                set { errorMsg = value; }
            }

            public bool IsValid
            {
                get { return false; }
                set { }
            }

            public void Validate()
            {
            }

            protected string FormatErrorMessage(List<ValidationResult> results, ValidationSummaryDisplayMode displayMode)
            {
                var stringBuilder = new StringBuilder();
                string errorsListStart;
                string errorStart;
                string errorEnd;
                string errorListEnd;

                switch (displayMode)
                {
                    case ValidationSummaryDisplayMode.List:
                        errorsListStart = string.Empty;
                        errorStart = string.Empty;
                        errorEnd = "<br/>";
                        errorListEnd = string.Empty;
                        break;

                    case ValidationSummaryDisplayMode.SingleParagraph:
                        errorsListStart = string.Empty;
                        errorStart = string.Empty;
                        errorEnd = " ";
                        errorListEnd = "<br/>";
                        break;

                    default:
                        errorsListStart = "<ul>";
                        errorStart = "<li>";
                        errorEnd = "</li>";
                        errorListEnd = "</ul>";
                        break;
                }

                stringBuilder.Append(errorsListStart);

                results.Select(x => x.Message).ToList().ForEach(x =>
                {
                    stringBuilder.Append(errorStart);
                    stringBuilder.Append(x);
                    stringBuilder.Append(errorEnd);
                });


                stringBuilder.Append(errorListEnd);

                return stringBuilder.ToString();
            }
        }



    }
}
