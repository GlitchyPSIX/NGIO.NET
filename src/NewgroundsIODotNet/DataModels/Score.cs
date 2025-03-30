using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Represents a singular score.
    /// </summary>
    public readonly struct Score {
        /// <summary>
        /// Formatted value for the Score.
        /// </summary>
        [JsonProperty("formatted_value")]
        public string FormattedValue { get; }

        /// <summary>
        /// Tag attached to the Score
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; }

        /// <summary>
        /// User that holds this Score. If null, assume current player.
        /// </summary>
        [JsonProperty("user")]
        public User? User { get; }

        /// <summary>
        /// Raw integer value of the score.
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; }

        [JsonConstructor]
        public Score(string formattedValue, string tag, int value, User user) {
            FormattedValue = formattedValue;
            Tag = tag;
            User = user;
            Value = value;
        }

        public override string ToString() {
            return $"< {User?.Name ?? "<< no user >>"} ({Tag}) - {FormattedValue} >";
        }
    }
}