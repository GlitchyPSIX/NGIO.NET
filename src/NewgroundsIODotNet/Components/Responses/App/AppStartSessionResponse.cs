using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.App
{
    public class AppStartSessionResponse : INgioComponentResponse, INgioSessionResponse {
        public string Component => "App.startSession";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        public Session? Session { get; }

        [JsonConstructor]
        private AppStartSessionResponse(Dictionary<string, object> data) {
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