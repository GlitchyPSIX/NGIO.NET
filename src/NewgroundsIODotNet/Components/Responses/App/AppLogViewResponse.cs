using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Responses.App
{
    public class AppLogViewResponse : INgioComponentResponse {
        public string Component => "App.logView";
        public bool Success { get; set; }
        public Dictionary<string, object> Data => null;
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        // feels like boilerplate. this is literally one of the two responses with no data returned
        [JsonConstructor]
        private AppLogViewResponse(bool success, bool debug = false) {
            Success = success;
            Debug = debug;
        }
    }
}