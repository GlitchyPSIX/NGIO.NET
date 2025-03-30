using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Medal {
    /// <summary>
    /// Request to get the Medal Score from the logged-in user.
    /// </summary>
    /// <remarks>This component has no parameters.</remarks>
    public class MedalGetMedalScoreRequest : INgioComponentRequest {
        public bool RequiresSecureCall => false;
        public MedalGetMedalScoreRequest(object echo = null) {
            Echo = echo;
        }

        public string Component => "Medal.getMedalScore";
        public Dictionary<string, object> Parameters => null;
        public object Echo { get; set; }
    }
}