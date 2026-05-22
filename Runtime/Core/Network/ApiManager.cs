/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiManager.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 * 
 **************************************************************************/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Middleware;

namespace MayaMystic.ApiFramework.Core.Managers
{
    public class ApiManager
    {
        // ------------------------------------------------
        // Variables
        // ------------------------------------------------

        private readonly HttpClient httpClient;

        private readonly ApiMiddlewarePipeline middlewarePipeline;

        // ------------------------------------------------
        // Constructor
        // ------------------------------------------------

        public ApiManager()
        {
            httpClient = new HttpClient();

            middlewarePipeline = new ApiMiddlewarePipeline();
        }

        // ------------------------------------------------
        // Middleware
        // ------------------------------------------------

        public void UseMiddleware(IApiMiddleware middleware)
        {
            middlewarePipeline.Use(middleware);
        }

        // ------------------------------------------------
        // Send Request
        // ------------------------------------------------

        public async Task<ApiResponse> SendAsync(
            ApiRequestParams requestParams)
        {
            try
            {
                MiddlewareDelegate terminalDelegate =
                    ExecuteHttpRequestAsync;

                var pipeline =
                    middlewarePipeline.Build(terminalDelegate);

                return await pipeline(requestParams);
            }
            catch (Exception ex)
            {
                return new ApiResponse(
                    false,
                    500,
                    null,
                    ex.Message
                );
            }
        }

        // ------------------------------------------------
        // HTTP Execution
        // ------------------------------------------------

        private async Task<ApiResponse> ExecuteHttpRequestAsync(
            ApiRequestParams requestParams)
        {
            try
            {
                using var request =
                    requestParams.BuildHttpRequestMessage();

                using var response =
                    await httpClient.SendAsync(request);

                var body =
                    await response.Content.ReadAsStringAsync();

                return new ApiResponse(
                    response.IsSuccessStatusCode,
                    (int)response.StatusCode,
                    body,
                    response.IsSuccessStatusCode
                        ? null
                        : response.ReasonPhrase
                );
            }
            catch (TaskCanceledException)
            {
                return new ApiResponse(
                    false,
                    408,
                    null,
                    "Request Timeout"
                );
            }
            catch (Exception ex)
            {
                return new ApiResponse(
                    false,
                    500,
                    null,
                    ex.Message
                );
            }
        }
    }
}