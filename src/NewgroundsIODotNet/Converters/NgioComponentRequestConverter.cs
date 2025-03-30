using System;
using System.Linq;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Converters {
    /// <summary>
    /// Converts a Component Request to a suitable JSON using JSON.NET. Does not read JSON.
    /// </summary>
    public class NgioComponentRequestConverter : JsonConverter<INgioComponentRequest> {
        public override bool CanRead => false; // Request objects are only intended to be written to JSON for sending purposes
        public override void WriteJson(JsonWriter writer, INgioComponentRequest value, JsonSerializer serializer) {
            JObject newObj = new JObject(
                new JProperty("component", value.Component)
            );
            if (value.Parameters != null) newObj.Add(new JProperty("parameters", JObject.FromObject(value.Parameters)));
            if (value.Echo != null) newObj.Add(new JProperty("echo", value.Echo));
            newObj.Add("_ngionet_requiresSecure", value.RequiresSecureCall);
            newObj.WriteTo(writer, serializer.Converters.ToArray());
            writer.Flush();
        }

        public override INgioComponentRequest ReadJson(JsonReader reader, Type objectType, INgioComponentRequest existingValue,
            bool hasExistingValue, JsonSerializer serializer) {
            return null;
        }
    }
}