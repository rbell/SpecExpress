using System.Linq;
using NUnit.Framework;
using SpecExpress.MessageStore;
using SpecExpress.Test.MessageStore;

namespace SpecExpress.Test
{
    [TestFixture]
    public class MessageStoreFactoryTests
    {
        [TearDown]
        public void Teardown()
        {
            ValidationCatalog.ResetConfiguration();
        }

        [Test]
        public void GetMessageStore_ReturnsDefaultMessageStore()
        {
            Assert.That(MessageStoreFactory.GetAllMessageStores().First(), Is.InstanceOf(typeof(ResourceMessageStore)));
        }

        [Test]
        public void GetCustomMessageStore_ReturnsNamedMessageStore()
        {
            ValidationCatalog.Configure(x => x.AddMessageStore(new ResourceMessageStore(TestRuleErrorMessages.ResourceManager), "MyMessageStore"));
            Assert.That(MessageStoreFactory.GetMessageStore("MyMessageStore"), Is.InstanceOf(typeof(ResourceMessageStore)));

        }

        [Test]
        public void GetMessageStore_ReturnsOverriddenDefaultMessageStore()
        {
            ValidationCatalog.Configure(x => x.DefaultMessageStore = new SimpleMessageStore());

            Assert.That(MessageStoreFactory.GetAllMessageStores().First(), Is.InstanceOf(typeof(SimpleMessageStore)));

        }
    }


    public class SimpleMessageStore : IMessageStore
    {
        
        public string GetMessageTemplate(string key)
        {
            return "A rule is broken!";
        }

        public bool IsMessageInStore(string key)
        {
            return true;
        }

     
    }
}
    