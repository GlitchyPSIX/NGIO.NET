using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.App {
    /// <summary>
    /// Request to get the current version of an app.
    /// </summary>
    public class AppGetCurrentVersionRequest : INgioComponentRequest {
        
        public string Component => "App.getCurrentVersion";
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;

        /// <summary>
        /// Current version of the app to check with
        /// </summary>
        [JsonIgnore]
        public string Version { get => (string)Parameters["version"]; set => Parameters["version"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public AppGetCurrentVersionRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "version", "0.0.0" } };
            Echo = echo;
        }

        public AppGetCurrentVersionRequest(string version = "0.0.0") : this(null, null) {
            Version = version;
        }
    }
}