using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Gateway {
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