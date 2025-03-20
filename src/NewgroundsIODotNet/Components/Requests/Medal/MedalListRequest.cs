using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.Medal {
    public class MedalListRequest : INgioComponentRequest {
        public string Component => "Medal.getList";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        public string AppId {
            get => (string) Parameters["app_id"];
            set => Parameters["app_id"] = value;
        }

        public MedalListRequest(string appId = null) {
            Parameters = new Dictionary<string, object>() {
                {"app_id", null}
            };
            AppId = appId;
        }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public MedalListRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object> { { "app_id", null } };
            Echo = echo;
        }

    }
}