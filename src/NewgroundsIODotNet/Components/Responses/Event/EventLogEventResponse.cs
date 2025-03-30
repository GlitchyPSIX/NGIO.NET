using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Event
{
    /// <summary>
    /// One Logging response to rule them all (they all have the same response.)
    /// </summary>
    public class EventLogEventResponse : INgioComponentResponse
    {
        /// <summary>
        /// The value returned by this property in this class is not standard in NG documentation because this is a combined response.
        /// </summary>
        public string Component => "Event.logEvent";
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }
        /// <summary>
        /// Event name that was logged
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// <b>You shouldn't use this constructor directly.</b>
        /// </summary>
        [JsonConstructor]
        private EventLogEventResponse(string component, Dictionary<string, object> data)
        {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug))
            {
                Debug = (bool)debug;
            }
            else
            {
                Debug = false;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;

            EventName = ((JObject)data["event_name"]).ToObject<string>();
        }
    }
}