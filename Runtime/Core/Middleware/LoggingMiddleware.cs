using System.Threading.Tasks;
using UnityEngine;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    public class LoggingMiddleware : IApiMiddleware
    {
        private readonly bool logRequestBody;
        private readonly bool logResponseBody;

        public LoggingMiddleware(bool logRequestBody = false,
                                 bool logResponseBody = false)
        {
            this.logRequestBody = logRequestBody;
            this.logResponseBody = logResponseBody;
        }

        public async Task<ApiResponse> InvokeAsync(
            ApiRequestParams requestParams,
            MiddlewareDelegate next)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            UnityEngine.Debug.Log(
                $"[API REQUEST] {requestParams.Verb} {requestParams.Url}");

            if (logRequestBody && !string.IsNullOrEmpty(requestParams.Content))
            {
                UnityEngine.Debug.Log(
                    $"[API REQUEST BODY] {requestParams.Content}");
            }

            var response = await next(requestParams);

            stopwatch.Stop();

            UnityEngine.Debug.Log(
                $"[API RESPONSE] {response.StatusCode} ({stopwatch.ElapsedMilliseconds} ms)");

            if (logResponseBody && !string.IsNullOrEmpty(response.Body))
            {
                UnityEngine.Debug.Log(
                    $"[API RESPONSE BODY] {response.Body}");
            }

            return response;
        }
    }
}