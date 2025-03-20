using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.Converters;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components {
    [JsonConverter(typeof(NgioComponentRequestConverter))]
    public struct NgioGenericComponentRequest : INgioComponentRequest {
        public string Component { get; set; }
        public Dictionary<string, object> Parameters { get; } 
        public object Echo { get; set; }
        public bool RequiresSecureCall { get; set; }

        public NgioGenericComponentRequest(string component, object echo = null) : this() {
            Component = component;
            Echo = echo;
            Parameters = new Dictionary<string, object>();
        }
    }
}