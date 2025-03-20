using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Medal
{
    public class MedalUnlockResponse : INgioComponentResponse {
        public string Component => "Medal.unlock";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// The user's new medal score.
        /// </summary>
        public int MedalScore { get; }

        /// <summary>
        /// The <seealso cref="DataModels.Medal">Medal</seealso> that was unlocked.
        /// </summary>
        public DataModels.Medal Medal { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private MedalUnlockResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            if (!Success) return;

            MedalScore = Convert.ToInt32(data["medal_score"]);
            Medal = ((JObject)data["medal"]).ToObject<DataModels.Medal>();
        }
    }
}