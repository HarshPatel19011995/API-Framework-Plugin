/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequestParams.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.0.0
 * 
 *  Description :
 *  Encapsulates all necessary parameters for an API request.
 *  Unity equivalent of Unreal's FApiRequestParams.
 * 
 **************************************************************************/

using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MayaMystic.ApiFramework.Core.Network
{
    public class ApiRequestParams
    {
        // URL
        public string Url;

        // HTTP Verb
        public HttpVerb Verb = HttpVerb.GET;

        // Bearer token
        public string AuthToken;

        // JSON or raw body
        public string Content;

        // Multipart binary body
        public byte[] MultipartBody;

        // Boundary (auto-generated if null)
        public string MultipartBoundary;

        // Additional headers
        public Dictionary<string, string> AdditionalHeaders = new();

        // Timeout
        public int TimeoutSeconds = 10;

        // Retry
        public int MaxRetries = 3;
        public int RetryDelayMilliseconds = 5000;

        // Internal retry tracking
        internal int CurrentRetryAttempt = 0;

        // Multipart flag
        public bool IsMultipart = false;

        /// <summary>
        /// Builds HttpRequestMessage based on parameters.
        /// Equivalent to Unreal ApplyToRequest().
        /// </summary>
        public HttpRequestMessage BuildHttpRequestMessage()
        {
            var request = new HttpRequestMessage(
                new HttpMethod(Verb.ToString()),
                Url
            );

            // -------------------------
            // Headers
            // -------------------------

            if (IsMultipart)
            {
                MultipartBoundary ??= "----MayaMysticBoundary";

                request.Headers.TryAddWithoutValidation(
                    "Content-Type",
                    $"multipart/form-data; boundary={MultipartBoundary}"
                );
            }
            else
            {
                request.Headers.TryAddWithoutValidation(
                    "Content-Type",
                    "application/json"
                );
            }

            if (!string.IsNullOrEmpty(AuthToken))
            {
                request.Headers.TryAddWithoutValidation(
                    "Authorization",
                    $"Bearer {AuthToken}"
                );
            }

            foreach (var header in AdditionalHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // -------------------------
            // Body
            // -------------------------

            if (Verb != HttpVerb.GET)
            {
                if (IsMultipart && MultipartBody != null)
                {
                    request.Content = new ByteArrayContent(MultipartBody);
                }
                else if (!string.IsNullOrEmpty(Content))
                {
                    request.Content = new StringContent(
                        Content,
                        Encoding.UTF8,
                        "application/json"
                    );
                }
            }

            return request;
        }
    }
}