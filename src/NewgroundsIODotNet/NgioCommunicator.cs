using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NewgroundsIODotNet.Components.Interfaces;
using NewgroundsIODotNet.Components.Requests.App;
using NewgroundsIODotNet.Components.Requests.CloudSave;
using NewgroundsIODotNet.Components.Requests.Event;
using NewgroundsIODotNet.Components.Requests.Gateway;
using NewgroundsIODotNet.Components.Requests.Loader;
using NewgroundsIODotNet.Components.Requests.Medal;
using NewgroundsIODotNet.Components.Requests.ScoreBoard;
using NewgroundsIODotNet.Components.Responses.App;
using NewgroundsIODotNet.Components.Responses.CloudSave;
using NewgroundsIODotNet.Components.Responses.Event;
using NewgroundsIODotNet.Components.Responses.Gateway;
using NewgroundsIODotNet.Components.Responses.Loader;
using NewgroundsIODotNet.Components.Responses.Medal;
using NewgroundsIODotNet.Components.Responses.ScoreBoard;
using NewgroundsIODotNet.Converters;
using NewgroundsIODotNet.DataModels;
using NewgroundsIODotNet.Enums;

namespace NewgroundsIODotNet {
    public abstract class NgioCommunicator {
        public string NewgroundsGatewayUrl { get; } = "https://www.newgrounds.io/gateway_v3.php";

        protected NgioComponentRequestConverter RequestConverter = new NgioComponentRequestConverter();
        protected NgioComponentResponseConverter ResponseConverter = new NgioComponentResponseConverter();
        protected NgioSecureRequestConverter SecureRequestConverter = new NgioSecureRequestConverter();
        protected NgioServerResponseConverter ServerResponseConverter = new NgioServerResponseConverter();

        /// <summary>
        /// The App ID to use with this Communicator.
        /// </summary>
        public string AppId { get; }
        /// <summary>
        /// Whether this Communicator was created with an Encryption key.
        /// </summary>
        public bool Secured { get; } // avoid exposing string of encryption key once created, just in case
        /// <summary>
        /// Toggles NG's debug mode
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// The current Session. <c>null</c> if the session is uninitialized.
        /// </summary>
        public Session? Session { get; protected set; }

        public ConnectionStatus ConnectionStatus { get; protected set; } = ConnectionStatus.Uninitialized;

        /// <summary>
        /// The current User. <c>null</c> if there's nobody logged in.
        /// </summary>
        public User? CurrentUser { get; protected set; }

        /// <summary>
        /// Is this game running on a legal host? Defaults to <c>true</c> unless server responds otherwise at any point.
        /// </summary>
        public bool LegalHost { get; protected set; } = true;

        /// <summary>
        /// Is this the latest version of the game? Defaults to <c>true</c> unless server responds otherwise at any point.
        /// </summary>
        public bool IsLatestVersion { get; protected set; } = true;

        /// <summary>
        /// Whether the login page is open.
        /// </summary>
        public bool LoginPageOpen { get; protected set; }

        private Dictionary<int, Medal> _preloadedMedals = new Dictionary<int, Medal>();
        private Dictionary<int, ScoreBoard> _preloadedScoreboards = new Dictionary<int, ScoreBoard>();
        private Dictionary<int, SaveSlot> _preloadedSaveSlots = new Dictionary<int, SaveSlot>();

        /// <summary>
        /// App's version defined by the constructor
        /// </summary>
        public string AppVersion { get; }

        private bool _preloadMedals;
        /// <summary>
        /// <c>true</c> if medals have been preloaded inside the Communicator. Required to unlock Medals through the friendly methods.
        /// </summary>
        public bool MedalsPreloaded { get; protected set; }

        private bool _preloadScoreboards;
        /// <summary>
        /// <c>true</c> if scoreboards have been preloaded inside the Communicator. Required to post and get Scores through the friendly methods.
        /// </summary>
        public bool ScoreboardsPreloaded { get; protected set; }

        /// <summary>
        /// <c>true</c> if save slots have been preloaded inside the Communicator. Required to load and save to Save Slots through the friendly methods.
        /// </summary>
        public bool SaveSlotsPreloaded { get; protected set; }

        public IReadOnlyDictionary<int, Medal> LoadedMedals => _preloadedMedals;
        public IReadOnlyDictionary<int, ScoreBoard> LoadedScoreboards => _preloadedScoreboards;
        public IReadOnlyDictionary<int, SaveSlot> LoadedSaveSlots => _preloadedSaveSlots;

