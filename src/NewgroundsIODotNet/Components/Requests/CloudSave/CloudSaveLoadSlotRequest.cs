using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.CloudSave {
    public class CloudSaveLoadSlotRequest : INgioComponentRequest {
        public string Component => "CloudSave.loadSlot";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        public int CloudSaveId {
            get => (int)Parameters["id"];
            set => Parameters["id"] = value;
        }

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
        public CloudSaveLoadSlotRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object> { { "id", null }, {"app_id", null} };
            Echo = echo;
        }

        public CloudSaveLoadSlotRequest(int saveId, string appId = null) : this(parameters: null) {
            CloudSaveId = saveId;
            AppId = appId;
        }
    }
}