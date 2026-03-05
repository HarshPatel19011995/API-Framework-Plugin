/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *
 *  Description :
 *  Abstraction for providing authentication tokens.
 *  Allows frameworks or applications to set/update tokens dynamically.
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Interfaces
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Returns the current authentication token.
        /// </summary>
        string GetToken();

        /// <summary>
        /// Updates the authentication token.
        /// Typically called after login or token refresh.
        /// </summary>
        void SetToken(string token);

        /// <summary>
        /// Clears the stored token (logout scenario).
        /// </summary>
        void ClearToken();
    }
}
