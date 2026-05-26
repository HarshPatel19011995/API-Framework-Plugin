/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiManager.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.3.0
 * 
 *  Description :
 *  Centralized API request manager responsible for:
 *  - HTTP request execution
 *  - Middleware pipeline handling
 *  - Timeout management
 *  - Cancellation token support
 *  - Response processing
 *  - Request lifecycle management
 * 
 *  Features :
 *  - Middleware pipeline execution
 *  - Timeout handling
 *  - Linked cancellation support
 *  - Strong response wrapping
 *  - HttpClient reuse
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Middleware;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Managers
{
    /// <summary>
    /// Centralized API request manager.
    /// </summary>
    /// <remarks>
    /// Responsible for:
    /// - HTTP request execution
    /// - Middleware pipeline handling
    /// - Timeout management
    /// - Request cancellation
    /// - Response wrapping
    /// 
    /// Acts as the core networking execution layer
    /// of the MayaMystic API Framework.
    /// </remarks>
    public class ApiManager : IDisposable
    {
        #region Private Variables

        /// <summary>
        /// Shared HTTP client instance.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Middleware execution pipeline.
        /// </summary>
        private readonly ApiMiddlewarePipeline middlewarePipeline;

        /// <summary>
        /// Indicates whether manager has been disposed.
        /// </summary>
        private bool isDisposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ApiManager"/> class.
        /// </summary>
        public ApiManager()
        {
            httpClient = new HttpClient();

            middlewarePipeline =
                new ApiMiddlewarePipeline();
        }

        #endregion

        #region Middleware

        /// <summary>
        /// Registers middleware into execution pipeline.
        /// </summary>
        /// <param name="middleware">
        /// Middleware instance.
        /// </param>
        public void UseMiddleware(
            IApiMiddleware middleware)
        {
            middlewarePipeline.Use(middleware);
        }

        #endregion

        #region Request Execution

        /// <summary>
        /// Sends API request asynchronously.
        /// </summary>
        /// <param name="requestParams">
        /// Request parameter container.
        /// </param>
        /// <returns>
        /// API response result.
        /// </returns>
        public async Task<ApiResponse> SendAsync(
            ApiRequestParams requestParams)
        {
            try
            {
                MiddlewareDelegate terminalDelegate =
                    ExecuteHttpRequestAsync;

                MiddlewareDelegate pipeline =
                    middlewarePipeline.Build(
                        terminalDelegate);

                return await pipeline(requestParams);
            }
            catch (TaskCanceledException ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.RequestTimeout,
                    $"Request cancelled or timed out: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.ServiceUnavailable,
                    $"HTTP Request Failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.InternalServerError,
                    ex.Message);
            }
        }

        #endregion

        #region HTTP Execution

        /// <summary>
        /// Executes HTTP request asynchronously.
        /// </summary>
        /// <param name="requestParams">
        /// Request parameter container.
        /// </param>
        /// <returns>
        /// API response result.
        /// </returns>
        private async Task<ApiResponse> ExecuteHttpRequestAsync(
            ApiRequestParams requestParams)
        {
            try
            {
                using HttpRequestMessage request =
                    requestParams.BuildHttpRequestMessage();

                using CancellationTokenSource timeoutCts =
                    new CancellationTokenSource(
                        TimeSpan.FromSeconds(
                            requestParams.TimeoutSeconds));

                using CancellationTokenSource linkedCts =
                    CancellationTokenSource
                        .CreateLinkedTokenSource(
                            timeoutCts.Token,
                            requestParams.CancellationToken);

                using HttpResponseMessage response =
                    await httpClient.SendAsync(
                        request,
                        linkedCts.Token);

                string body =
                    await response.Content.ReadAsStringAsync();

                return new ApiResponse(
                    response.IsSuccessStatusCode,
                    (int)response.StatusCode,
                    body,
                    response.IsSuccessStatusCode
                        ? null
                        : response.ReasonPhrase);
            }
            catch (TaskCanceledException ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.RequestTimeout,
                    $"Request Timeout: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.ServiceUnavailable,
                    $"HTTP Request Failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(
                    NetworkResponseCode.InternalServerError,
                    ex.Message);
            }
        }

        #endregion

        #region Response Helpers

        /// <summary>
        /// Creates standardized error response.
        /// </summary>
        /// <param name="responseCode">
        /// Network response code.
        /// </param>
        /// <param name="message">
        /// Error message.
        /// </param>
        /// <returns>
        /// Configured error response.
        /// </returns>
        private ApiResponse CreateErrorResponse(
            NetworkResponseCode responseCode,
            string message)
        {
            return new ApiResponse(
                false,
                (int)responseCode,
                null,
                message);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                return;

            httpClient?.Dispose();

            isDisposed = true;
        }

        #endregion
    }
}