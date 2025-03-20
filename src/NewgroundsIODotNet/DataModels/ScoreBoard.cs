using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public readonly struct ScoreBoard {
        [JsonProperty("id")]
        public int Id { get; }

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