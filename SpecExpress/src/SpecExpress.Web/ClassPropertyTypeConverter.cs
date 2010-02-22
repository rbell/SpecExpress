using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace SpecExpress.Web
{
 public class ClassPropertyTypeConverter : StringConverter
    {
       
        protected virtual bool FilterControl(Control control)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null)
            {
                return null;
            }
            
            var properties = getValuesFromManager(context);
            return new TypeConverter.StandardValuesCollection(properties);
        }
      

        private static List<string> getValuesFromManager(ITypeDescriptorContext context)
        {
            var specManager = context.Container.Components.OfType<SpecificationManager>().FirstOrDefault();
            var validator = context.Instance as Validator;

            if (validator == null || specManager == null)
            {
                return null;
            }

            return   specManager.GetTypeToValidate().GetProperties().Select(p => p.Name).OrderBy(x => x).ToList();
        }

     public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return (context != null);
        }
    }
}
