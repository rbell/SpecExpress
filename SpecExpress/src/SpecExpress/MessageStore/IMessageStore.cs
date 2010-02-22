using System.Reflection;
using SpecExpress.Rules;

namespace SpecExpress.MessageStore
{
    public interface IMessageStore
    {
        //string GetMessageTemplate(MessageContext context);
        string GetMessageTemplate(string key);
        bool IsMessageInStore(string key);
    }
}