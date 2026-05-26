/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequestParams.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Centralized API request parameter container.
 * 
 *  Stores:
 *  - HTTP request configuration
 *  - Authentication data
 *  - Headers
 *  - Request body content
 *  - Retry configuration
 *  - Timeout settings
 *  - Cancellation tokens
 * 
 *  Features :
 *  - Request configuration abstraction
 *  - Retry override support
 *  - Timeout override support
 *  - Multipart/form/json body handling
 *  - Header management
 *  - Cancellation token support
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace MayaMystic.ApiFramework.Core.Network
{
    /// <summary>
    /// Stores API request configuration parameters.
    /// </summary>
    /// <remarks>
    /// Acts as the transport container between:
    /// 
    /// - ApiRequest
    /// - Middleware pipeline
    /// - ApiManager
    /// 
    /// Stores request metadata and runtime configuration.
    /// </remarks>
    public class ApiRequestParams
    {
        #region Request Properties

        /// <summary>
        /// Gets or sets target request URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets HTTP request verb.
        /// </summary>
        public HttpVerb Verb { get; set; } =
            HttpVerb.GET;

        /// <summary>
        /// Gets or sets request body type.
        /// </summary>
        public ApiBodyType BodyType { get; set; } =
            ApiBodyType.None;

        #endregion

        #region Authentication

        /// <summary>
        /// Gets or sets bearer authentication token.
        /// </summary>
        public string AuthToken { get; set; }

        #endregion

        #region Body Content

        /// <summary>
        /// Gets or sets JSON request content.
        /// </summary>
        public string JsonContent { get; set; }

        /// <summary>
        /// Gets form field collection.
        /// </summary>
        public Dictionary<string, string> FormFields { get; }
            = new();

        /// <summary>
        /// Gets or sets multipart binary body.
        /// </summary>
        public byte[] MultipartBody { get; set; }

        /// <summary>
        /// Gets or sets multipart boundary identifier.
        /// </summary>
        public string MultipartBoundary { get; set; }

        #endregion

        #region Headers

        /// <summary>
        /// Gets additional request headers.
        /// </summary>
        public Dictionary<string, string> AdditionalHeaders { get; }
            = new();

        #endregion

        #region Retry & Timeout

        /// <summary>
        /// Gets or sets request timeout duration in seconds.
        /// </summary>
        /// <remarks>
        /// Use:
        /// 
        /// -1 = Use framework default timeout
        ///  0 = No timeout
        /// >0 = Explicit timeout value
        /// </remarks>
        public int TimeoutSeconds { get; set; } = -1;

        /// <summary>
        /// Gets or sets maximum retry attempts override.
        /// </summary>
        /// <remarks>
        /// Use:
        /// 
        /// -1 = Use framework default
        ///  0 = Disable retries
        /// >0 = Explicit retry count
        /// </remarks>
        public int MaxRetries { get; set; } = -1;

        /// <summary>
        /// Gets or sets retry delay override in milliseconds.
        /// </summary>
        /// <remarks>
        /// Use:
        /// 
        /// -1 = Use framework default delay
        /// >=0 = Explicit retry delay
        /// </remarks>
        public int RetryDelayMilliseconds { get; set; } = -1;

        /// <summary>
        /// Internal retry attempt tracker.
        /// </summary>
        internal int CurrentRetryAttempt = 0;

        #endregion

        #region Cancellation

        /// <summary>
        /// Gets or sets external cancellation token.
        /// </summary>
        /// <remarks>
        /// Allows external systems to cancel active requests.
        /// 
        /// Useful for:
        /// - Scene unload
        /// - UI destruction
        /// - Logout cleanup
        /// - Application shutdown
        /// </remarks>
        public CancellationToken CancellationToken { get; set; }
            = CancellationToken.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ApiRequestParams"/> class.
        /// </summary>
        public ApiRequestParams()
        {
        }

        /// <summary>
        /// Initializes request parameters with URL and HTTP verb.
        /// </summary>
        /// <param name="url">
        /// Target request URL.
        /// </param>
        /// <param name="verb">
        /// HTTP request verb.
        /// </param>
        public ApiRequestParams(
            string url,
            HttpVerb verb = HttpVerb.GET)
        {
            Url = url;
            Verb = verb;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Adds custom request header.
        /// </summary>
        /// <param name="key">
        /// Header key.
        /// </param>
        /// <param name="value">
        /// Header value.
        /// </param>
        public void AddHeader(
            string key,
            string value)
        {
            AdditionalHeaders[key] = value;
        }

        /// <summary>
        /// Adds form field entry.
        /// </summary>
        /// <param name="key">
        /// Form field key.
        /// </param>
        /// <param name="value">
        /// Form field value.
        /// </param>
        public void AddFormField(
            string key,
            string value)
        {
            FormFields[key] = value;
        }

        /// <summary>
        /// Sets JSON request body.
        /// </summary>
        /// <param name="json">
        /// Serialized JSON string.
        /// </param>
        public void SetJsonBody(string json)
        {
            BodyType = ApiBodyType.Json;
            JsonContent = json;
        }

        #endregion

        #region Request Builder

        /// <summary>
        /// Builds HTTP request message from current parameters.
        /// </summary>
        /// <returns>
        /// Configured HTTP request message.
        /// </returns>
        public HttpRequestMessage BuildHttpRequestMessage()
        {
            HttpRequestMessage request =
                new HttpRequestMessage(
                    new HttpMethod(Verb.ToString()),
                    Url);

            // ------------------------------------------------
            // Authorization
            // ------------------------------------------------

            if (!string.IsNullOrEmpty(AuthToken))
            {
                request.Headers.TryAddWithoutValidation(
                    "Authorization",
                    $"Bearer {AuthToken}");
            }

            // ------------------------------------------------
            // Additional Headers
            // ------------------------------------------------

            foreach (KeyValuePair<string, string> header
                     in AdditionalHeaders)
            {
                request.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }

            // ------------------------------------------------
            // GET Usually Has No Body
            // ------------------------------------------------

            if (Verb == HttpVerb.GET)
                return request;

            // ------------------------------------------------
            // Body Handling
            // ------------------------------------------------

            switch (BodyType)
            {
                case ApiBodyType.Json:

                    request.Content =
                        new StringContent(
                            JsonContent ?? string.Empty,
                            Encoding.UTF8,
                            "application/json");

                    break;

                case ApiBodyType.FormUrlEncoded:

                    request.Content =
                        new FormUrlEncodedContent(
                            FormFields);

                    break;

                case ApiBodyType.Multipart:

                    MultipartBoundary ??=
                        "----MayaMysticBoundary";

                    MultipartFormDataContent multipartContent =
                        new MultipartFormDataContent(
                            MultipartBoundary);

                    if (MultipartBody != null)
                    {
                        ByteArrayContent fileContent =
                            new ByteArrayContent(
                                MultipartBody);

                        multipartContent.Add(
                            fileContent,
                            "file",
                            "upload.bin");
                    }

                    request.Content = multipartContent;

                    break;
            }

            return request;
        }

        #endregion
    }
}