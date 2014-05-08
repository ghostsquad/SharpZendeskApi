namespace SharpZendeskApi
{
    /// <summary>
    /// The zendesk authentication method.
    /// </summary>
    public enum ZendeskAuthenticationMethod
    {
        /// <summary>
        /// The basic authentication method.
        /// Requires a username (email address) and password.
        /// </summary>      
        Basic, 

        /// <summary>
        /// The token authentication method.
        /// Requires a username (email address) and the API token.
        /// </summary>
        Token
    }
}