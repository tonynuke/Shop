using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using Notifications.Domain;
using Notifications.Persistence;

namespace Notifications.Tests.Unit
{
    public class InMemoryNotificationsContext : NotificationsContext
    {
        public InMemoryNotificationsContext(IMongoDatabase database) 
            : base(database)
        {
        }

        public Mock<IMongoCollection<User>> UsersMock { get; } = new ();

        public override IMongoCollection<User> Users => UsersMock.Object;

        public override Task ExecuteInTransaction(Func<Task> func)
        {
            return func();
        }

        public static InMemoryNotificationsContext Create()
        {
            var databaseStub = new Mock<IMongoDatabase>();
            databaseStub
                .SetupGet(client => client.Client)
                .Returns(Mock.Of<IMongoClient>());

            return new InMemoryNotificationsContext(databaseStub.Object);
        }
    }
}