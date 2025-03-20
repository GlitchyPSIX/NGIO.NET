using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public struct Medal {
        [JsonProperty("description")]
        public string Description { get; }
        [JsonProperty("difficulty")]
        public MedalDifficulty Difficulty { get; }
        [JsonProperty("icon")]
        public string MedalIcon { get; }
        [JsonProperty("id")]
        public int Id { get; }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("secret")]
        public bool Secret { get; }
        [JsonProperty("value")]
        public int Value { get; }
        [JsonProperty("unlocked")]
        public bool? Unlocked { get; }

        [JsonConstructor]
        public Medal(string description, MedalDifficulty difficulty, string medalIcon, int id, string name, bool secret, int value, bool? unlocked = null) : this() {
            Description = description;
            Difficulty = difficulty;
            MedalIcon = medalIcon;
            Id = id;
            Name = name;
            Secret = secret;
            Value = value;
            Unlocked = unlocked;
        }

        public override string ToString() {
            return $"< Medal: {Name} ({Difficulty}) - Value: {Value} - Unlocked: {Unlocked} >";
        }
    }
}