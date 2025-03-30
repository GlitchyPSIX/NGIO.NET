namespace NewgroundsIODotNet.Logging {
    /// <summary>
    /// Generic logging information to be handled by platform-specific logging.
    /// </summary>
    public struct LogInfo {
        /// <summary>
        /// Message to log
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Severity of the log.
        /// </summary>
        public LogSeverity Severity { get; set; }
        /// <summary>
        /// Any object that can serve as context for the log.
        /// </summary>
        public object Context { get; set; }
    }
}