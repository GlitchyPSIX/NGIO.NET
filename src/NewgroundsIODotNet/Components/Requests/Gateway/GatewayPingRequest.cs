using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Gateway {
    /// <summary>
    /// The humble ping request.
    /// </summary>
    /// <remarks>This component has no parameters.</remarks>
    public class GatewayPingRequest : INgioComponentRequest {
        public string Component => "Gateway.ping";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;

        public GatewayPingRequest(object echo = null) {
            Echo = echo;
        }
    }
}