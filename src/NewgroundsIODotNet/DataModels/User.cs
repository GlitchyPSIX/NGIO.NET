using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Represents a user.
    /// </summary>
    public struct User {

        /// <summary>
        /// The user's numeric ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The user's textual name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns true if the user has a Newgrounds Supporter upgrade.
        /// </summary>
        [JsonProperty("supporter")]
        public bool IsSupporter { get; set; }

        /// <summary>
        /// The user's icon images.
        /// </summary>
        [JsonProperty("icons")]
        public UserIcons Icons { get; set; }
    }
}