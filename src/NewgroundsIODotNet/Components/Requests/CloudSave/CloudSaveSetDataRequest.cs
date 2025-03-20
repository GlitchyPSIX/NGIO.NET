using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.CloudSave {
    public class CloudSaveSetDataRequest : INgioComponentRequest {

        public string Component => "CloudSave.setData";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        [JsonIgnore]
        public int Id { get => (int)Parameters["id"]; set => Parameters["id"] = value; }

        [JsonIgnore]
        public string Data { get => (string)Parameters["data"]; set => Parameters["data"] = value; }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public CloudSaveSetDataRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() { { "id", null }, {"data", ""} };
            Echo = echo;
        }

        public CloudSaveSetDataRequest(string data, int id) : this(parameters: null) {
            Data = data ?? throw new ArgumentException("CloudSave.setData: Data is required, must not be null.");
            Id = id;
        }
    }
}