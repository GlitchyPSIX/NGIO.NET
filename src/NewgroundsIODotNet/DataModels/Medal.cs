using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Newgrounds Medal.
    /// </summary>
    public struct Medal {
        /// <summary>
        /// Description of the Medal
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; }
        /// <summary>
        /// Difficulty of the Medal.
        /// </summary>
        [JsonProperty("difficulty")]
        public MedalDifficulty Difficulty { get; }

        /// <summary>
        /// Icon URL for the Medal.
        /// </summary>
        [JsonProperty("icon")]
        public string MedalIcon { get; }

        /// <summary>
        /// ID of the Medal.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Name of the Medal
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// Whether this Medal is Secret
        /// </summary>
        [JsonProperty("secret")]
        public bool Secret { get; }

        /// <summary>
        /// Value of the medal in Medal Score. 0 if it has not been unlocked by anyone.
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; }

        /// <summary>
        /// Have you already unlocked this Medal?
        /// </summary>
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