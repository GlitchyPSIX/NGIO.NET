namespace NewgroundsIODotNet.Enums {
    public enum ConnectionStatus {
        /// <summary>
        /// Checking the version of the game.
        /// </summary>
        CheckingLocalVersion,
        /// <summary>
        /// Server cannot be connected to.
        /// </summary>
        ServerUnreachable,
        /// <summary>
        /// NGIO.NET has just been initialized.
        /// </summary>
        Initialized,
        /// <summary>
        /// Items have been preloaded.
        /// </summary>
        ItemsPreloaded,
        /// <summary>
        /// Game version has been checked.
        /// </summary>
        LocalVersionChecked,
        /// <summary>
        /// Login failed or was cancelled by the user.
        /// </summary>
        LoginCancelled,
        /// <summary>
        /// Login failed for an unknown reason.
        /// </summary>
        LoginFailed,
        /// <summary>
        /// Login is required to use NGIO.
        /// </summary>
        LoginRequired,
        /// <summary>
        /// Login was successful.
        /// </summary>
        LoginSuccessful,
        /// <summary>
        /// NGIO.NET is preloading medals, scoreboards, and/or save slots.
        /// </summary>
        PreloadingItems,
        /// <summary>
        /// NGIO.NET is ready.
        /// </summary>
        Ready,
        /// <summary>
        /// The server does not want to respond.
        /// </summary>
        ServerUnavailable,
        /// <summary>
        /// NGIO.NET was just brought to life, but hasn't been told to begin yet.
        /// </summary>
        Uninitialized,
        /// <summary>
        /// User's session expired or has logged out.
        /// </summary>
        UserLoggedOut
    }
}