using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Medal
{
    /// <summary>
    /// Response to a request to get a list of Medals.
    /// </summary>
    public class MedalListResponse : INgioComponentResponse {
        public string Component => "Medal.getList";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// The App ID of <b>another</b>, approved app to load medals from.
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// List of Medals received
        /// </summary>
        public DataModels.Medal[] Medals { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private MedalListResponse(Dictionary<string, object> data) {
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
            
            Medals = ((JArray)data["medals"]).ToObject<DataModels.Medal[]>();
        }
    }
}