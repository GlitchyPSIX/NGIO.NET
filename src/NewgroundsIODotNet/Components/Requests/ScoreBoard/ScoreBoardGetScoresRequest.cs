﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using NewgroundsIODotNet.Enums;

namespace NewgroundsIODotNet.Components.Requests.ScoreBoard {
    /// <summary>
    /// Request to get the Scores from a Scoreboard
    /// </summary>
    public class ScoreBoardGetScoresRequest : INgioComponentRequest {
        public string Component => "ScoreBoard.getScores";
        public Dictionary<string, object> Parameters { get; }
        public object Echo { get; set; }
        public bool RequiresSecureCall => false;

        /// <summary>
        /// App ID of another game to get the scores from. Set to null to use the current game
        /// </summary>
        [JsonIgnore]
        public string AppId {
            get => (string)Parameters["app_id"];
            set => Parameters["app_id"] = value;
        }

        /// <summary>
        /// ID of the ScoreBoard to get scores from.
        /// </summary>
        [JsonIgnore]
        public int Id {
            get => (int)Parameters["id"];
            set => Parameters["id"] = value;
        }

        /// <summary>
        /// How many score entries to fetch
        /// </summary>
        [JsonIgnore]
        public int Limit {
            get => (int)Parameters["limit"];
            set => Parameters["limit"] = value;
        }

        private ScoreBoardPeriod _period;

        /// <summary>
        /// Period of time to get the scores from
        /// </summary>
        [JsonIgnore]
        public ScoreBoardPeriod Period {
            get => _period;
            set {
                switch (value) {
                    case ScoreBoardPeriod.Day:
                        Parameters["period"] = "D";
                        break;
                    case ScoreBoardPeriod.Week:
                        Parameters["period"] = "W";
                        break;
                    case ScoreBoardPeriod.Month:
                        Parameters["period"] = "M";
                        break;
                    case ScoreBoardPeriod.Year:
                        Parameters["period"] = "Y";
                        break;
                    case ScoreBoardPeriod.Lifetime:
                        Parameters["period"] = "A";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                _period = value;
            }
        }

        /// <summary>
        /// How many scores to skip before fetching
        /// </summary>
        [JsonIgnore]
        public int Skip {
            get => (int)Parameters["skip"];
            set => Parameters["skip"] = value;
        }

        /// <summary>
        /// Whether the scores will only include the logged in user and their friends
        /// </summary>
        /// <remarks>Useless if there's no Session with a specified User.</remarks>
        [JsonIgnore]
        public bool Social {
            get => (bool)Parameters["social"];
            set => Parameters["social"] = value;
        }

        /// <summary>
        /// Tag to filter scores by.
        /// </summary>
        [JsonIgnore]
        public string FilterTag {
            get => (string)Parameters["tag"];
            set => Parameters["tag"] = value;
        }

        /// <summary>
        /// ID or Username of the User to get local stores in relation to.
        /// </summary>
        [JsonIgnore]
        public string User {
            get => (string)Parameters["user"];
            set => Parameters["user"] = value;
        }

        /// <summary>
        /// Use this constructor only if you know what you're doing.
        /// </summary>
        /// <param name="parameters">Manually provided component parameters</param>
        /// <param name="echo">NGIO Echo response</param>
        [JsonConstructor]
        public ScoreBoardGetScoresRequest(Dictionary<string, object> parameters, object echo = null) {
            Parameters = parameters ?? new Dictionary<string, object>() {
                {"app_id", null},
                {"id", null},
                {"limit", 10},
                {"period", "D"},
                {"skip", 0},
                {"social", false},
                {"tag", null},
                {"user", null}
            };
            _period = ScoreBoardPeriod.Day;
            Echo = echo;
        }

        public ScoreBoardGetScoresRequest(int id,
            string appId = null,
            int limit = 10,
            ScoreBoardPeriod period = ScoreBoardPeriod.Day,
            int skip = 0,
            bool social = false,
            string tag = null,
            User? user = null) : this(null, null) {

            Id = id;
            AppId = appId;
            Limit = limit;
            Period = period;
            Skip = skip;
            Social = social;
            FilterTag = tag;
            User = user?.Id.ToString();
        }

        public ScoreBoardGetScoresRequest(DataModels.ScoreBoard board,
            string appId = null,
            int limit = 10,
            ScoreBoardPeriod period = ScoreBoardPeriod.Day,
            int skip = 0,
            bool social = false,
            string tag = null,
            User? user = null) : this(board.Id, appId, limit, period, skip, social, tag, user) { }
    }
}