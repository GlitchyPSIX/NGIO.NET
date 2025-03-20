﻿using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.Medal {
    public class MedalUnlockRequest : INgioComponentRequest {
        public string Component => "Medal.unlock";
        public bool RequiresSecureCall => true;
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        public int MedalId {
            get => (int)Parameters["id"];
            set => Parameters["id"] = value;
        }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public MedalUnlockRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object> { { "id", null } };
            Echo = echo;
        }

        public MedalUnlockRequest(int medalId) : this(parameters: null) {
            MedalId = medalId;
        }
    }
}