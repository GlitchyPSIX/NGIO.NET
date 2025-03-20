using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public readonly struct Score {
        [JsonProperty("formatted_value")]
        public string FormattedValue { get; }

        [JsonProperty("tag")]
        public string Tag { get; }

        [JsonProperty("user")]
        public User? User { get; }

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