using Newtonsoft.Json;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.ScoreBoard {
    /// <summary>
    /// Request to post a score to a ScoreBoard.
    /// </summary>
    public class ScoreBoardPostScoreRequest : INgioComponentRequest {
        public string Component => "ScoreBoard.postScore";
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        public bool RequiresSecureCall => true;

        /// <summary>
        /// ID of the ScoreBoard to post to.
        /// </summary>
        [JsonIgnore]
        public int Id {
            get => (int)Parameters["id"];
            set => Parameters["id"] = value;
        }

        /// <summary>
        /// Value to post.
        /// </summary>
        /// <remarks><para>
        /// <b>NOTE:</b> Depending on your Scoreboard display setting, the argument <c>amount</c> might be interpreted differently:
        /// </para>
        /// <para>
        /// - Simple: interpreted as-is, displayed as-is.<br/>
        /// - Decimal: interpreted as-is, displayed as  <c>amount</c> divided by 100.<br/>
        /// - Currency: interpreted as-is, displayed like Decimal with a "$" symbol as prefix.<br/>
        /// - Time: <c>amount</c> is interpreted as milliseconds, displayed as HH:mm:ss.SS .<br/>
        /// - Distance: <c>amount</c> is interpreted as inches, displayed as ft' in''.<br/>
        /// - Metric Distance (m): <c>amount</c> is interpreted as centimeters, displayed as meters.<br/>
        /// - Metric distance (km): <c>amount</c> is interpreted as meters, displayed as kilometers.
        /// </para></remarks>
        [JsonIgnore]
        public int Value {
            get => (int)Parameters["value"];
            set => Parameters["value"] = value;
        }

        /// <summary>
        /// Tag that can be used to filter scores by.
        /// </summary>
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