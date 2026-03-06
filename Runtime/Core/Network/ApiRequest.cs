/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequest.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.1.0
 * 
 *  Description :
 *  Fluent request builder for creating API requests easily.
 *  Provides a developer-friendly wrapper around ApiRequestParams.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework
{
    public class ApiRequest
    {
        private readonly ApiRequestParams requestParams;

        private ApiRequest(string url)
        {
            requestParams = new ApiRequestParams(url);
        }

        /// <summary>
        /// Creates a new API request builder.
        /// </summary>
        public static ApiRequest Create(string url)
        {
            return new ApiRequest(url);
        }

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

        public ApiRequest WithRetry(int maxRetries, int delayMs)
        {
            requestParams.MaxRetries = maxRetries;
            requestParams.RetryDelayMilliseconds = delayMs;
            return this;
        }

        /// <summary>
        /// Sends the request using ApiManager.
        /// </summary>
        public Task<ApiResponse> SendAsync()
        {
            return ApiManager.SendAsync(requestParams);
        }
    }
}