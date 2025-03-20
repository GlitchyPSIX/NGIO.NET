using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Medal
{
    /// <summary>
    /// One Loader URL response to rule them all (they all have the same response.)
    /// </summary>
    public class MedalGetMedalScoreResponse : INgioComponentResponse {
        public string Component => "Medal.getMedalScore"; // Nonstandard in NG documentation, but I'm making DX more fun by not exaggerating the type amount.
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }
        public int MedalScore { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private MedalGetMedalScoreResponse(string component, Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            else {
                Debug = false;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            // json.net assumes it's a Long, which, understandable, but documentation says int
            MedalScore = Convert.ToInt32(data["medal_score"]);
        }
    }
}