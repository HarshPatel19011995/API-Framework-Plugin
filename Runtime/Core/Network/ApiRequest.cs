/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequest.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.2.0
 * 
 *  Description :
 *  Fluent request builder for creating API requests easily.
 *  Provides a developer-friendly wrapper around ApiRequestParams.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Managers;

namespace MayaMystic.ApiFramework
{
    public class ApiRequest
    {
        // ------------------------------------------------
        // Variables
        // ------------------------------------------------

        private readonly ApiRequestParams requestParams;

        private readonly ApiManager apiManager;

        // ------------------------------------------------
        // Constructor
        // ------------------------------------------------

        private ApiRequest(
            string url,
            ApiManager apiManager)
        {
            requestParams = new ApiRequestParams(url);
            this.apiManager = apiManager;
        }

        // ------------------------------------------------
        // Factory
        // ------------------------------------------------

        /// <summary>
        /// Creates a new API request builder.
        /// </summary>
        public static ApiRequest Create(
            string url,
            ApiManager apiManager)
        {
            return new ApiRequest(url, apiManager);
        }

        // ------------------------------------------------
        // HTTP Methods
        // ------------------------------------------------

        public ApiRequest Get()
        {
            requestParams.Verb = HttpVerb.GET;
            return this;
        }

        public ApiRequest Post()
        {
            requestParams.Verb = HttpVerb.POST;
            return this;
        }

        public ApiRequest Put()
        {
            requestParams.Verb = HttpVerb.PUT;
            return this;
        }

        public ApiRequest Delete()
        {
            requestParams.Verb = HttpVerb.DELETE;
            return this;
        }

        // ------------------------------------------------
        // Request Configuration
        // ------------------------------------------------

        public ApiRequest WithAuth(string token)
        {
            requestParams.AuthToken = token;
            return this;
        }

        public ApiRequest WithHeader(string key, string value)
        {
            requestParams.AdditionalHeaders[key] = value;
            return this;
        }

        public ApiRequest WithJson(string json)
        {
            requestParams.BodyType = ApiBodyType.Json;
            requestParams.JsonContent = json;
            return this;
        }

        public ApiRequest WithFormField(string key, string value)
        {
            requestParams.BodyType = ApiBodyType.FormUrlEncoded;
            requestParams.AddFormField(key, value);
            return this;
        }

        public ApiRequest WithTimeout(int seconds)
        {
            requestParams.TimeoutSeconds = seconds;
            return this;
        }

        public ApiRequest WithRetry(
            int maxRetries,
            int delayMs)
        {
            requestParams.MaxRetries = maxRetries;
            requestParams.RetryDelayMilliseconds = delayMs;
            return this;
        }

        // ------------------------------------------------
        // Send Request
        // ------------------------------------------------

        /// <summary>
        /// Sends the request using ApiManager.
        /// </summary>
        public Task<ApiResponse> SendAsync()
        {
            return apiManager.SendAsync(requestParams);
        }
    }
}