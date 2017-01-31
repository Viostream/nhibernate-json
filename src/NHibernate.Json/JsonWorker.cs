namespace NHibernate.Json
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using Util;

    public class JsonWorker
    {
        public static readonly JsonSerializerSettings Settings;

        static JsonWorker()
        {
            Settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<Newtonsoft.Json.JsonConverter> {new StringEnumConverter()},
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    TypeNameHandling = TypeNameHandling.Auto
                };
        }

        public static JsonSerializerSettings Configure(params Action<JsonSerializerSettings>[] actions)
        {
            actions.ForEach(x => x(Settings));
            return Settings;
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public static void PopulateObject<T>(string json, T obj)
        {
            JsonConvert.PopulateObject(json, obj, Settings);
        }
    }
}