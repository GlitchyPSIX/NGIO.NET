using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Gateway {
    /// <summary>
    /// Request to get the version of the NG API Gateway.
    /// </summary>
    /// <remarks>This component has no parameters.</remarks>
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