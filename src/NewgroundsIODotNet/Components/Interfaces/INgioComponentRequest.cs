using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Interfaces
{
    public interface INgioComponentRequest
    {

        /// <summary>
        /// Component's name.
        /// </summary>
        [JsonProperty("component")]
        string Component { get; }

        /// <summary>
        /// Component's parameters. <b>Only supply manually if you know what you're doing.</b>
        /// </summary>
        [JsonProperty("parameters")]
        Dictionary<string, object> Parameters { get; }

        /// <summary>
        /// Value is returned verbatim by the server.
        /// </summary>
        [JsonProperty("echo")]
        object Echo { get; set; }

        /// <summary>
        /// Whether this component will be made into a secure execution call if Encryption is enabled. Stripped from the final JSON.
        /// </summary>
        bool RequiresSecureCall { get; } 
    }
}