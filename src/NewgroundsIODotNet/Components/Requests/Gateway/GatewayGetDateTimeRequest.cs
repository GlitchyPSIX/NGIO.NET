using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Gateway
{
    public class GatewayGetDateTimeRequest : INgioComponentRequest
    {
        public string Component => "Gateway.getDatetime";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters  => null;
        public object Echo { get; set; }

        public GatewayGetDateTimeRequest(object echo = null) {
            Echo = echo;
        }
    }
}