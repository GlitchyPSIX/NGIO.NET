using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public struct UserIcons {
        [JsonProperty("large")]
        public string Large { get; set; }
        [JsonProperty("medium")]
        public string Medium{ get; set; }
        [JsonProperty("small")]
        public string Small { get; set; }

    }
}