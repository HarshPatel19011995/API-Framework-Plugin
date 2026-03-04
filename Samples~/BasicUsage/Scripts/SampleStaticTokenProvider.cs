using MayaMystic.ApiFramework.Core.Interfaces;

namespace MayaMystic.ApiFramework.Samples
{
    /// <summary>
    /// Example static token provider.
    /// Use this when API uses fixed API key.
    /// </summary>
    public class SampleStaticTokenProvider : ITokenProvider
    {
        private readonly string token;

        public SampleStaticTokenProvider(string token)
        {
            this.token = token;
        }

        public string GetToken() => token;
    }
}