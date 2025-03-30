using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Responses.App
{
    /// <summary>
    /// Response from a request to end a session.
    /// </summary>
    public class AppEndSessionResponse : INgioComponentResponse {
        public string Component => "App.endSession";
        public bool Success { get; set; }
        public Dictionary<string, object> Data => null;
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        // feels like boilerplate. this is literally one of the two responses with no data returned
        [JsonConstructor]
        private AppEndSessionResponse(bool success, bool debug = false) {
            Success = success;
            Debug = debug;
        }
    }
}