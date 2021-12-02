using Microsoft.Extensions.CommandLineUtils;

namespace Common.MongoDb.Migrations.Services.ToDo
{
    /// <summary>
    /// Command line application.
    /// </summary>
    internal class Application
    {
        private readonly MigrationsIdGenerator _migrationsIdGenerator;

        public int Run(params string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "migrator",
                FullName = "MongoDb migrations manager",
                Description = "Creates, runs and does other migrations managing deals",
            };
            app.HelpOption("-?|-h|--help");
            app.Command(
                "create",
                application =>
                {
                    application.Description = "Creates migration";
                    var migrationName = application.Argument("name", "Migration name");
                    var projectPath = application.Argument("project", "Project path");
                    application.OnExecute(
                        () =>
                        {
                            return 0;
                        });
                });

            return app.Execute(args);
        }
    }
}