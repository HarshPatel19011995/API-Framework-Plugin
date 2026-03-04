using System.Net.Http;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    public interface IApiMiddleware
    {
        Task<ApiResponse> InvokeAsync(
            ApiRequestParams requestParams,
            MiddlewareDelegate next);
    }

    public delegate Task<ApiResponse> MiddlewareDelegate(
        ApiRequestParams requestParams);
}