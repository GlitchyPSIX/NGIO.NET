using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.App {
    /// <summary>
    /// Request to end a Session.
    /// </summary>
    public class AppEndSessionRequest : INgioComponentRequest {
        public string Component => "App.endSession";
        public bool RequiresSecureCall => false;
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
    }
}