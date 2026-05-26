/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : LoggingMiddleware.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.3.0
 *  Created     : 2026-03-06
 *  Last Updated: 2026-05-23
 * 
 *  Description :
 *  Middleware responsible for logging API requests and responses.
 * 
 *  Features :
 *  - Request logging
 *  - Response logging
 *  - Request body logging
 *  - Response body logging
 *  - Execution time tracking
 *  - Development build filtering
 *  - Doxygen documentation support
 * 
 *  Documentation :
 *  https://harshpatel19011995.github.io/API-Framework-Plugin/Documentation~/
 * 
 *  License :
 *  https://github.com/HarshPatel19011995/API-Framework-Plugin/blob/main/LICENSE.md
 * 
 *  Copyright (c) MayaMystic. All rights reserved.
 * 
 **************************************************************************/

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    /// <summary>
    /// Middleware responsible for API request and response logging.
    /// </summary>
    /// <remarks>
    /// Provides:
    /// - Request logging
    /// - Response logging
    /// - Body inspection
    /// - Request timing
    /// 
    /// Logging automatically runs only in:
    /// - Unity Editor
    /// - Development Builds
    /// 
    /// Prevents unnecessary logs in production builds.
    /// </remarks>
    public class LoggingMiddleware : IApiMiddleware
    {
        #region Private Variables

        /// <summary>
        /// Should request body be logged.
        /// </summary>
        private readonly bool logRequestBody;

        /// <summary>
        /// Should response body be logged.
        /// </summary>
        private readonly bool logResponseBody;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="logRequestBody">
        /// Enables request body logging.
        /// </param>
        /// <param name="logResponseBody">
        /// Enables response body logging.
        /// </param>
        public LoggingMiddleware(
            bool logRequestBody = false,
            bool logResponseBody = false)
        {
            this.logRequestBody = logRequestBody;
            this.logResponseBody = logResponseBody;
        }

        #endregion

        #region Middleware Execution

        /// <summary>
        /// Executes middleware logging pipeline.
        /// </summary>
        /// <param name="requestParams">
        /// API request parameters.
        /// </param>
        /// <param name="next">
        /// Next middleware delegate.
        /// </param>
        /// <returns>
        /// API response result.
        /// </returns>
        public async Task<ApiResponse> InvokeAsync(
            ApiRequestParams requestParams,
            MiddlewareDelegate next)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

            Stopwatch stopwatch =
                Stopwatch.StartNew();

            // ------------------------------------------------
            // Request Log
            // ------------------------------------------------

            Debug.Log(
                $"[API REQUEST] " +
                $"{requestParams.Verb} " +
                $"{requestParams.Url}");

            // ------------------------------------------------
            // Request Body
            // ------------------------------------------------

            if (logRequestBody)
            {
                string requestBody =
                    BuildRequestBodyLog(requestParams);

                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    Debug.Log(
                        $"[API REQUEST BODY]\n{requestBody}");
                }
            }

            // ------------------------------------------------
            // Execute Next Middleware
            // ------------------------------------------------

            ApiResponse response =
                await next(requestParams);

            stopwatch.Stop();

            // ------------------------------------------------
            // Response Log
            // ------------------------------------------------

            Debug.Log(
                $"[API RESPONSE] " +
                $"Status: {response.StatusCode} | " +
                $"Time: {stopwatch.ElapsedMilliseconds} ms");

            // ------------------------------------------------
            // Response Body
            // ------------------------------------------------

            if (logResponseBody &&
                !string.IsNullOrWhiteSpace(response.ResponseBody))
            {
                Debug.Log(
                    $"[API RESPONSE BODY]\n" +
                    $"{response.ResponseBody}");
            }

            return response;

#else
            return await next(requestParams);
#endif
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Builds formatted request body log string.
        /// </summary>
        /// <param name="requestParams">
        /// Request parameter container.
        /// </param>
        /// <returns>
        /// Formatted request body string.
        /// </returns>
        private string BuildRequestBodyLog(
            ApiRequestParams requestParams)
        {
            switch (requestParams.BodyType)
            {
                case ApiBodyType.Json:

                    return requestParams.JsonContent;

                case ApiBodyType.FormUrlEncoded:

                    if (requestParams.FormFields == null)
                        return null;

                    return string.Join(
                        "&",
                        requestParams.FormFields.Select(
                            kv => $"{kv.Key}={kv.Value}"));

                case ApiBodyType.Multipart:

                    return "[MULTIPART DATA]";

                default:

                    return null;
            }
        }

        #endregion
    }
}