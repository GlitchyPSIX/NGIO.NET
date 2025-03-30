using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Medal
{
    /// <summary>
    /// Response to a request to get the Scoreboards of the game.
    /// </summary>
    public class ScoreBoardGetBoardsResponse : INgioComponentResponse {
        public string Component => "ScoreBoard.getBoards";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// Received Scoreboards
        /// </summary>
        public DataModels.ScoreBoard[] ScoreBoards { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private ScoreBoardGetBoardsResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            if (!Success) return;

            ScoreBoards = ((JArray)data["scoreboards"]).ToObject<DataModels.ScoreBoard[]>();
        }
    }
}