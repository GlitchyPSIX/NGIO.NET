using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.App {
    public class AppCheckSessionRequest : INgioComponentRequest {
        public string Component => "App.checkSession";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;
    }
}