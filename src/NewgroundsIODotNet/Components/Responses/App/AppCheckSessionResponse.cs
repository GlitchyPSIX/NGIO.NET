using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.App
{
    /// <summary>
    /// Response from a session check.
    /// </summary>
    public class AppCheckSessionResponse : INgioComponentResponse, INgioSessionResponse {
        public string Component => "App.checkSession";
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }

        public NgioServerError? Error { get; }
        /// <summary>
        /// Session object returned by the check.
        /// </summary>
        public Session? Session { get; }

        [JsonConstructor]
        private AppCheckSessionResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            else {
                Debug = false;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;
            Session = null;

            if (!Success) return;

            if (data.TryGetValue("session", out object session)) {
                Session = ((JObject)session).ToObject<Session>();
            }

        }
    }
}