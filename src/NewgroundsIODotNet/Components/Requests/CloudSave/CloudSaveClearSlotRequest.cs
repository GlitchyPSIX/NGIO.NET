using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.CloudSave {
    /// <summary>
    /// Request to empty a Cloud Save slot.
    /// </summary>
    public class CloudSaveClearSlotRequest : INgioComponentRequest {

        public string Component => "CloudSave.clearSlot";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        /// <summary>
        /// Save Slot ID to clear
        /// </summary>
        [JsonIgnore]
        public int Id { get => (int)Parameters["id"]; set => Parameters["id"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public CloudSaveClearSlotRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "id", null } };
            Echo = echo;
        }

        public CloudSaveClearSlotRequest(int id) : this(parameters: null) {
            Id = id;
        }
    }
}