using Newtonsoft.Json;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.ScoreBoard {
    public class ScoreBoardPostScoreRequest : INgioComponentRequest {
        public string Component => "ScoreBoard.postScore";
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        public bool RequiresSecureCall => true;

        [JsonIgnore]
        public int Id {
            get => (int)Parameters["id"];
            set => Parameters["id"] = value;
        }

        [JsonIgnore]
        public int Value {
            get => (int)Parameters["value"];
            set => Parameters["value"] = value;
        }

        [JsonIgnore]
        public string Tag {
            get => (string)Parameters["tag"];
            set => Parameters["tag"] = value;
        }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public ScoreBoardPostScoreRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() {
                {"id", null},
                {"tag", null},
                {"value", 0}
            };
            Echo = echo;
        }

        public ScoreBoardPostScoreRequest(int id, int value, string tag = null) : this(null, null) {
            Value = value;
            Tag = tag;
            Id = id;
        }

        public ScoreBoardPostScoreRequest(DataModels.ScoreBoard board, int value, string tag = null) : this(board.Id, value, tag) { }
    }
}