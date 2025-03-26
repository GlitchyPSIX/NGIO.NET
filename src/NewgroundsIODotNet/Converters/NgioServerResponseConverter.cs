using System;
using System.Runtime.Serialization;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Converters
{
    public class NgioServerResponseConverter : JsonConverter<NgioServerResponse> {
        public override bool CanWrite => false;
        
        public override void WriteJson(JsonWriter writer, NgioServerResponse value, JsonSerializer serializer) { }

        public override NgioServerResponse ReadJson(JsonReader reader, Type objectType,
            NgioServerResponse existingValue, bool hasExistingValue,
            JsonSerializer serializer) {
            JObject jObj = JObject.Load(reader);

            JToken results = jObj.GetValue("result");

            if (results is JObject resultObj) {
                // JObject?
                // NGIO will return a single object when only one execution was sent
                // as an object (not array)
                // this creates an array and removes the old property to make way
                // for the array to comply with the structure of the response class
                JArray newResultArr = new JArray(
                    resultObj.DeepClone() );
                jObj.Remove("result");
                jObj.Add("result", newResultArr);
            }

            NgioServerResponse returnedResponse = new NgioServerResponse(
                jObj.GetValue("app_id").ToString(),
                jObj.GetValue("success").ToObject<bool>(),
                jObj.GetValue("error")?.ToObject<NgioServerError?>(),
                jObj.GetValue("echo")?.ToString(),
                jObj.GetValue("result")?.ToObject<INgioComponentResponse[]>(serializer) ?? Array.Empty<INgioComponentResponse>(), // null response, empty array
                jObj.GetValue("api_version")?.ToString(),
                jObj.GetValue("debug")?.ToObject<Debug?>()
                );

            return returnedResponse;

        }
    }
}