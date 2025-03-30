using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.Components.Responses.Gateway;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using NewgroundsIODotNet.Components.Responses.App;
using NewgroundsIODotNet.Components.Responses.CloudSave;
using NewgroundsIODotNet.Components.Responses.Loader;
using NewgroundsIODotNet.Components.Responses.Medal;
using NewgroundsIODotNet.Components.Responses.Event;
using NewgroundsIODotNet.Components.Responses.ScoreBoard;

namespace NewgroundsIODotNet.Converters
{
    /// <summary>
    /// Converts a Component Response from JSON using JSON.NET. Does not write JSON.
    /// </summary>
    public class NgioComponentResponseConverter : JsonConverter<INgioComponentResponse> {
        private static readonly Dictionary<string, Type> TypeBinding = new Dictionary<string, Type> {
            {"Gateway.getDatetime", typeof(GatewayGetDateTimeResponse)},
            {"Gateway.getVersion", typeof(GatewayGetVersionResponse)},
            {"Gateway.ping", typeof(GatewayPingResponse)},
            {"App.checkSession", typeof(AppCheckSessionResponse)},
            {"App.startSession", typeof(AppStartSessionResponse)},
            {"App.endSession", typeof(AppEndSessionResponse)},
            {"App.getHostLicense", typeof(AppGetHostLicenseResponse)},
            {"App.logView", typeof(AppLogViewResponse)},
            {"App.getCurrentVersion", typeof(AppGetCurrentVersionResponse)},
            {"CloudSave.clearSlot", typeof(CloudSaveSlotResponse)},
            {"CloudSave.loadSlot", typeof(CloudSaveSlotResponse)},
            {"CloudSave.loadSlots", typeof(CloudSaveLoadSlotsResponse)},
            {"CloudSave.setData", typeof(CloudSaveSlotResponse)},
            {"Loader.loadAuthorUrl", typeof(LoaderUrlResponse)},
            {"Loader.loadMoreGames", typeof(LoaderUrlResponse)},
            {"Loader.loadNewgrounds", typeof(LoaderUrlResponse)},
            {"Loader.loadOfficialUrl", typeof(LoaderUrlResponse)},
            {"Loader.loadReferral", typeof(LoaderUrlResponse)},
            {"Medal.getList", typeof(MedalListResponse)},
            {"Medal.getMedalScore", typeof(MedalGetMedalScoreResponse)},
            {"Medal.unlock", typeof(MedalUnlockResponse)},
            {"ScoreBoard.getBoards", typeof(ScoreBoardGetBoardsResponse)},
            {"ScoreBoard.getScores", typeof(ScoreBoardGetScoresResponse)},
            {"ScoreBoard.postScore", typeof(ScoreBoardPostScoreResponse)},
            {"Event.logEvent", typeof(EventLogEventResponse)}
        };
        public override void WriteJson(JsonWriter writer, INgioComponentResponse value, JsonSerializer serializer) { }

        public override INgioComponentResponse ReadJson(JsonReader reader, Type objectType, INgioComponentResponse existingValue,
            bool hasExistingValue, JsonSerializer serializer) {
            JObject jObj = JObject.Load(reader);
            string componentName = jObj.GetValue("component")?.ToString();

            if (componentName == null)
                throw new ArgumentException("Invalid component returned by NG.IO server (null).");

            if (!TypeBinding.TryGetValue(componentName, out Type componentType))
                throw new ArgumentException($"Component {componentName} has no binding set in the Converter.");

            return (INgioComponentResponse)jObj.ToObject(componentType);
        }
    }
}