        public bool LoginSkipped { get; protected set; }
        public bool IsReady => ConnectionStatus == ConnectionStatus.Ready;
        public bool HasUser => CurrentUser != null && Session != null;

        /// <summary>
        /// Last time a <seealso cref="GatewayPingResponse">Gateway.ping</seealso> or <seealso cref="AppCheckSessionResponse">App.checkSession</seealso> request replies successfully. 
        /// </summary>
        public DateTime LastSuccessfulPing { get; protected set; }

        /// <summary>
        /// Last time any component was executed.
        /// </summary>
        public DateTime LastExecution { get; protected set; }

        /// <summary>
        /// Last scores fetched by the Communicator through <seealso cref="GetScoreboardScores">GetScoreboardScores</seealso>.
        /// </summary>
        public (Score[], ScoreBoardPeriod)? LastScoresResult { get; protected set; }

        /// <summary>
        /// Last score posted through <seealso cref="PostScore">PostScore</seealso>.
        /// </summary>
        /// <remarks>
        /// <b>NOTE:</b> <seealso cref="User"/> in this Score will always be null; assume it's the current player.
        /// </remarks>
        public Score? LastScorePosted { get; protected set; }

        /// <summary>
        /// Last save slot saved to through <seealso cref="SetSaveSlot">SetSaveSlot</seealso>.
        /// </summary>
        public SaveSlot? LastSlotSaved { get; protected set; }

        /// <summary>
        /// Last save slot that has been fetched data using <seealso cref="GetSaveSlotData">GetSaveSlotData</seealso>.
        /// </summary>
        public SaveSlot? LastSlotLoaded { get; protected set; }

        /// <summary>
        /// Last medal unlocked through <seealso cref="UnlockMedal">UnlockMedal</seealso>.
        /// </summary>
        public Medal? LastUnlockedMedal { get; protected set; }

        /// <summary>
        /// Fires when a user write operation is performed (medal, save slot, scoreboard) with the friendly methods and was not successful
        /// </summary>
        public event EventHandler<INgioComponentResponse> UserWriteFailure;

        /// <summary>
        /// Fires when a medal that wasn't previously unlocked is successfully unlocked through the friendly method.
        /// </summary>
        public event EventHandler<Medal> MedalUnlocked;

        /// <summary>
        /// Fires when a response is received, prior to the Communicator knowing they came through.
        /// </summary>
        public event EventHandler<NgioServerResponse> ResponseReceived;

        /// <summary>
        /// Fires when the HTTP client of the Communicator fails to connect to NG.
        /// </summary>
        public event EventHandler CommunicationError;

        /// <summary>
        /// Fires the moment that NG does not report an HTTP 200 when making a request.
        /// </summary>
        public event EventHandler ServerUnavailable;

        /// <summary>
        /// Fires when the response has an error.
        /// </summary>
        public event EventHandler<NgioServerResponse> ResponseError;

        /// <summary>
        /// Fires when any components inside a response have an error.
        /// </summary>
        public event EventHandler<INgioComponentResponse[]> ComponentResponseError;

        /// <summary>
        /// Fires the moment the communicator signals Ready.
        /// </summary>
        public event EventHandler Ready;

        protected string _sessionId;
        protected string _host;
        protected bool _logViewOnInit;
        protected bool _heartbeatPending;


        /// <summary>
        /// Creates a new Communicator instance.
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
        protected NgioCommunicator(string appId,
            string encryptionKey,
            string appVersion = "0.0.0",
            bool debugMode = false,
            bool preloadMedals = false,
            bool preloadScores = false,
            bool logViewOnInit = true,
            string sessionId = null,
            string host = "localhost") {

            AppId = appId;
            DebugMode = debugMode;
            AppVersion = appVersion;
            _preloadMedals = preloadMedals;
            _preloadScoreboards = preloadScores;

            if (encryptionKey != null) {
                SecureRequestConverter.EncryptionKey = Convert.FromBase64String(encryptionKey);
                Secured = true;
            }
            _sessionId = sessionId;
            _host = host;
            _logViewOnInit = logViewOnInit;
        }

