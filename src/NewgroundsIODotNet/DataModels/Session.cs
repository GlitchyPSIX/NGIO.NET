using System;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    /// <summary>
    /// Represents a user's session.
    /// </summary>
    public struct Session {
        /// <summary>
        /// If true, the session_id is expired. Use App.startSession to get a new one.
        /// </summary>
        [JsonProperty("expired")]
        public Boolean Expired { get; }

        /// <summary>
        /// A unique session identifier
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set;  }

        /// <summary>
        /// If the session has no associated user but is not expired, this property will provide a URL that can be used to sign the user in.
        /// </summary>
        [JsonProperty("passport_url")]
        public string PassportUrl { get; }

        /// <summary>
        /// If true, the user would like you to remember their session id.
        /// </summary>
        [JsonProperty("remember")]
        public bool RememberSession { get; }

        /// <summary>
        /// The user associated with the session.
        /// </summary>
        [JsonProperty("user")]
        public User? User { get; }

        [JsonConstructor]
        public Session(bool expired, string id, string passportUrl, bool rememberSession, User? user) {
            Expired = expired;
            Id = id;
            PassportUrl = passportUrl;
            RememberSession = rememberSession;
            User = user;
        }
    }
}