using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;

namespace NewgroundsIODotNet {
    /// <summary>
    /// <b>Don't instantiate this class directly. Let a <seealso cref="NgioCommunicator">NgioCommunicator</seealso> implementation handle that.</b>
    /// </summary>
    public class NgioServerRequest {

        /// <summary>
        /// Sets whether the server request is Secure. Used for the JSON converter to determine whether to serialize as encrypted.
        /// </summary>
        [JsonIgnore]
        public SecurityLevel SecurityLevel { get; set; }

        /// <summary>
        /// Whether the server request is coming from an app with encryption enabled.
        /// </summary>
        [JsonIgnore]
        public bool Secured { get; }

        /// <summary>
        /// Your application's unique ID. Set from the Communicator's information.
        /// </summary>
        [JsonProperty("app_id")]
        public string AppId { get; }

        /// <summary>
        /// An Execute <seealso cref="INgioComponentRequest">(component request)</seealso> object ,or array of one-or-more Execute objects.
        /// </summary>
        [JsonProperty("execute")]
        public INgioComponentRequest[] ExecutedComponents { get; set; }

        /// <summary>
        /// Session ID to be used with this request. <b>Only set manually if you know what you're doing.</b>
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// If set to <c>true</c>, calls will be executed in debug mode.
        /// </summary>
        [JsonProperty("debug")]
        public bool Debug { get; set; }

        /// <summary>
        /// An optional value that will be returned, verbatim, in the <seealso cref="NgioServerResponse"/> object.
        /// </summary>
        [JsonProperty("echo")]
        public object Echo { get; set; }

        public NgioServerRequest(NgioCommunicator communicator, Session? forceSession) {
            AppId = communicator.AppId;
            Secured = communicator.Secured;
            SessionId = forceSession?.Id ?? communicator.Session?.Id;
        }
    }
}