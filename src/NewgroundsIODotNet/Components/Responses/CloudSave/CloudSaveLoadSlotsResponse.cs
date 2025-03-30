using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.CloudSave
{
    /// <summary>
    /// Response to a request to load Cloud Save slots.
    /// </summary>
    public class CloudSaveLoadSlotsResponse : INgioComponentResponse {
        public string Component => "CloudSave.loadSlots";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// The App ID of <b>another</b>, approved app to load slot data from.
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// Received Cloud Save slots
        /// </summary>
        public DataModels.SaveSlot[] Slots { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private CloudSaveLoadSlotsResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            if (!Success) return;

            if (data.TryGetValue("app_id", out object appid)) {
                AppId = ((JObject)appid).ToObject<string>();
            }
            
            Slots = ((JArray)data["slots"]).ToObject<DataModels.SaveSlot[]>();
        }
    }
}