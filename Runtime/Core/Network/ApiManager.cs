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
        // -------------------------
        // Request Info
        // -------------------------

        public string Url { get; set; }

        public HttpVerb Verb { get; set; } = HttpVerb.GET;

        public ApiBodyType BodyType { get; set; } = ApiBodyType.None;

        // -------------------------
        // Authorization
        // -------------------------

        public string AuthToken { get; set; }

        // -------------------------
        // Body
        // -------------------------

        public string JsonContent { get; set; }

        public Dictionary<string, string> FormFields { get; } = new();

        public byte[] MultipartBody { get; set; }

        public string MultipartBoundary { get; set; }

        // -------------------------
        // Headers
        // -------------------------

        public Dictionary<string, string> AdditionalHeaders { get; } = new();

        // -------------------------
        // Request Settings
        // -------------------------

        public int TimeoutSeconds { get; set; } = 10;

        public int MaxRetries { get; set; } = 3;

        public int RetryDelayMilliseconds { get; set; } = 5000;

        internal int CurrentRetryAttempt = 0;

        // -------------------------
        // Constructors
        // -------------------------

        public ApiRequestParams() { }

        public ApiRequestParams(string url, HttpVerb verb = HttpVerb.GET)
        {
            Url = url;
            Verb = verb;
        }

        // -------------------------
        // Helper Methods
        // -------------------------

        public void AddHeader(string key, string value)
        {
            AdditionalHeaders[key] = value;
        }

        public void AddFormField(string key, string value)
        {
            FormFields[key] = value;
        }

        public void SetJsonBody(string json)
        {
            BodyType = ApiBodyType.Json;
            JsonContent = json;
        }

        // -------------------------
        // Build Request
        // -------------------------

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

            // Authorization

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

            // GET requests usually have no body

            if (Verb == HttpVerb.GET)
                return request;

            // Body handling

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

                    var multipartContent = new MultipartFormDataContent(MultipartBoundary);

                    if (MultipartBody != null)
                    {
                        var fileContent = new ByteArrayContent(MultipartBody);
                        multipartContent.Add(fileContent, "file", "upload.bin");
                    }

                    request.Content = multipartContent;

                    break;
            }

            return request;
        }
    }
}