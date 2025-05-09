﻿using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.App
{
    /// <summary>
    /// Response from a game version check.
    /// </summary>
    public class AppGetCurrentVersionResponse : INgioComponentResponse {
        public string Component => "App.getCurrentVersion";
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        /// <summary>
        /// Whether the version provided is considered deprecated
        /// </summary>
        [JsonIgnore]
        public bool ClientDeprecated { get; }

        /// <summary>
        /// The current version of the game
        /// </summary>
        [JsonIgnore]
        public string Version { get; }

        [JsonConstructor]
        private AppGetCurrentVersionResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            else {
                Debug = false;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;
            ClientDeprecated = false;
            Version = "";


            if (!Success) return;

            if (data.TryGetValue("client_deprecated", out object deprecated)) {
                ClientDeprecated = ((bool)deprecated);
            }

            if (data.TryGetValue("version", out object version)) {
                Version = ((JObject)version).ToObject<string>();
            }

        }
    }
}