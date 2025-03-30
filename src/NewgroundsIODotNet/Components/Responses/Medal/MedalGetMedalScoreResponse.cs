using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Medal
{
    public class MedalGetMedalScoreResponse : INgioComponentResponse {
        public string Component => "Medal.getMedalScore"; 
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }
        /// <summary>
        /// Medal score of the user.
        /// </summary>
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