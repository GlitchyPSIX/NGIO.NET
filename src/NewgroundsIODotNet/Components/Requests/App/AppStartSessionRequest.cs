using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.App {
    public class AppStartSessionRequest : INgioComponentRequest {
        
        public string Component => "App.startSession";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        [JsonIgnore]
        public bool Force { get => (bool)Parameters["force"]; set => Parameters["force"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public AppStartSessionRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "force", false } };
            Echo = echo;
        }

        public AppStartSessionRequest(bool force) : this(null, null) {
            Force = force;
        }
    }
}