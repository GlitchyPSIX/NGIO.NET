using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewgroundsIODotNet.Components.Responses.App
{
    public class AppGetHostLicenseResponse : INgioComponentResponse {
        public string Component => "App.getHostLicense";
        public bool Success { get; set; }
        public Dictionary<string, object> Data { get; }
        public bool Debug { get; }
        public NgioServerError? Error { get; }

        [JsonIgnore]
        public bool HostApproved { get; }

        [JsonConstructor]
        private AppGetHostLicenseResponse(Dictionary<string, object> data) {
            Success = (bool)data["success"];
            if (data.TryGetValue("debug", out object debug)) {
                Debug = (bool)debug;
            }
            else {
                Debug = false;
            }
            if (data.TryGetValue("error", out object err)) {
                Error = ((JObject)err).ToObject<NgioServerError>();
            }
            Data = data;
            HostApproved = true;


            if (!Success) return;

            if (data.TryGetValue("host_approved", out object hostApproved)) {
                HostApproved = ((bool)hostApproved);
            }

        }
    }
}