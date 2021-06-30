using Common.Configuration;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Xunit.Abstractions;

namespace TestUtils.Integration
{
    /// <summary>
    /// Mongo test fixture.
    /// </summary>
    public class MongoClientFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFixture"/> class.
        /// </summary>
        /// <param name="testOutputHelper">Class used to provide test output.</param>
        public MongoClientFixture(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsHelper.AppSettingsPath)
                .AddEnvironmentVariables()
                .Build();

            MongoEntitiesMapsRegistrar.RegisterConventions();
            Configuration = configuration.GetSection(DatabaseConfiguration.Key).Get<DatabaseConfiguration>();
            Client = CreateClient(Configuration);
            Database = Client.GetDatabase(Configuration.DatabaseName);
        }

        /// <summary>
        /// Gets test output helper.
        /// </summary>
        public ITestOutputHelper TestOutputHelper { get; }

        /// <summary>
        /// Gets database.
        /// </summary>
        public IMongoDatabase Database { get; }

        /// <summary>
        /// Gets client.
        /// </summary>
        public IMongoClient Client { get; }

        /// <summary>
        /// Gets configuration.
        /// </summary>
        public DatabaseConfiguration Configuration { get; }

        private IMongoClient CreateClient(DatabaseConfiguration configuration)
        {
            var settings = MongoClientSettings.FromConnectionString(configuration.ConnectionString);
            if (configuration.EnableQueryLog)
            {
                settings.ClusterConfigurator = builder =>
                {
                    builder.Subscribe<CommandStartedEvent>(e =>
                    {
                        TestOutputHelper.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                    });
                };
            }

            return new MongoClient(settings);
        }
    }
}