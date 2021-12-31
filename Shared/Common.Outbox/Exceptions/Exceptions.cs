namespace Common.Outbox.Exceptions
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
