namespace NewgroundsIODotNet.Logging {
    public struct LogInfo {
        public string Message { get; set; }
        public LogSeverity Severity { get; set; }
        public object Context { get; set; }
    }
}