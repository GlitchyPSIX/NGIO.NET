using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;

namespace NewgroundsIODotNet.Components.Requests.Medal {
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