/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequest.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.3.0
 * 
 *  Description :
 *  Fluent request builder for creating API requests easily.
 *  Provides a developer-friendly wrapper around ApiRequestParams.
 * 
 *  Features :
 *  - Fluent API request builder
 *  - Generic typed response support
 *  - Middleware pipeline support
 *  - Retry and timeout configuration
 *  - Header and authentication handling
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Managers;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Utilities;

namespace MayaMystic.ApiFramework
{
    /// <summary>
    /// Provides fluent API request creation and execution.
    /// </summary>
    /// <remarks>
    /// Acts as the primary request builder for the framework.
    /// 
    /// Supports:
    /// - HTTP method configuration
    /// - Authentication
    /// - Headers
    /// - Retry settings
    /// - Timeout configuration
    /// - Typed response parsing
    /// </remarks>
    public class ApiRequest
    {
        #region Private Variables

        /// <summary>
        /// Internal request parameter container.
        /// </summary>
        private readonly ApiRequestParams requestParams;

        /// <summary>
        /// API manager responsible for request execution.
        /// </summary>
        private readonly ApiManager apiManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new API request builder instance.
        /// </summary>
        /// <param name="url">
        /// Target request URL.
        /// </param>
        /// <param name="apiManager">
        /// Framework API manager instance.
        /// </param>
        private ApiRequest(
            string url,
            ApiManager apiManager)
        {
            requestParams = new ApiRequestParams(url);
            this.apiManager = apiManager;
        }

        #endregion

        #region Factory

        /// <summary>
        /// Creates a new API request builder.
        /// </summary>
        /// <param name="url">
        /// Target request URL.
        /// </param>
        /// <param name="apiManager">
        /// Framework API manager instance.
        /// </param>
        /// <returns>
        /// Configured <see cref="ApiRequest"/> instance.
        /// </returns>
        public static ApiRequest Create(
            string url,
            ApiManager apiManager)
        {
            return new ApiRequest(url, apiManager);
        }

        #endregion

        #region HTTP Methods

        /// <summary>
        /// Configures request as HTTP GET.
        /// </summary>
        public ApiRequest Get()
        {
            requestParams.Verb = HttpVerb.GET;
            return this;
        }

        /// <summary>
        /// Configures request as HTTP POST.
        /// </summary>
        public ApiRequest Post()
        {
            requestParams.Verb = HttpVerb.POST;
            return this;
        }

        /// <summary>
        /// Configures request as HTTP PUT.
        /// </summary>
        public ApiRequest Put()
        {
            requestParams.Verb = HttpVerb.PUT;
            return this;
        }

        /// <summary>
        /// Configures request as HTTP DELETE.
        /// </summary>
        public ApiRequest Delete()
        {
            requestParams.Verb = HttpVerb.DELETE;
            return this;
        }

        #endregion

        #region Request Configuration

        /// <summary>
        /// Adds bearer authentication token.
        /// </summary>
        /// <param name="token">
        /// Authentication token.
        /// </param>
        public ApiRequest WithAuth(string token)
        {
            requestParams.AuthToken = token;
            return this;
        }

        /// <summary>
        /// Adds custom request header.
        /// </summary>
        /// <param name="key">
        /// Header key.
        /// </param>
        /// <param name="value">
        /// Header value.
        /// </param>
        public ApiRequest WithHeader(
            string key,
            string value)
        {
            requestParams.AdditionalHeaders[key] = value;
            return this;
        }

        /// <summary>
        /// Adds JSON request body.
        /// </summary>
        /// <param name="json">
        /// Serialized JSON string.
        /// </param>
        public ApiRequest WithJson(string json)
        {
            requestParams.BodyType = ApiBodyType.Json;
            requestParams.JsonContent = json;
            return this;
        }

        /// <summary>
        /// Adds form field content.
        /// </summary>
        public ApiRequest WithFormField(
            string key,
            string value)
        {
            requestParams.BodyType =
                ApiBodyType.FormUrlEncoded;

            requestParams.AddFormField(key, value);

            return this;
        }

        /// <summary>
        /// Configures request timeout duration.
        /// </summary>
        /// <param name="seconds">
        /// Timeout duration in seconds.
        /// </param>
        public ApiRequest WithTimeout(int seconds)
        {
            requestParams.TimeoutSeconds = seconds;
            return this;
        }

        /// <summary>
        /// Configures retry behavior.
        /// </summary>
        /// <param name="maxRetries">
        /// Maximum retry attempts.
        /// </param>
        /// <param name="delayMs">
        /// Retry delay in milliseconds.
        /// </param>
        public ApiRequest WithRetry(
            int maxRetries,
            int delayMs)
        {
            requestParams.MaxRetries = maxRetries;
            requestParams.RetryDelayMilliseconds = delayMs;

            return this;
        }

        #endregion

        #region Request Execution

        /// <summary>
        /// Sends request and returns raw API response.
        /// </summary>
        /// <returns>
        /// Raw <see cref="ApiResponse"/>.
        /// </returns>
        public Task<ApiResponse> SendAsync()
        {
            return apiManager.SendAsync(requestParams);
        }

        /// <summary>
        /// Sends request and automatically parses response data.
        /// </summary>
        /// <typeparam name="T">
        /// Target response data type.
        /// </typeparam>
        /// <returns>
        /// Strongly typed API response.
        /// </returns>
        public async Task<ApiResponse<T>> SendAsync<T>()
        {
            ApiResponse response =
                await apiManager.SendAsync(requestParams);

            ApiResponse<T> typedResponse =
                new ApiResponse<T>
                {
                    IsSuccess = response.IsSuccess,
                    StatusCode = response.StatusCode,
                    ResponseBody = response.ResponseBody,
                    ErrorMessage = response.ErrorMessage
                };

            if (response.IsSuccess &&
                !string.IsNullOrWhiteSpace(
                    response.ResponseBody))
            {
                typedResponse.Data =
                    JsonUtilityService.Deserialize<T>(
                        response.ResponseBody);
            }

            return typedResponse;
        }

        #endregion
    }
}