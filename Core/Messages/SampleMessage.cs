using NServiceBus;

namespace Core.Messages
{
    public class SampleMessage: IMessage
    {
        public string MyString = "asdf";
    }
}
