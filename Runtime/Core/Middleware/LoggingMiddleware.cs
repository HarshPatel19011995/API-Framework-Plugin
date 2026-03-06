/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : LoggingMiddleware.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.1.0
 *  Created     : 2026-03-06
 *  Last Updated: 2026-03-06
 * 
 *  Description :
 *  Middleware responsible for logging API requests and responses.
 *  Supports optional logging of request and response bodies for
 *  debugging and development.
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
