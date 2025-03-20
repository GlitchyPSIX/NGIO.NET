using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Gateway {
    public class GatewayGetVersionRequest : INgioComponentRequest {
        public string Component => "Gateway.getVersion";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;

        public GatewayGetVersionRequest(object echo = null) {
            Echo = echo;
        }
    }
}