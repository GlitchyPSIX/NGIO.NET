using System.Linq;
using System.Runtime.CompilerServices;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;

namespace NewgroundsIODotNet
{
    public class NgioServerResponse {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public NgioServerError? Error { get; set; }

        [JsonProperty("debug")]
        public Debug? Debug { get; set; }

        [JsonProperty("echo")]
        public string Echo { get; set; }

        [JsonProperty("result")]
        public INgioComponentResponse[] Results { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

        // Skipping debug object for brevity

        [JsonConstructor]
        public NgioServerResponse(string appId, bool success, NgioServerError? error, string echo, INgioComponentResponse[] results, string apiVersion, Debug? debug = null) {
            AppId = appId;
            Success = success;
            Error = error;
            Echo = echo;
            Results = results;
            ApiVersion = apiVersion;
            Debug = debug;
        }

        public T GetComponentResult<T>() where T : INgioComponentResponse {
            if (Results.Length == 0 && Results[0] is T) {
                return (T) Results[0];
            }

            return (T)Results.FirstOrDefault(component => component is T);
        }
    }
}