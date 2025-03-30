using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.App {
    /// <summary>
    /// Request to check if a host has permission to run the game.
    /// </summary>
    public class AppGetHostLicenseRequest : INgioComponentRequest {
        
        public string Component => "App.getHostLicense";
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;

        /// <summary>
        /// Host address that is currently hosting the game
        /// </summary>
        [JsonIgnore]
        public string Host { get => (string)Parameters["host"]; set => Parameters["host"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public AppGetHostLicenseRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "host", "" } };
            Echo = echo;
        }

        public AppGetHostLicenseRequest(string host = "") : this(null, null) {
            Host = host;
        }
    }
}