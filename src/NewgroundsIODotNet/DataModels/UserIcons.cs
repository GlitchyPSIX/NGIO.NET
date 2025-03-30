using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Collection of icons for a user.
    /// </summary>
    /// <remarks>The URLs may be in webp format.</remarks>
    public struct UserIcons {
        /// <summary>
        /// URL for the Large sized icon.
        /// </summary>
        [JsonProperty("large")]
        public string Large { get; set; }

        /// <summary>
        /// URL for the Medium sized icon.
        /// </summary>
        [JsonProperty("medium")]
        public string Medium{ get; set; }

        /// <summary>
        /// URL for the Small sized icon.
        /// </summary>
        [JsonProperty("small")]
        public string Small { get; set; }

    }
}