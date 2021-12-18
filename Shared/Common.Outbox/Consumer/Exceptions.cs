namespace Common.Outbox.Consumer
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
