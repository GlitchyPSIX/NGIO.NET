﻿using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.ScoreBoard
{
    /// <summary>
    /// Response to posting to a Scoreboard.
    /// </summary>
    public class ScoreBoardPostScoreResponse : INgioComponentResponse {
        public string Component => "ScoreBoard.postScore";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// Score that was just posted.
        /// </summary>
        public Score Score { get; }

        /// <summary>
        /// ScoreBoard that was just posted to.
        /// </summary>
        public DataModels.ScoreBoard ScoreBoard { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private ScoreBoardPostScoreResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            if (!Success) return;

            
            Score = ((JObject)data["score"]).ToObject<Score>();
            ScoreBoard = ((JObject)data["scoreboard"]).ToObject<DataModels.ScoreBoard>();
        }
    }
}