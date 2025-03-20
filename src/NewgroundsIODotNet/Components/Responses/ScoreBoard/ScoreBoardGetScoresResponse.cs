using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.ScoreBoard
{
    public class ScoreBoardGetScoresResponse : INgioComponentResponse {
        public string Component => "ScoreBoard.getScores";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// The App ID of <b>another</b>, approved app to load medals from.
        /// </summary>
        public string AppId { get; }

        public int Limit { get; }

        public ScoreBoardPeriod Period { get; }

        public Score[] Scores { get; }
        public DataModels.ScoreBoard Scoreboard { get; }

        public bool Social { get; }

        public User? User { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private ScoreBoardGetScoresResponse(Dictionary<string, object> data) {
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
                AppId = (string)appid;
            }

            if (data.TryGetValue("limit", out object limit)) {
                Limit = Convert.ToInt32(limit);
            }

            if (data.TryGetValue("period", out object period)) {
                string periodLetter = (string) period;
                switch (periodLetter) {
                    case "D":
                        Period = ScoreBoardPeriod.Day;
                        break;
                    case "W":
                        Period = ScoreBoardPeriod.Week;
                        break;
                    case "M":
                        Period = ScoreBoardPeriod.Month;
                        break;
                    case "Y":
                        Period = ScoreBoardPeriod.Year;
                        break;
                    case "A":
                        Period = ScoreBoardPeriod.Lifetime;
                        break;
                    default:
                        throw new ArgumentException(
                            $"NGIO.NET: ScoreBoard period {periodLetter} returned by NG is unknown to NGIO.NET");
                }
            }

            if (data.TryGetValue("social", out object social)) {
                Social = (bool)social;
            }

            if (data.TryGetValue("user", out object user)) {
                if (user != null) User = ((JObject)data["user"]).ToObject<User?>();
            }
            Scores = ((JArray)data["scores"]).ToObject<Score[]>();
            Scoreboard = ((JObject)data["scoreboard"]).ToObject<DataModels.ScoreBoard>();
        }
    }
}