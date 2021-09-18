using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace DataAccess.Entities
{
    /// <summary>
    /// Mongo entities mappings registrar.
    /// </summary>
    public static class MongoEntitiesMapsRegistrar
    {
        /// <summary>
        /// Registers entities mappings from assembly.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        public static void RegisterEntitiesMapsFromAssembly(Assembly assembly)
        {
            var mapsTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition);

            var registerEntityMapMethod = typeof(MongoEntitiesMapsRegistrar)
                .GetMethods()
                .Single(methodInfo =>
                    methodInfo.Name == nameof(RegisterEntityMap)
                    && methodInfo.ContainsGenericParameters
                    && methodInfo.GetParameters().SingleOrDefault()?.ParameterType
                        .GetGenericTypeDefinition() == typeof(IMongoEntityMap<>));

            foreach (var type in mapsTypes)
            {
                // Only accept types that contain a parameterless constructor, are not abstract.
                if (type.GetConstructor(Type.EmptyTypes) == null)
                {
                    continue;
                }

                foreach (var @interface in type.GetInterfaces())
                {
                    if (!@interface.IsGenericType)
                    {
                        continue;
                    }

                    if (@interface.GetGenericTypeDefinition() != typeof(IMongoEntityMap<>))
                    {
                        continue;
                    }

                    var target = registerEntityMapMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]);
                    var map = Activator.CreateInstance(type);
                    target.Invoke(map, new[] { Activator.CreateInstance(type) });
                }
            }
        }

        /// <summary>
        /// Registers entity map.
        /// </summary>
        /// <param name="map">Entity map.</param>
        /// <typeparam name="TEntity">Entity.</typeparam>
        public static void RegisterEntityMap<TEntity>(IMongoEntityMap<TEntity> map)
            where TEntity : class
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                BsonClassMap.RegisterClassMap<TEntity>(map.Map);
            }
        }

        /// <summary>
        /// Registers conventions.
        /// </summary>
        public static void RegisterConventions()
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
            };
            const string baseConventions = "Base Conventions";
            ConventionRegistry.Register(baseConventions, conventionPack, _ => true);
        }

        /// <summary>
        /// Registers serializer.
        /// </summary>
        /// <typeparam name="TSerializer">Serializer.</typeparam>
        /// <typeparam name="TValue">Type.</typeparam>
        public static void RegisterSerializer<TSerializer, TValue>()
            where TSerializer : IBsonSerializer<TValue>
        {
            var type = typeof(TSerializer);
            var serializer = (IBsonSerializer<TValue>)Activator.CreateInstance(type);
            BsonSerializer.RegisterSerializer(serializer);
        }
    }
}