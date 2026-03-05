/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiRequestParams.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.1.0
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

        // Body type
        public ApiBodyType BodyType = ApiBodyType.None;

        // Bearer token
        public string AuthToken;

        // JSON body
        public string JsonContent;

        // Form fields
        public Dictionary<string, string> FormFields = new();

        // Multipart binary body
        public byte[] MultipartBody;

        // Multipart boundary
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

        /// <summary>
        /// Helper to add form field
        /// </summary>
        public void AddFormField(string key, string value)
        {
            FormFields[key] = value;
        }

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
            // Authorization
            // -------------------------

            if (!string.IsNullOrEmpty(AuthToken))
            {
                request.Headers.TryAddWithoutValidation(
                    "Authorization",
                    $"Bearer {AuthToken}"
                );
            }

            // Additional headers
            foreach (var header in AdditionalHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // -------------------------
            // Body Handling
            // -------------------------

            if (Verb == HttpVerb.GET)
                return request;

            switch (BodyType)
            {
                case ApiBodyType.Json:

                    request.Content = new StringContent(
                        JsonContent ?? "",
                        Encoding.UTF8,
                        "application/json"
                    );

                    break;

                case ApiBodyType.FormUrlEncoded:

                    request.Content = new FormUrlEncodedContent(FormFields);

                    break;

                case ApiBodyType.Multipart:

                    MultipartBoundary ??= "----MayaMysticBoundary";

                    var content = new MultipartFormDataContent(MultipartBoundary);

                    if (MultipartBody != null)
                    {
                        var fileContent = new ByteArrayContent(MultipartBody);
                        content.Add(fileContent, "file", "upload.bin");
                    }

                    request.Content = content;

                    break;
            }

            return request;
        }
    }
}