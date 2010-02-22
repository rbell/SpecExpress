using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpecExpress.MessageStore;

namespace SpecExpress
{
    public class ValidationCatalogConfiguration
    {
        public  IDictionary<string, IMessageStore> MessageStores { private set; get;}


        public bool ValidateObjectGraph { get; set; }

        public void AddMessageStore(IMessageStore store, string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (MessageStores == null)
            {
                MessageStores = new Dictionary<string, IMessageStore>();
            }
            MessageStores.Add(key, store);
        }

        public IMessageStore DefaultMessageStore { get; set; }
        
    }
}
