using Newtonsoft.Json;

namespace NewgroundsIODotNet.DataModels {
    public struct Debug {
        /// <summary>
        /// The time, in milliseconds, that it took to execute a request.
        /// </summary>
        [JsonProperty("exec_time")] public string ExecutionTime { get; }
        /// <summary>
        /// A copy of the <b>NON DESERIALIZED</b> request object that was posted to the server.
        /// </summary>
        [JsonProperty("request")] public object Request { get; }
    }
}