using System.Collections.Generic;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Interfaces
{
    public interface INgioComponentResponse
    {
        /// <summary>
        /// Component's name.
        /// </summary>
        [JsonProperty("component")]
        string Component { get; }

        /// <summary>
        /// Whether this component's response returned a success.
        /// </summary>
        [JsonProperty("success")]
        bool Success { get; }

        /// <summary>
        /// Component's response data.
        /// </summary>
        [JsonProperty("data")]
        Dictionary<string, object> Data { get; }
        
        /// <summary>
        /// Whether debug mode was enabled
        /// </summary>
        [JsonProperty("debug")]
        bool Debug { get; }

        /// <summary>
        /// If success is <c>false</c>, this will contain error information.
        /// </summary>
        [JsonProperty("error")]
        NgioServerError? Error { get; }
    }
}