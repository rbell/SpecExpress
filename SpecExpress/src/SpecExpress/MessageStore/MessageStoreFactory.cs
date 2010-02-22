using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecExpress.MessageStore
{
    public static class MessageStoreFactory
    {
        private static IMessageStore _messageStore;
        
        public static IMessageStore GetMessageStore(string key)
        {
            if (ValidationCatalog.Configuration.MessageStores.ContainsKey(key))
            {
                return ValidationCatalog.Configuration.MessageStores[key];
            }
            else
            {
                return null;
            }
        }

        public static List<IMessageStore> GetAllMessageStores()
        {
            var allMessageStores = new List<IMessageStore>()
                                       {
                                           ValidationCatalog.Configuration.DefaultMessageStore
                                       };


            if (ValidationCatalog.Configuration.MessageStores != null && ValidationCatalog.Configuration.MessageStores.Any())
            {
                allMessageStores.AddRange(ValidationCatalog.Configuration.MessageStores.Values);
            }
           
         
            return allMessageStores;

        }

    }
}