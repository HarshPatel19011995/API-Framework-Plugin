using MayaMystic.ApiFramework.Core.Interfaces;

namespace MayaMystic.ApiFramework.Samples
{
    /// <summary>
    /// Example dynamic token provider.
    /// Use this when token is received after login.
    /// </summary>
    public class SampleRuntimeTokenProvider : ITokenProvider
    {
        private string token;

        public void SetToken(string newToken)
        {
            token = newToken;
        }

        public string GetToken() => token;
    }
}