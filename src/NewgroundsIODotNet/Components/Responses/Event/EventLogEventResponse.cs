using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.Event
{
    /// <summary>
    /// One Loader URL response to rule them all (they all have the same response.)
    /// </summary>
    public class EventLogEventResponse : INgioComponentResponse
    {
        public string Component => "Event.logEvent"; // Nonstandard in NG documentation, but I'm making DX more fun by not exaggerating the type amount.
        public bool Success { get; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }
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