using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.DataModels;
using NewgroundsIODotNet.Enums;
using NewgroundsIODotNet.Logging;
using Newtonsoft.Json;

namespace NewgroundsIODotNet
{
    /// <summary>
    /// Communicator implementation for use in Desktop/Task supported platforms (Windows, Linux, Unity Desktop).
    /// </summary>
    /// <remarks>Do <b>NOT</b> use with WebGL runtimes unless Task/<c>await async</c> is supported by the target platform. Usage on Mobile has not been tested.</remarks>
    public class NgioTaskCommunicator : NgioCommunicator {
        private readonly HttpClient _httpClient = new HttpClient();
        private CancellationTokenSource _cToken = new CancellationTokenSource();
        private float _heartbeatLength;

        /// <summary>
        /// Creates a new Desktop Communicator instance.
        /// </summary>
        /// <param name="appId">Required. The App ID of your Newgrounds Game project.</param>
        /// <param name="encryptionKey">Required if you want to use medals. The Encryption Key of your Newgrounds Game project.</param>
        /// <param name="appVersion">App version as defined in your Game project's API Tools. Defaults to "0.0.0".</param>
        /// <param name="debugMode">Whether to enable the debug mode for extra NGIO developer-related info.</param>
        /// <param name="preloadMedals">Whether to preload Medals from the server. Required to use the Medal methods.</param>
        /// <param name="preloadScores">Whether to preload Scoreboards from the server. Required to use the Score methods.</param>
        /// <param name="logViewOnInit">Whether to log a view when the Communicator is initialized.</param>
        /// <param name="sessionId">Optional session ID to pass and use right after initialization. Usually given by Newgrounds through the browser.</param>
        /// <param name="host">Where your game is running. Extract this from the browser's URI. Defaults to "localhost".</param>
        public NgioTaskCommunicator(string appId, string encryptionKey, string appVersion, bool debugMode = false,
            bool preloadMedals = false, bool preloadScores = false, bool logViewOnInit = true, string sessionId = null, string host = "localhost") : base(appId, encryptionKey, appVersion, debugMode,
            preloadMedals, preloadScores, true, sessionId, host) {
            _httpClient.BaseAddress = new Uri(NewgroundsGatewayUrl);
        }

        /// <summary>
        /// Fetches the data from a Save Slot, if populated.
        /// </summary>
        /// <remarks>All Save Slot data is saved and fetched as <see cref="string">string</see>.</remarks>
        /// <param name="slot">The Save Slot from which to get data.</param>
        /// <param name="responseCallback">The function to execute with the save data, if any.</param>
        public override void GetSaveSlotData(SaveSlot slot, Action<string> responseCallback = null) {
            Task.Run(async () => {
                string data = await GetSaveSlotDataAsync(slot);
                LastSlotLoaded = slot;
                responseCallback?.Invoke(data);
            }).ContinueWith((t) => {
                if (t.IsFaulted)
                    if (t.Exception != null)
                        throw t.Exception;
            });
        }

        /// <summary>
        /// Fetches a slot's data asynchronously, if populated.
        /// </summary>
        /// <param name="slot">The Save Slot from which to get data.</param>
        /// <returns>The data of the Save Slot if populated, null if nothing.</returns>
        /// <exception cref="HttpRequestException">Thrown if the request fails for any reason.</exception>
        public async Task<string> GetSaveSlotDataAsync(SaveSlot slot) {
            if (slot.Url == null) return null;
            string resp = null;
            try {
                resp = await _httpClient.GetStringAsync(slot.Url);
            }
            catch (Exception ex) {
                throw new HttpRequestException($"NGIO.NET: Trying to get the data from save slot #{slot.Id} failed due to an error with the HTTP client: {ex.Message}.", ex);
            }
            return resp;
        }

        /// <summary>
        /// Opens an URL with shell execution. Intended to open the browser for an URl given from referral or NG Passport.
        /// </summary>
        /// <param name="url">The URL to open</param>
        public override void OpenUrl(string url) {
            ProcessStartInfo pInfo = new ProcessStartInfo(url) {
                UseShellExecute = true
            };
            Process.Start(pInfo);
        }

