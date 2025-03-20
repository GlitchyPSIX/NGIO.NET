namespace NewgroundsIODotNet.Enums {
    public enum NgioErrorCode {
        Unknown = 0,
        MissingInput = 100,
        InvalidInput = 101,
        MissingParameter = 102,
        InvalidParameter = 103,
        ExpiredSession = 104,
        DuplicateSession = 105,
        MaxConnectionsExceeded = 106,
        MaxCallsExceeded = 107,
        MaxMemoryExceeded = 108,
        ExecutionTimeOut = 109,
        LoginRequired = 110,
        SessionCancelled = 111,
        InvalidAppId = 200,
        InvalidEncryption = 201,
        InvalidMedalId = 202,
        InvalidScoreboardId = 203,
        Forbidden = 403,
        ServerUnavailable = 504
    }
}