        /// <summary>
        /// Initializes the Communicator.
        /// </summary>
        public void Initialize() {
            // sends prechecks
            List<INgioComponentRequest> precheckRequests = new List<INgioComponentRequest>
                { new AppGetCurrentVersionRequest(AppVersion), new AppGetHostLicenseRequest(_host) };

            // if logview on init is true, also add the log request
            if (_logViewOnInit) precheckRequests.Add(new AppLogViewRequest(_host));

            ConnectionStatus = ConnectionStatus.Initialized;
            SendRequest(precheckRequests.ToArray(), ReceiveVersionCheck); // send em → ReceiveVersionCheck + set LegalHost & IsLatestVersion
        }

        /// <summary>
        /// Gets the info of a preloaded medal.
        /// </summary>
        /// <param name="id">Medal ID</param>
        /// <returns>The found medal.</returns>
        /// <exception cref="ApplicationException">Thrown when there's no user or medals have not been preloaded.</exception>
        /// <exception cref="ArgumentException">Thrown when medals haven't been preloaded.</exception>
        public Medal GetMedal(int id) {
            if (!HasUser) throw new ApplicationException("NGIO.NET: Medals disabled: no user.");
            if (!MedalsPreloaded) throw new ApplicationException("NGIO.NET: Medals have not been preloaded.");
            bool medalExists = LoadedMedals.TryGetValue(id, out Medal foundMedal);
            if (!medalExists) throw new ArgumentException($"NGIO.NET: Medal #{id} not found.");
            return foundMedal;
        }

        /// <summary>
        /// Unlocks a Medal.
        /// </summary>
        /// <param name="medal">The Medal to unlock.</param>
        /// <param name="responseCallback">Callback to execute when the medal is unlocked</param>
        /// <exception cref="Exception">Thrown when the Medal could not be unlocked.</exception>
        public void UnlockMedal(Medal medal, Action<Medal> responseCallback = null) {
            if (medal.Unlocked.HasValue && (bool)medal.Unlocked) return; // no use in re-unlocking an unlocked medal
            if (!HasUser) throw new ApplicationException("NGIO.NET: Medals disabled: no user.");
            SendRequest(new MedalUnlockRequest(medal.Id), (serverResponse) => {
                MedalUnlockResponse medalResp = serverResponse.GetComponentResult<MedalUnlockResponse>();
                if (!medalResp.Success) {
                    UserWriteFailure?.Invoke(this, medalResp);
                    throw new Exception($"NGIO.NET: Wasn't able to unlock medal {medal.Name}");
                }

                LastUnlockedMedal = medalResp.Medal;
                MedalUnlocked?.Invoke(this, medalResp.Medal);
                responseCallback?.Invoke(medalResp.Medal); // Medal already gets re-cached by ProcessRequest
            });
        }

        /// <summary>
        /// Gets the info of a Cloud Save slot.
        /// </summary>
        /// <param name="id">ID of the Cloud Save slot</param>
        /// <returns>The info for the Save Slot</returns>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SaveSlot GetSaveSlot(int id) {
            if (!HasUser) throw new ApplicationException("NGIO.NET: Save Slots disabled: no user.");
            if (!SaveSlotsPreloaded) throw new ApplicationException("NGIO.NET: Save Slots have not been preloaded.");
            bool saveExists = LoadedSaveSlots.TryGetValue(id, out SaveSlot foundSlot);
            if (!saveExists) throw new ArgumentException($"NGIO.NET: Save slot #{id} not found.");
            return foundSlot;
        }

