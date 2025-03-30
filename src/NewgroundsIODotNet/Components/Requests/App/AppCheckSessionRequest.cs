using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.App {
    /// <summary>
    /// Request for a Session to be checked against NG's servers.
    /// </summary>
    public class AppCheckSessionRequest : INgioComponentRequest {
        public string Component => "App.checkSession";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;
    }
}