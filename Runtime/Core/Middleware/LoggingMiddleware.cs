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

            Debug.Log($"[API REQUEST] {requestParams.Verb} {requestParams.Url}");

            // Updated body logging
            if (logRequestBody)
            {
                string bodyContent = null;

                if (requestParams.BodyType == ApiBodyType.Json)
                {
                    bodyContent = requestParams.JsonBody;
                }
                else if (requestParams.BodyType == ApiBodyType.FormUrlEncoded &&
                         requestParams.FormFields != null)
                {
                    bodyContent = string.Join("&",
                        System.Linq.Enumerable.Select(
                            requestParams.FormFields,
                            kv => $"{kv.Key}={kv.Value}"));
                }

                if (!string.IsNullOrEmpty(bodyContent))
                {
                    Debug.Log($"[API REQUEST BODY] {bodyContent}");
                }
            }

            var response = await next(requestParams);

            stopwatch.Stop();

            Debug.Log($"[API RESPONSE] {response.StatusCode} ({stopwatch.ElapsedMilliseconds} ms)");

            if (logResponseBody && !string.IsNullOrEmpty(response.Body))
            {
                Debug.Log($"[API RESPONSE BODY] {response.Body}");
            }

            return response;
        }
    }
}
