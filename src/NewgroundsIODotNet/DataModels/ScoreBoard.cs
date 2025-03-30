using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Represents a ScoreBoard, the container for scores.
    /// </summary>
    public readonly struct ScoreBoard {
        /// <summary>
        /// ID of the ScoreBoard
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Name of the ScoreBoard
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        [JsonConstructor]
        public ScoreBoard(int id, string name) {
            Id = id;
            Name = name;
        }

        public override string ToString() {
            return $"< (#{Id}) Scoreboard: {Name} >";
        }
    }
}