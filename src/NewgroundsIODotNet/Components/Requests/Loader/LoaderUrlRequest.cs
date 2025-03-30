using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.Enums;
using Newtonsoft.Json;

namespace NewgroundsIODotNet.Components.Requests.Loader
{
    /// <summary>
    /// Request that represents one of the many specific Loader URL types Newgrounds provides.
    /// </summary>
    public class LoaderUrlRequest : INgioComponentRequest
    {
        public bool RequiresSecureCall => false;
        /// <summary>
        /// Type of Loader to represent
        /// </summary>
        [JsonIgnore] private StandardLoaderType LoaderType { get; }
        public string Component
        {
            get
            {
                switch (LoaderType)
                {
                    case StandardLoaderType.AuthorUrl:
                        return "Loader.loadAuthorUrl";
                    case StandardLoaderType.MoreGames:
                        return "Loader.loadMoreGames";
                    case StandardLoaderType.OfficialUrl:
                        return "Loader.loadOfficialUrl";
                    default: // just in case
                    case StandardLoaderType.Newgrounds:
                        return "Loader.loadNewgrounds";
                }
            }
        }

        /// <summary>
        /// Component's parameters.
        /// </summary>
        /// <remarks>
        /// Do <b>NOT</b> modify the "<c>redirect</c>" parameter manually, as it will not return a JSON response if not false and the library only handles JSON responses.
        /// </remarks>
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }

        /// <summary>
        /// Host where the game is running
        /// </summary>
        [JsonIgnore]
        public string Host { get => (string)Parameters["host"]; set => Parameters["host"] = value; }

        /// <summary>
        /// Whether to log this referral call in Stats
        /// </summary>
        [JsonIgnore]
        public bool LogStat { get => (bool)Parameters["log_stat"]; set => Parameters["log_stat"] = value; }


        // NOTE: All NGIO.NET referral redirects default to false. The device is not guaranteed to be a browser, so the JSON response is needed
        // such that any runtime can open the referral URL however it pleases.

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public LoaderUrlRequest(Dictionary<string, object> parameters, object echo = null)
        {
            LoaderType = StandardLoaderType.Newgrounds;
            Parameters = parameters ?? new Dictionary<string, object> {
                {"host", "localhost"},
                {"log_stat", false},
                {"redirect", false}
            };
            Echo = echo;
        }

        public LoaderUrlRequest(StandardLoaderType type, string host, bool logStat = true) {
            Parameters = new Dictionary<string, object> {
                {"host", "localhost"},
                {"log_stat", false},
                {"redirect", false}
            };

            Host = host;
            LogStat = logStat;
            LoaderType = type;
        }
    }
}