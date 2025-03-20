using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.App {
    public class AppLogViewRequest : INgioComponentRequest {
        
        public string Component => "App.logView";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        [JsonIgnore]
        public string Host { get => (string)Parameters["host"]; set => Parameters["host"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public AppLogViewRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "host", "localhost" } };
            Echo = echo;
        }

        public AppLogViewRequest(string host) : this(null, null) {
            Host = host;
        }
    }
}