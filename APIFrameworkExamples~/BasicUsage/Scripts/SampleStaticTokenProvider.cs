using MayaMystic.ApiFramework.Core.Interfaces;

namespace MayaMystic.ApiFramework.Samples
{
    /// <summary>
    /// Example static token provider.
    /// Use this when API uses fixed API key.
    /// </summary>
    public class SampleStaticTokenProvider : ITokenProvider
    {
        private string token;

        public SampleStaticTokenProvider(string token)
        {
            this.token = token;
        }

        public string GetToken()
        {
            return token;
        }

        public void SetToken(string newToken)
        {
            token = newToken;
        }

        public void ClearToken()
        {
            token = string.Empty;
        }
    }
}