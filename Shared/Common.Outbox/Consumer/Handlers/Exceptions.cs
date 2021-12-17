namespace Common.Outbox.Consumer.Handlers
{
    public class EventsConsumerException : Exception
    {
    }

    public class TypeHeaderIsNotSpecified : EventsConsumerException
    {
    }

    public class EventTypeNotFound : EventsConsumerException
    {
    }
}