        /// <summary>
        /// Sets data in a Save Slot.
        /// </summary>
        /// <param name="slot">Save Slot to save to.</param>
        /// <param name="data">Data to save in the Save Slot</param>
        /// <param name="responseCallback">Callback to execute when data is received</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void SetSaveSlot(SaveSlot slot, string data, Action<SaveSlot?> responseCallback = null) {
            if (!HasUser) throw new ApplicationException("NGIO.NET: Save Slots disabled: no user.");
            SendRequest(new CloudSaveSetDataRequest(data, slot.Id), (response) => {
                CloudSaveSlotResponse resp = response.GetComponentResult<CloudSaveSlotResponse>();
                if (!resp.Success) {
                    responseCallback?.Invoke(null);
                    UserWriteFailure?.Invoke(this, resp);
                    throw new ArgumentException($"NGIO.NET: Saving to slot #{slot.Id} failed: {resp.Error?.Message}");
                }

                LastSlotSaved = resp.Slot;
                responseCallback?.Invoke(resp.Slot);
            });
        }

        /// <summary>
        /// Gets the data from the Save Slot.
        /// </summary>
        /// <remarks>All Cloud Save data is stored as strings.</remarks>
        /// <param name="slot">The Save slot to get data from.</param>
        /// <param name="responseCallback">Callback to execute when the data arrives.</param>
        public abstract void GetSaveSlotData(SaveSlot slot, Action<string> responseCallback = null);

        /// <summary>
        /// Loads and opens a Loader URL.
        /// </summary>
        /// <param name="type">The kind of URL to get from Newgrounds</param>
        /// <param name="logStat">Whether to log this load in the stats.</param>
        public void LoadUrl(StandardLoaderType type, bool logStat) {
            SendRequest(new LoaderUrlRequest(type, _host, logStat), (response) => {
                LoaderUrlResponse loaderResponse = response.GetComponentResult<LoaderUrlResponse>();
                if (loaderResponse.Success) OpenUrl(loaderResponse.Url);
            });
        }

        /// <summary>
        /// Loads and opens a Loader Referral URL.
        /// </summary>
        /// <param name="referral">The ID of the referral from your API Tools dashboard.</param>
        /// <param name="logStat">Whether to log this load in the stats.</param>
        /// <exception cref="ArgumentException">Thrown if the server returns unsuccessful.</exception>
        public void LoadReferral(string referral, bool logStat) {
            SendRequest(new LoaderReferralUrlRequest(_host, referral, logStat), (response) => {
                LoaderUrlResponse loaderResponse = response.GetComponentResult<LoaderUrlResponse>();
                if (loaderResponse.Success)
                    OpenUrl(loaderResponse.Url);
                else {
                    throw new ArgumentException($"NGIO.NET: Loading referral {referral} failed.");
                }
            });
        }

        /// <summary>
        /// Logs an event with an ID as specified in your Referrals and Events page. <see href="https://www.newgrounds.io/help/components/#event-logevent">Newgrounds.IO Documentation</see>
        /// </summary>
        /// <param name="eventId">ID of the event to log.</param>
        /// <param name="responseCallback">Callback to execute when data is received</param>
        /// <exception cref="Exception">Thrown when the operation returns unsuccessful.</exception>
        public void LogEvent(string eventId, Action<EventLogEventResponse> responseCallback) {
            SendRequest(new EventLogEventRequest(_host, eventId), (response) => {
                EventLogEventResponse evtResponse = response.GetComponentResult<EventLogEventResponse>();
                if (!evtResponse.Success) {
                    throw new Exception(
                        $"NGIO.NET: Logging event with ID {eventId} failed: {evtResponse.Error?.Message}");
                }

                responseCallback?.Invoke(evtResponse);
            });
        }

        /// <summary>
        /// Gets the date and time from the Newgrounds.IO server.
        /// </summary>
        /// <param name="responseCallback">Action to execute when data is received</param>
        /// <exception cref="Exception">You know, this shouldn't happen unless the server is dying, or you're offline.</exception>
        public void GetDateTime(Action<DateTime> responseCallback) {
            SendRequest(new GatewayGetDateTimeRequest(), (response) => {
                GatewayGetDateTimeResponse dtResponse = response.GetComponentResult<GatewayGetDateTimeResponse>();
                if (!dtResponse.Success) {
                    throw new Exception(
                        $"NGIO.NET: SOMEHOW getting the date-time from the server failed: {dtResponse.Error?.Message}");
                }

                responseCallback?.Invoke(dtResponse.Date);
            });
        }

        /// <summary>
        /// Gets the info of a Scoreboard.
        /// </summary>
        /// <param name="id">ID of the Scoreboard</param>
        /// <returns>The info for the Scoreboard</returns>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public ScoreBoard GetScoreboard(int id) {
            if (!ScoreboardsPreloaded) throw new ApplicationException("NGIO.NET: Scoreboards have not been preloaded.");
            bool sbExists = LoadedScoreboards.TryGetValue(id, out ScoreBoard foundScoreboard);
            if (!sbExists) throw new ArgumentException($"NGIO.NET: Scoreboard {id} not found.");
            return foundScoreboard;
        }

        /// <summary>
        /// Gets the scores of a Scoreboard
        /// </summary>
        /// <param name="scoreBoard">Scoreboard to get scores from</param>
        /// <param name="limit">Limit number of scores returned</param>
        /// <param name="skip">Skip number of scores, used for pagination</param>
        /// <param name="social">Whether to only include the logged in user's friends</param>
        /// <param name="filterTag">Tag to filter scores with</param>
        /// <param name="responseCallback" >Callback to execute when data is received</param>
        /// <returns>The info for the Scoreboard</returns>
        /// <exception cref="Exception">Thrown when there's an issue getting the scoreboards.</exception>
        public void GetScoreboardScores(ScoreBoard scoreBoard, ScoreBoardPeriod period = ScoreBoardPeriod.Day, int limit = 10, int skip = 0, bool social = false,
            string filterTag = null, Action<ScoreBoardGetScoresResponse> responseCallback = null) {
            if (!HasUser) throw new ApplicationException("NGIO.NET: Scoreboards disabled: no user.");
            SendRequest(new ScoreBoardGetScoresRequest(scoreBoard.Id, null, limit, period, skip, social, filterTag),
                response => {
                    ScoreBoardGetScoresResponse scoresResponse =
                        response.GetComponentResult<ScoreBoardGetScoresResponse>();
                    if (!scoresResponse.Success)
                        throw new Exception($"NGIO.NET: Failure getting scores: {scoresResponse.Error?.Message}");

                    LastScoresResult = (scoresResponse.Scores, scoresResponse.Period);
                    responseCallback?.Invoke(scoresResponse);
                });
        }

        public void PostScore(ScoreBoard scoreBoard, int amount, string tag = null, Action<Score?> responseCallback = null) {
            if (!HasUser) throw new ApplicationException("NGIO.NET: Scoreboard posting disabled: no user.");
            SendRequest(new ScoreBoardPostScoreRequest(scoreBoard.Id, amount, tag), (response) => {
                ScoreBoardPostScoreResponse resp = response.GetComponentResult<ScoreBoardPostScoreResponse>();
                if (!resp.Success) {
                    responseCallback?.Invoke(null);
                    UserWriteFailure?.Invoke(this, resp);
                    throw new ArgumentException($"NGIO.NET: Posting score {amount} to Scoreboard #{scoreBoard.Id} failed: {resp.Error?.Message}");
                }

                LastScorePosted = resp.Score;
                responseCallback?.Invoke(resp.Score);
            });
        }

        /// <summary>
        /// Resets login, session, caches for saves/medals/scores and login-skip state.
        /// </summary>
        public void ResetConnectionState() {
            LoginSkipped = false;
            Session = null;
            CurrentUser = null;
            ConnectionStatus = ConnectionStatus.Initialized;

            LastScoresResult = null;
            LastUnlockedMedal = null;
            LastScorePosted = null;
            LastSlotLoaded = null;
            LastSlotSaved = null;
        }

        /// <summary>
        /// Opens the Newgrounds Passport URL supplied by a given session, or starts a new session and <i>then</i> opens the page.
        /// </summary>
        public void LogIn() {
            if (Session == null ||
                ConnectionStatus == ConnectionStatus.LoginCancelled ||
                ConnectionStatus == ConnectionStatus.UserLoggedOut ||
                ConnectionStatus == ConnectionStatus.LoginFailed) {
                SendRequest(new AppStartSessionRequest(false), response => {
                    LoginPageOpen = true;
                    OpenLoginPage();
                });
            }
            else {
                LoginPageOpen = true;
                OpenLoginPage();
            }
        }

        /// <summary>
        /// Cancels login if in progress and terminates the user session.
        /// </summary>
        public void LogOut() {
            if (LoginPageOpen) CancelLogin();
            if (CurrentUser != null || Session != null) SendRequest(new AppEndSessionRequest(), null);
        }

        /// <summary>
        /// Cancels login if the login window was opened.
        /// </summary>
        public void CancelLogin() {
            if (ConnectionStatus == ConnectionStatus.Ready) return;
            if (!LoginPageOpen) return;
            LoginPageOpen = false;
            // Cancel login fully and go back to login required
            if (ConnectionStatus == ConnectionStatus.LoginCancelled) {
                Session = null;
                ConnectionStatus = ConnectionStatus.LoginRequired;
            }
        }

        /// <summary>
        /// Skips login, sets NGIO.NET to be ready with no session. Scoreboards and medals disabled.
        /// </summary>
        /// <exception cref="Exception">Thrown if skipping is attempted while login is required.</exception>
        public void SkipLogin() {
            if (ConnectionStatus == ConnectionStatus.Ready) return;
            if (ConnectionStatus != ConnectionStatus.LoginRequired
                || ConnectionStatus != ConnectionStatus.LoginCancelled)
                throw new Exception("NGIO.NET: attempted to skip login without login required");

            ConnectionStatus = ConnectionStatus.Ready;
            Ready?.Invoke(this, EventArgs.Empty);
            LoginPageOpen = false;
            LoginSkipped = true;
        }

        /// <summary>
        /// Opens the login page from an existing
        /// </summary>
        public void OpenLoginPage() {
            if (Session?.PassportUrl == null) return;
            OpenUrl(Session?.PassportUrl);
        }

        /// <summary>
        /// Opens an URL. Implementation is platform dependent.
        /// </summary>
        /// <remarks>This is intended to be used with any methods that return a URL, such as the NG Passport URL or referrals.</remarks>
        /// <param name="url">The URL to open</param>
        public abstract void OpenUrl(string url);

        #region Low level methods (direct communication with gateway and processing)
        /// <summary>
        /// Gives a Session ID to the Communicator and then talks to NG to populate it.
        /// </summary>
        /// <param name="sessionId">The Session ID to use</param>
        public virtual void PopulateSessionFromId(string sessionId) {
            Session forcedSession = new Session() {
                Id = sessionId
            };
            // do not save this temp session to Communicator, use only to force
            SendRequest(new INgioComponentRequest[] { new AppCheckSessionRequest() }, null, forcedSession); // → ProcessRequest (INgioSessionResponse) → HandleSessionResponse
            // the above assumes that the concrete implementation's SendRequest also calls ProcessRequest
            // it SHOULD
        }

        public void SetStartupSessionId(string sessionId) {
            if (ConnectionStatus != ConnectionStatus.Uninitialized) return; // only has effect before Initialize()
            _sessionId = sessionId;
        }

        public void HeartBeat() {
            if (ConnectionStatus == ConnectionStatus.ServerUnavailable ||
                ConnectionStatus == ConnectionStatus.ServerUnreachable) return;
            if (_heartbeatPending) return; // avoid crumpling requests together just in case

            _heartbeatPending = true;
            if (LoginPageOpen) {
                SendRequest(new AppCheckSessionRequest(), response => _heartbeatPending = false);
            }
            else {
                SendRequest(new GatewayPingRequest(), response => _heartbeatPending = false);
            }
        }

        /// <summary>
        /// Starts the session heartbeat and keeps it running until stopped or server stops responding.
        /// </summary>
        /// <remarks>Turning this on will keep the session alive by sending Gateway pings every <c>seconds</c>. Normally, a Communicator implementation would send session checks instead if the login page is opened to catch the user info. <see href="https://www.newgrounds.io/help/objects/#Session">Newgrounds.IO Documentation</see></remarks>
        public abstract void StartHeartbeat(float seconds = 10);

        /// <summary>
        /// Stops the session heartbeat.
        /// </summary>
        public abstract void StopHeartbeat();

        /// <summary>
        /// Uses the appropriate HTTP Client to send the components over the wire.
        /// </summary>
        /// <param name="components">Components to send.</param>
        /// <param name="callback">Callback to execute.</param>
        /// <param name="forcedSession">Whether to use this provided Session instead of the Communicator's</param>
        public abstract void SendRequest(INgioComponentRequest[] components, Action<NgioServerResponse> callback, Session? forcedSession = null);

        /// <summary>
        /// Uses the appropriate HTTP Client to send a single component over the wire.
        /// </summary>
        /// <param name="component">Component to send.</param>
        /// <param name="callback">Callback to execute.</param>
        /// <param name="forcedSession">Whether to use this provided Session instead of the Communicator's</param>
        public void SendRequest(INgioComponentRequest component, Action<NgioServerResponse> callback, Session? forcedSession = null) {
            SendRequest(new[] { component }, callback);
        }

        /// <summary>
        /// Uses the appropriate HTTP Client to send a single secure component over the wire.
        /// </summary>
        /// <param name="component">Component to send.</param>
        /// <param name="callback">Callback to execute</param>
        public abstract void SendSecureRequest(INgioComponentRequest component, Action<NgioServerResponse> callback);

        /// <summary>
        /// Calls appropriate events and methods for a received response. Should only be overriden if a special implementation requires it.
        /// </summary>
        /// <param name="response">The received response.</param>
        protected virtual void ProcessResponse(NgioServerResponse response) {
            // Do things here such as keeping the session id when startSession returns true and so-so-on
            ResponseReceived?.Invoke(this, response);
            CheckForErrors(response);

            foreach (INgioComponentResponse compResponse in response.Results) {
                switch (compResponse) {
                    case INgioSessionResponse sessResp:
                        LastSuccessfulPing = DateTime.Now;
                        if (LoginSkipped) return;
                        HandleSessionResponse(sessResp);
                        break;
                    case GatewayPingResponse pingResp:
                        LastSuccessfulPing = DateTime.Now;
                        break;
                    case AppEndSessionResponse _: {
                            Session = null;
                            CurrentUser = null;
                            ConnectionStatus = ConnectionStatus.UserLoggedOut;
                            break;
                        }
                    case AppGetCurrentVersionResponse verResp:
                        HandleVersionCheckResponse(verResp);
                        break;
                    case AppGetHostLicenseResponse hostResp:
                        HandleHostCheckResponse(hostResp);
                        break;
                    case CloudSaveSlotResponse singleSaveResponse:
                        SaveSlot save = singleSaveResponse.Slot;
                        _preloadedSaveSlots[save.Id] = save;
                        break;
                    case CloudSaveLoadSlotsResponse saveSlotResp:
                        HandleSaveSlotsResponse(saveSlotResp);
                        break;
                    case MedalListResponse medalResp:
                        HandleMedalListResponse(medalResp);
                        break;
                    case MedalUnlockResponse medalUnlockResp: {
                            if (!medalUnlockResp.Success) break;
                            _preloadedMedals[medalUnlockResp.Medal.Id] = medalUnlockResp.Medal;
                            break;
                        }
                    case ScoreBoardGetBoardsResponse scoreboardsResp:
                        HandleScoreboardListResponse(scoreboardsResp);
                        break;
                }
            }
        }

        protected void CheckForErrors(NgioServerResponse response) {
            if (!response.Success) {
                ResponseError?.Invoke(this, response);
                switch (response.Error?.ErrorCode) {
                    case NgioErrorCode.Unknown:
                        throw new WarningException(
                            $"NGIO.NET: Error code {response.Error?.Code} is not known by NGIO.NET. Message is: {response.Error?.Message}");
                    case NgioErrorCode.ServerUnavailable: // stop everything
                        ConnectionStatus = ConnectionStatus.ServerUnavailable;
                        StopHeartbeat();
                        Session = null;
                        return;
                }
            }

            INgioComponentResponse[] erroredComponents = response.Results.Where(component => !component.Success).ToArray();

            if (erroredComponents.Length > 0) {
                ComponentResponseError?.Invoke(this, erroredComponents);

                if (erroredComponents.Any(errComp => errComp.Error?.ErrorCode == NgioErrorCode.Unknown)) {
                    throw new WarningException(
                        $"NGIO.NET: One or more components returned errors unknown to NGIO.NET. Check the InnerException property to see which.",
                        new AggregateException(erroredComponents.Select(comp =>
                            new WarningException($"{comp.Component} - ({comp.Error?.Code}) {comp.Error?.Message}"))));
                }

                if (erroredComponents.Any(errComp => errComp.Error?.ErrorCode == NgioErrorCode.ServerUnavailable)) {
                    ConnectionStatus = ConnectionStatus.ServerUnavailable; // if any of the components says that the server is gone, believe it
                    StopHeartbeat();
                    Session = null;
                }
            }
        }

        protected void HandleMedalListResponse(MedalListResponse response) {
            if (!response.Success) return; // Om, what happened here
            _preloadedMedals.Clear();
            foreach (Medal responseMedal in response.Medals) {
                _preloadedMedals.Add(responseMedal.Id, responseMedal);
            }

            MedalsPreloaded = true;
        }



        protected void HandleSaveSlotsResponse(CloudSaveLoadSlotsResponse response) {
            if (!response.Success) return;
            _preloadedSaveSlots.Clear();
            foreach (SaveSlot slot in response.Slots) {
                _preloadedSaveSlots.Add(slot.Id, slot);
            }

            SaveSlotsPreloaded = true;
        }

        protected void HandleScoreboardListResponse(ScoreBoardGetBoardsResponse response) {
            if (!response.Success) return;
            _preloadedScoreboards.Clear();
            foreach (ScoreBoard responseBoard in response.ScoreBoards) {
                _preloadedScoreboards.Add(responseBoard.Id, responseBoard);
            }

            ScoreboardsPreloaded = true;
        }

        protected void HandleVersionCheckResponse(AppGetCurrentVersionResponse response) {
            if (response.ClientDeprecated) IsLatestVersion = false;
        }

        protected void HandleHostCheckResponse(AppGetHostLicenseResponse response) {
            if (!response.HostApproved) LegalHost = false;
        }

        protected void ReceiveVersionCheck(NgioServerResponse resp) {

            if (!(IsLatestVersion && LegalHost)) {
                ConnectionStatus = ConnectionStatus.Ready; // after server response, host is not legal, game is ready but latestver/legalhost should take prec
                Ready?.Invoke(this, EventArgs.Empty);
                return; // no session will be given to game if host is not legal or version is outdated, might do something bad
            }

            if (_sessionId != null) { // version checked, all good, send NGIO a checksession to get the status of this session id
                ConnectionStatus = ConnectionStatus.LocalVersionChecked;
                PopulateSessionFromId(_sessionId); // → PopulateFromSessionId 
            }
            else { // require login
                ConnectionStatus = ConnectionStatus.LoginRequired;
            }
        }

        protected void PreloadData() {
            if (ConnectionStatus != ConnectionStatus.LoginSuccessful || CurrentUser == null) return; // No user, not in setup stage, no preload

            if (LoginPageOpen) LoginPageOpen = false;

            ConnectionStatus = ConnectionStatus.PreloadingItems;

            List<INgioComponentRequest> componentQueue = new List<INgioComponentRequest>();

            if (_preloadMedals) componentQueue.Add(new MedalListRequest());
            if (_preloadScoreboards) componentQueue.Add(new ScoreBoardGetBoardsRequest());
            componentQueue.Add(new CloudSaveLoadManySlotsRequest());

            SendRequest(componentQueue.ToArray(), response => {
                if (response.Success) {
                    ConnectionStatus = ConnectionStatus.Ready; // END
                    Ready?.Invoke(this, EventArgs.Empty);
                }
                else {
                    ConnectionStatus = ConnectionStatus.LoginFailed;
                    throw new Exception("NGIO.NET: Preloading failed when trying to get medals, scoreboards and saves. (server said no success)");
                }

            });
        }

        protected void HandleSessionResponse(INgioSessionResponse sessionResponse) {
            if (sessionResponse.Session == null) { // Zero session at all, should start again
                Session = null;
                if (sessionResponse is AppCheckSessionResponse checkSess) {
                    if (ConnectionStatus == ConnectionStatus.LoginRequired && LoginPageOpen && !checkSess.Success) {
                        ConnectionStatus = ConnectionStatus.LoginCancelled;
                        LoginPageOpen = false;
                    }
                }
                return;
            }

            if (Session != null) { // Existing session 
                Session receivedSession = sessionResponse.Session.Value;

                if (receivedSession.Expired) {
                    Session = null;
                    CurrentUser = null;
                    ConnectionStatus = ConnectionStatus.UserLoggedOut;
                    return;
                }

                if (CurrentUser == null && receivedSession.User != null) { // we had a session, but we're overwriting it
                    ConnectionStatus = ConnectionStatus.LoginSuccessful;
                    Session = receivedSession;
                    CurrentUser = sessionResponse.Session?.User;
                    PreloadData(); // → PreloadData
                }
            }
            else { // 100% Non-existing session (Init setup)
                if (ConnectionStatus == ConnectionStatus.Ready) return; // State machine should not reach here if we're fully ready

                Session receivedSession = sessionResponse.Session.Value;

                if (ConnectionStatus != ConnectionStatus.LocalVersionChecked &&
                    ConnectionStatus != ConnectionStatus.LoginRequired) return;

                Session = sessionResponse.Session;

                if (receivedSession.Expired) {
                    Session = null;
                    ConnectionStatus = ConnectionStatus.UserLoggedOut;
                    return;
                }

                if (receivedSession.User == null) { // no user received right after checking session, must have passport URL alone
                    ConnectionStatus = ConnectionStatus.LoginRequired;
                }
                else { // user received right after checking session, at this stage we preload medals, scoreboards and save slots
                    ConnectionStatus = ConnectionStatus.LoginSuccessful;
                    CurrentUser = sessionResponse.Session?.User;
                    Session = receivedSession;
                    PreloadData(); // → PreloadData
                }
            }
        }
        #endregion

        #region Events usable by implementations
        /// <summary>
        /// Under any circumstance NG returns a non HTTP 200 response
        /// </summary>
        protected void OnCommunicationError() {
            CommunicationError?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Under any circumstance the NG gateway is unreachable (offline, etc)
        /// </summary>
        protected void OnServerUnavailable() {
            ServerUnavailable?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}