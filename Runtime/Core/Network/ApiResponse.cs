/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiResponse.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.0.0
 * 
 *  Description :
 *  Standardized API response wrapper.
 *  Unity equivalent of Unreal callback response structure.
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Network
{
    /// <summary>
    /// Represents a standardized API response.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Indicates if the request was successful.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Raw response body.
        /// </summary>
        public string Body { get; }

        /// <summary>
        /// Error message if request failed.
        /// </summary>
        public string ErrorMessage { get; }

        public ApiResponse(bool success, int statusCode, string body, string error = null)
        {
            IsSuccess = success;
            StatusCode = statusCode;
            Body = body;
            ErrorMessage = error;
        }
    }
}