using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.Loader
{
    public class LoaderReferralUrlRequest : INgioComponentRequest
    {
        public string Component => "Loader.loadReferral";
        public bool RequiresSecureCall => false;

        /// <summary>
        /// Component's parameters.
        /// </summary>
        /// <remarks>
        /// Do <b>NOT</b> modify the "<c>redirect</c>" parameter manually, as it will not return a JSON response if false and the library only handles JSON responses.
        /// </remarks>
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        

        [JsonIgnore]
        public string Host { get => (string)Parameters["host"]; set => Parameters["host"] = value; }

        [JsonIgnore]
        public bool LogStat { get => (bool)Parameters["log_stat"]; set => Parameters["log_stat"] = value; }

        [JsonIgnore]
        public string ReferralName { get => (string)Parameters["referral_name"]; set => Parameters["referral_name"] = value; }


        // NOTE: All NGIO.NET referral redirects default to false. This is not guaranteed to be a browser, you need the JSON response.

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public LoaderReferralUrlRequest(Dictionary<string, object> parameters, object echo = null)
        {
            Parameters = parameters ?? new Dictionary<string, object> {
                {"host", "localhost"},
                {"log_stat", false},
                {"redirect", false},
                {"referral_name", ""}
            };
            Echo = echo;
        }

        public LoaderReferralUrlRequest(string host, string referralName, bool logStat = true) {
            Parameters = new Dictionary<string, object> {
                {"host", "localhost"},
                {"log_stat", false},
                {"redirect", false},
                {"referral_name", ""}
            };

            Host = host;
            LogStat = logStat;
            ReferralName = referralName;
        }
    }
}