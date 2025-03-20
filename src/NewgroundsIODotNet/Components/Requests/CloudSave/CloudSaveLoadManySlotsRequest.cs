using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.CloudSave {
    public class CloudSaveLoadManySlotsRequest : INgioComponentRequest { // named like that to avoid potential confusion
        public string Component => "CloudSave.loadSlots";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        public string AppId {
            get => (string)Parameters["app_id"];
            set => Parameters["app_id"] = value;
        }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public CloudSaveLoadManySlotsRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object> { {"app_id", null} };
            Echo = echo;
        }

        public CloudSaveLoadManySlotsRequest(string appId = null) : this(parameters: null) {
            AppId = appId;
        }
    }
}