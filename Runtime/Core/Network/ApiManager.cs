/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 * 
 *  Description :
 *  Centralized async API manager using middleware pipeline.
 *  Handles request tracking and cancellation.
 **************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Middleware;

namespace MayaMystic.ApiFramework.Core.Network
{
    public class ApiManager
    {
        private static readonly HttpClient httpClient = new();
        private readonly ConcurrentDictionary<string, CancellationTokenSource> activeRequests = new();

        private readonly ApiMiddlewarePipeline pipeline = new();

        /// <summary>
        /// Register middleware into pipeline.
        /// </summary>
        public void UseMiddleware(IApiMiddleware middleware)
        {
            pipeline.Use(middleware);
        }

        public async Task<ApiResponse> SendAsync(ApiRequestParams requestParams)
        {
            string requestId = Guid.NewGuid().ToString();
            var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(requestParams.TimeoutSeconds));

            activeRequests.TryAdd(requestId, cts);

            try
            {
                // Terminal delegate (actual HTTP execution)
                MiddlewareDelegate terminal = async (req) =>
                {
                    try
                    {
                        var requestMessage = req.BuildHttpRequestMessage();
                        var response = await httpClient.SendAsync(requestMessage, cts.Token);
                        var body = await response.Content.ReadAsStringAsync();

                        return new ApiResponse(
                            response.IsSuccessStatusCode,
                            (int)response.StatusCode,
                            body,
                            response.IsSuccessStatusCode ? null : response.ReasonPhrase
                        );
                    }
                    catch (TaskCanceledException)
                    {
                        return new ApiResponse(false, 408, null, "Request Timeout");
                    }
                    catch (Exception ex)
                    {
                        return new ApiResponse(false, 500, null, ex.Message);
                    }
                };

                var pipelineDelegate = pipeline.Build(terminal);

                return await pipelineDelegate(requestParams);
            }
            finally
            {
                activeRequests.TryRemove(requestId, out _);
                cts.Dispose();
            }
        }

        public void CancelAllRequests()
        {
            foreach (var pair in activeRequests)
            {
                pair.Value.Cancel();
            }

            activeRequests.Clear();
        }
    }
}