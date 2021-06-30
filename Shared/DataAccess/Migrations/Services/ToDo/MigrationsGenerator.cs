using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace DataAccess.Migrations.Services.ToDo
{
    // TODO:
    public class MigrationsGenerator
    {
        private const string Key = "Migrations";
        private readonly MigrationsIdGenerator _idGenerator;

        public MigrationsGenerator(MigrationsIdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }

        public async Task<Result> Generate(
            string migrationName,
            string projectPath = null,
            string dbContextName = null)
        {
            //projectPath ??= Directory
            //    .GetFiles(Directory.GetCurrentDirectory(), "*.csproj")
            //    .Single();
            //Type dbContextType = null;
            //using (var workspace = MSBuildWorkspace.Create())
            //{
            //    var project = await workspace.OpenProjectAsync(projectPath);
            //    var compilation = await project.GetCompilationAsync();
            //    var diagnostics = workspace.Diagnostics;
            //    var x = compilation.Assembly
            //        .GetForwardedTypes()
            //        .Single(type => type.BaseType == typeof(DbContext)
            //                        || type.Name == dbContextName);
            //}
            //if (dbContextName == null)
            //{
            //    var x = AppDomain.CurrentDomain
            //        .GetAssemblies()
            //        .SelectMany(assembly => assembly.GetTypes())
            //        .Where(type => type.BaseType != null && type.BaseType == typeof(DbContext));
            //}
            //else
            //{
            //    dbContextType = Type.GetType(dbContextName);
            //}
            //var migrationId = _idGenerator.GenerateId(migrationName);
            //var migrationCode = GenerateMigrationCode(migrationName, migrationId, dbContextType);
            //var targetPath = Path.Combine(Path.GetDirectoryName(projectPath), Key);
            //Directory.CreateDirectory(targetPath);
            //var migrationFile = $"{migrationId}.cs";
            //var migrationPath = Path.Combine(targetPath, migrationFile);
            //await File.WriteAllTextAsync(migrationPath, migrationCode);
            return Result.Success();
        }

        private static string GenerateMigrationCode(
            string migrationName, string migrationId, Type dbContextType)
        {
            //var members = typeof(IMigration).GetMembers();
            //var methods = members.OfType<MethodInfo>()
            //    .Select(member => MethodDeclaration(ParseTypeName(member.ReturnType.Name), member.Name)
            //        .AddModifiers(
            //            Token(SyntaxKind.PublicKeyword),
            //            Token(SyntaxKind.OverrideKeyword))
            //        .WithBody(Block(ParseStatement("return Task.CompletedTask;"))));
            //var migrationAttribute = Attribute(
            //    IdentifierName("Migration"),
            //    AttributeArgumentList(SeparatedList(
            //        new[]
            //        {
            //            AttributeArgument(
            //                LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(migrationId)))
            //        })));
            //const string parameterName = "context";
            //var constructor = ConstructorDeclaration(migrationName)
            //    .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            //    .WithParameterList(ParameterList()
            //        .AddParameters(Parameter(Identifier(parameterName))
            //            .WithType(ParseTypeName(dbContextType.Name))))
            //    .WithInitializer(
            //        ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
            //            .AddArgumentListArguments(Argument(IdentifierName(parameterName))))
            //    .WithBody(Block());
            //var migrationTypeSyntax = SimpleBaseType(ParseTypeName($"Migration<{dbContextType.Name}>"));
            //var attributeListSyntax = AttributeList(SingletonSeparatedList(migrationAttribute));
            //var migrationClass = ClassDeclaration(migrationName)
            //    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword))
            //    .AddBaseListTypes(migrationTypeSyntax)
            //    .AddMembers(constructor)
            //    .AddMembers(methods.ToArray())
            //    .AddAttributeLists(attributeListSyntax);
            //return NamespaceDeclaration(ParseName($"{dbContextType.Namespace}.{Key}"))
            //    .AddUsings(
            //        UsingDirective(ParseName("System")),
            //        UsingDirective(ParseName("System.Threading.Tasks")),
            //        UsingDirective(ParseName("Migrator.Domain")),
            //        UsingDirective(ParseName("Migrator.Domain.Attributes")))
            //    .AddMembers(migrationClass)
            //    .NormalizeWhitespace()
            //    .ToFullString();
            return string.Empty;
        }
    }
}