using NEventStore;
using NEventStore.Logging;
using NEventStore.Serialization;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WCA.Core.CQRS
{
    /// <summary>
    /// Custom Json Serializer with NodaTime support turned on by default.
    /// 
    /// Based on JsonSerializer from NEventStore:
    ///   https://github.com/NEventStore/NEventStore/blob/b2c23054eac5ec4b5721827d0f5a2da1e58c88a2/src/NEventStore.Serialization.Json/JsonSerializer.cs
    /// </summary>
    public class NEventStoreJsonSerializer : ISerialize
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(JsonSerializer));

        private readonly IEnumerable<Type> _knownTypes = new[] { typeof(List<EventMessage>), typeof(Dictionary<string, object>) };

        private readonly JsonSerializer _typedSerializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.All,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        private readonly JsonSerializer _untypedSerializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

        public NEventStoreJsonSerializer(params Type[] knownTypes)
        {
            if (knownTypes?.Length == 0)
            {
                knownTypes = null;
            }

            _knownTypes = knownTypes ?? _knownTypes;

            if (Logger.IsDebugEnabled)
            {
                foreach (var type in _knownTypes)
                {
                    Logger.Debug("Registering type '{0}' as a known type.", type);
                }
            }
        }

        public virtual void Serialize<T>(Stream output, T graph)
        {
            if (Logger.IsVerboseEnabled) Logger.Verbose("Serializing object graph of type '{0}'.", typeof(T));
            using (var streamWriter = new StreamWriter(output, Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                Serialize(jsonTextWriter, graph);
            }
        }

        public virtual T Deserialize<T>(Stream input)
        {
            if (Logger.IsVerboseEnabled) Logger.Verbose("Deserializing stream to object of type '{0}'.", typeof(T));
            using (var streamReader = new StreamReader(input, Encoding.UTF8))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return Deserialize<T>(jsonTextReader);
            }
        }

        protected virtual void Serialize(JsonWriter writer, object graph)
        {
            GetSerializer(graph.GetType()).Serialize(writer, graph);
        }

        protected virtual T Deserialize<T>(JsonReader reader)
        {
            Type type = typeof(T);
            return (T)GetSerializer(type).Deserialize(reader, type);
        }

        protected virtual JsonSerializer GetSerializer(Type typeToSerialize)
        {
            if (_knownTypes.Contains(typeToSerialize))
            {
                if (Logger.IsVerboseEnabled) Logger.Verbose("The object to be serialized is of type '{0}'.  Using an untyped serializer for the known type.", typeToSerialize);
                return _untypedSerializer;
            }

            if (Logger.IsVerboseEnabled) Logger.Verbose("The object to be serialized is of type '{0}'.  Using a typed serializer for the unknown type.", typeToSerialize);
            return _typedSerializer;
        }
    }
}