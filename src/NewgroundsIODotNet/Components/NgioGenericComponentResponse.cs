using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;

namespace NewgroundsIODotNet.Components
{
    public struct NgioGenericComponentResponse : INgioComponentResponse {
        public string Component { get; set; }
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        public NgioGenericComponentResponse(string component) : this() {
            Component = component;
            Data = new Dictionary<string, object>();
        }
    }
}