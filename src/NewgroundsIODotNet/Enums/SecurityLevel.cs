namespace NewgroundsIODotNet.Enums {
    public enum SecurityLevel {
        /// <summary>
        /// Encrypts no <seealso cref="NewgroundsIODotNet.Components.Interfaces.INgioComponentRequest">Component</seealso>.
        /// </summary>
        None,
        /// <summary>
        /// Only encrypts <seealso cref="NewgroundsIODotNet.Components.Interfaces.INgioComponentRequest">Components</seealso> that have
        /// <seealso cref="NewgroundsIODotNet.Components.Interfaces.INgioComponentRequest.RequiresSecureCall">RequiresSecureCall</seealso> set to <c>true</c>.
        /// </summary>
        OnlyRequired,
        /// <summary>
        /// Encrypts every <seealso cref="NewgroundsIODotNet.Components.Interfaces.INgioComponentRequest">Component</seealso> in the request.
        /// </summary>
        ForceAll
    }
}