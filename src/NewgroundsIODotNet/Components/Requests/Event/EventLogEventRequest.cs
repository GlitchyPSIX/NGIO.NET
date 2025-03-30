using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.Event {
    /// <summary>
    /// Request to log an Event.
    /// </summary>
    public class EventLogEventRequest : INgioComponentRequest {

        public string Component => "Event.logEvent";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        /// <summary>
        /// Host where the game is running
        /// </summary>
        [JsonIgnore]
        public string Host { get => (string)Parameters["host"]; set => Parameters["host"] = value; }

        /// <summary>
        /// Name of the Event to log as per API Tools settings
        /// </summary>
        [JsonIgnore]
        public string EventName { get => (string)Parameters["event_name"]; set => Parameters["event_name"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public EventLogEventRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "host", "localhost" }, { "event_name", null } };
            Echo = echo;
        }

        public EventLogEventRequest(string host, string eventName) : this(parameters: null, null) {
            Host = host;
            EventName = eventName;
        }
    }
}