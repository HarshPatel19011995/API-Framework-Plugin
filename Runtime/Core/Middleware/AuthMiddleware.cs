using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Interfaces;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    public class AuthMiddleware : IApiMiddleware
    {
        private readonly ITokenProvider tokenProvider;

        public AuthMiddleware(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        public async Task<ApiResponse> InvokeAsync(
            ApiRequestParams requestParams,
            MiddlewareDelegate next)
        {
            var token = tokenProvider?.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                requestParams.AuthToken = token;
            }

            return await next(requestParams);
        }
    }
}