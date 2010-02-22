using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SpecExpress.Util;

[assembly: TagPrefix("SpecExpress", "spec")]

namespace SpecExpress.Web
{
    [ToolboxData("<{0}:Validator runat='server'></{0}:Validator>")]    
    public class Validator : BaseValidator
    {
        private PropertyValidator _currentPropertyValidator;
        private Specification _currentSpecification;
        private ValidationSummaryDisplayMode displayMode;
        private string _defaultErrorMessage = "Default error message";

        public Validator()
        {

        }


        protected PropertyValidator CurrentPropertyValidator
        {
            get
            {
                if (_currentPropertyValidator == null)
                {
                    if (CurrentSpecification == null)
                    {
                        //added for support for designer
                        return null;
                    }

                    _currentPropertyValidator =
                        CurrentSpecification.PropertyValidators.Where(x => x.PropertyInfo.Name == PropertyName).FirstOrDefault();
                }

                return _currentPropertyValidator;
            }
        }

        protected  Specification CurrentSpecification
        {
            get
            {
                var manager = Page.Controls.All().OfType<SpecificationManager>().First();
                return manager.GetSpecification();
            }
        }

        protected List<ValidationResult> PropertyErrors
        {
            get
            {
                if (ValidationNotification == null || !ValidationNotification.Errors.Any())
                {
                    return new List<ValidationResult>();
                }
                else
                {
                    //TODO: Support Nested ValidationResults
                    return ValidationNotification.Errors.Where(x => x.Property.Name == PropertyName).ToList();
                }
            }

            private set 
            { 
                ValidationNotification = new ValidationNotification();
                ValidationNotification.Errors = value;
            }
        }

        protected bool PropertyIsRequired
        {
            get 
            {
                if (CurrentPropertyValidator == null)
                {
                    return false;
                }
                else
                {
                    return CurrentPropertyValidator.PropertyValueRequired;
                }
            }
        }

        [TypeConverter(typeof(ClassPropertyTypeConverter)), Description("Property on Type this validator is bound to."), Category("Behavior"), Themeable(false), DefaultValue("")]
        public string PropertyName { get; set; }
        public string TypeName { get; set; }
        public ValidationNotification ValidationNotification { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ValidationSummaryDisplayMode"/> indicating how to format multiple validation results.
        /// </summary>
        public ValidationSummaryDisplayMode DisplayMode
        {
            get { return displayMode; }
            set { displayMode = value; }
        }
        
        public string InitialValue
        {
            get
            {
                object obj2 = ViewState["InitialValue"];
                if (obj2 != null)
                {
                    return (string) obj2;
                }
                return string.Empty;
            }
            set { ViewState["InitialValue"] = value; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (PropertyIsRequired)
            {
                Page.ClientScript.RegisterExpandoAttribute(ClientID, "initialvalue", InitialValue);

                //The script below causes a "Duplicate Key" exception when combined with the Server side extender
                ////Client Scripts adding scripts mocking a  Required Field Validator
                Page.ClientScript.RegisterExpandoAttribute(ClientID, "evaluationfunction",
                                                           "SpecExpressProxyValidatorEvaluateIsValid", false);
              

               string requiredErrorMessage = CurrentPropertyValidator.RequiredRule.ErrorMessageTemplate;


                var labelName = GetLabelName();


                string formattedRequiredErrorMessage = requiredErrorMessage.Replace("{PropertyName}", labelName);

                //TODO: This is required if you want the error message to be displayed inline. Not sure how to handle this generically
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    this.ErrorMessage = formattedRequiredErrorMessage;
                    this.Text = formattedRequiredErrorMessage;
                }
                else
                {
                    this.Text = ErrorMessage;
                } 

                Page.ClientScript.RegisterExpandoAttribute(ClientID, "requirederrormessage",
                                                           formattedRequiredErrorMessage, true);
            }

            base.AddAttributesToRender(writer);
        }

        /// <summary>
        ///Try and get the Property Name from label, if not found, default to PropertyName 
        /// </summary>
        /// <returns></returns>
        private string GetLabelName()
        {
            string labelName;
            var labelControl = Page.Controls.All().OfType<Label>().Where(x => x.AssociatedControlID == ControlToValidate).FirstOrDefault();
            if (labelControl == null)
            {
                //TODO: Get UI friendly name from PropertyValidator 
                //Label control not found, default to type name
                labelName = PropertyName.SplitPascalCase();
            }
            else
            {
                labelName = labelControl.Text.Replace(":", "");
            }
            return labelName;
        }

        protected override bool EvaluateIsValid()
        {
            //UPDATE: Because the SpecificationManager is a Custom Validator, it will only be run on the server
            //          If SpecExpress Web is used in conjunction with a standard ASP.NET Validator, and a Validator fails,
            //          SpecificationManager will not execute, so each Validator will need to continue to support validating a property
            if (ValidationNotification == null)
            {
                //Validate just this property
                //Create a new object of Type and set the property
                PropertyErrors = validateProperty();
            }
          
            //TODO: Also check in Nested ValidationResults for this PropertyType and PropertyName
            if (PropertyErrors.Any())
            {
                //Update error message to change Property Name to label
                var pageLocalizedPropertyErrors = PropertyErrors.Select(x => x.Message.Replace(x.Property.Name.SplitPascalCase(), GetLabelName())).ToList();
                ErrorMessage = FormatErrorMessage(pageLocalizedPropertyErrors, DisplayMode);
                IsValid = false;
                return false;
            }
            else
            {
                IsValid = true;
                return true;
            }
        }

        private List<ValidationResult> validateProperty()
        {
            var controlValue = GetControlValidationValue(ControlToValidate);
            var value = TryConvertControlValue(controlValue);
            var objToValidate = setPropertyOnProxyObject(value);
            var results = ValidationCatalog.ValidateProperty(objToValidate, PropertyName, CurrentSpecification).Errors;
            return results;
        }
        
        /// <summary>
        /// Attempt to convert from a string to the Property Type. Return null if it fails.
        /// </summary>
        /// <param name="controlValue"></param>
        /// <returns></returns>
        private object TryConvertControlValue(string controlValue)
        {
            //Get the Type of the Property represented by PropertyName
            var propertyType = getTypeForProperty();

            //Convert from string value to the type for the Property
            var foo = TypeDescriptor.GetConverter(propertyType);
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

        private object setPropertyOnProxyObject(object value)
        {
            //Create a placeholder object for the type we are validating
            var obj = Activator.CreateInstance(CurrentSpecification.ForType, true);

            //Only set the property value if it's not null, otherwise the value will be the types default
            //for example, if the type is Double and the value is passed in, the resulting value will be 0.0, 
            //the default for the type
            if (value != null)
            {
                //set the value of the Property we are validating
                CurrentSpecification.ForType.GetProperty(this.PropertyName).SetValue(obj, value, null);
            }

            return obj;
        }

        private Type getTypeForProperty()
        {
            var property = CurrentSpecification.ForType.GetProperty(this.PropertyName);
            return property.PropertyType;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (PropertyIsRequired)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof (Validator), "Script"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof (Validator), "Script",
                                                                @"<script type=""text/javascript"">function SpecExpressProxyValidatorEvaluateIsValid(val) {var returnval = RequiredFieldValidatorEvaluateIsValid(val); if (!returnval){ val.errormessage == val.requirederrormessage;};return returnval;}</script>");
                }
            }
        }

        internal string FormatErrorMessage(List<string> messages, ValidationSummaryDisplayMode displayMode)
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


            messages.ForEach(x =>
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