        private async Task InfiniteHeartbeat(CancellationToken cToken) {
            while (!cToken.IsCancellationRequested) {
                HeartBeat();
                await Task.Delay((int)(_heartbeatLength * 1000), cToken); // technically this could've been implemented to use another Task, but eh.
            }
        }

        public override void StartHeartbeat(float seconds = 10) {
            _heartbeatLength = seconds;
            if (_cToken.IsCancellationRequested) _cToken = new CancellationTokenSource();
            Task.Run(() => InfiniteHeartbeat(_cToken.Token));
        }

        public override void SetHeartbeatSpeed(float newSeconds) {
            _heartbeatLength = newSeconds;
        }

        public override void StopHeartbeat() {
            _cToken.Cancel();
        }

        public override void SendRequest(INgioComponentRequest[] components, Action<NgioServerResponse> callback, Session? forceSession = null) {
            Task.Run(async () => {
                NgioServerResponse resp = await SendRequestAsync(components, forceSession);
                callback?.Invoke(resp);
                return resp;
            }).ContinueWith((t) => {
                if (t.IsFaulted)
                    if (t.Exception != null)
                        throw t.Exception;
            });
        }

        public override void SendSecureRequest(INgioComponentRequest component, Action<NgioServerResponse> callback) {
            Task.Run(async () => {
                NgioServerResponse resp = await SendSecureRequestAsync(component);
                callback?.Invoke(resp);
                return resp;
            }).ContinueWith((t) => {
                if (t.IsFaulted)
                    if (t.Exception != null)
                        throw t.Exception;
            });
        }

        public async Task<NgioServerResponse> SendRequestAsync(INgioComponentRequest[] components, Session? forceSession = null) {
            return await InternalSendRequestAsync(components, false, forceSession);
        }

        public async Task<NgioServerResponse> SendSecureRequestAsync(INgioComponentRequest component) {
            return await InternalSendRequestAsync(new[] { component }, true);
        }

        private async Task<NgioServerResponse> InternalSendRequestAsync(INgioComponentRequest[] components,
            bool forceSecure, Session? forceSession = null) {
            NgioServerRequest newRequest = new NgioServerRequest(this, forceSession) {
                Debug = DebugMode,
                SecurityLevel = SecurityLevel.None,
                ExecutedComponents = components
            };

            if (Secured) {
                newRequest.SecurityLevel = forceSecure ? SecurityLevel.ForceAll : SecurityLevel.OnlyRequired;
            }

            string jsonToSend = JsonConvert.SerializeObject(newRequest, RequestConverter, SecureRequestConverter);
            // while NGIO supports sending components over the querystring (see documentation), it's more reliable to send as form fields.
            HttpResponseMessage resp = null;
            try {
                resp = await _httpClient.PostAsync("",
                    new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("request", jsonToSend) }));
                LastExecution = DateTime.Now;
            }
            catch {
                OnServerUnavailable();
                StopHeartbeat();
                ConnectionStatus = ConnectionStatus.ServerUnreachable;
            }

            if (resp.IsSuccessStatusCode) {
                string json;
                using (
                    StreamReader ms = new StreamReader(await resp.Content.ReadAsStreamAsync())) {
                    json = await ms.ReadToEndAsync();
                }

                NgioServerResponse deserializedResp =
                    JsonConvert.DeserializeObject<NgioServerResponse>(json, ServerResponseConverter,
                        ResponseConverter);
                ProcessResponse(deserializedResp);
                return deserializedResp;
            }
            else {
                OnCommunicationError();
                ConnectionStatus = ConnectionStatus.ServerUnavailable;
                StopHeartbeat();
                OnLogMessage($"NGIO.NET: NG Connectivity error, sending request to Newgrounds.io returned {resp?.StatusCode}",
                    null, LogSeverity.Critical);
                return null;
            }
        }

    }
}
