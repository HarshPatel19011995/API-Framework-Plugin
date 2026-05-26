using System.Linq;
using MayaMystic.ApiFramework.Core.Config;
using MayaMystic.ApiFramework.Core.Interfaces;

namespace MayaMystic.ApiFramework.Core.Utilities
{
    public class ApiEndpointResolver :
        IApiEndpointResolver
    {
        private readonly ApiEnvironmentConfig config;

        public ApiEndpointResolver(
            ApiEnvironmentConfig config)
        {
            this.config = config;
        }

        public string GetFullUrl(string endpointKey)
        {
            var endpoint =
                config.Endpoints.FirstOrDefault(
                    e => e.Key == endpointKey);

            if (endpoint == null)
            {
                return string.Empty;
            }

            return
                $"{config.BaseUrl.TrimEnd('/')}/" +
                $"{endpoint.Endpoint.TrimStart('/')}";
        }
    }
}