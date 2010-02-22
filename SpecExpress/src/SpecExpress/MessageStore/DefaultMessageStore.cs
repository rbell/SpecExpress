using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Resources;
using SpecExpress.Rules;

namespace SpecExpress.MessageStore
{
    public class DefaultMessageStore : IMessageStore
    {
        private ResourceManager _resource;

        public DefaultMessageStore(ResourceManager resourceManager)
        {
            _resource = resourceManager;
        }

        #region IMessageStore Members

        public string GetMessageTemplate(MessageContext context)
        {
            //Use Name of the Rule Validator as the Key to get the error message format string
            //RuleValidator types have Generics which return Type Name as LengthValidator`1 and we need to remove that
            string key = context.ValidatorType.Name.Split('`').FirstOrDefault();
            return GetMessageTemplate(key);
        }

        public string GetMessageTemplate(object key)
        {   
            string keyName = key as string; 

            if (keyName == null)
            {
                throw new ArgumentException("key must be a string.", "key");
            }

            string errorString = _resource.GetString(keyName);

            if (System.String.IsNullOrEmpty(errorString))
            {
                throw new InvalidOperationException(
                    System.String.Format("Unable to find error message for {0} in resources file.", key));
            }

            return errorString;
        }

        #endregion

    }
}