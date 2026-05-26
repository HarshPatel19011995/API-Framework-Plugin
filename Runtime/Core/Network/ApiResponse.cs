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
 *  Features :
 *  - Base HTTP response model
 *  - Generic typed response support
 *  - Backward compatibility support
 *  - Strongly typed API data
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Network
{
    /// <summary>
    /// Represents a standardized API response model.
    /// </summary>
    /// <remarks>
    /// This class stores common HTTP response information used across
    /// the framework networking pipeline.
    /// 
    /// Acts as the base response container for all API requests.
    /// </remarks>
    public class ApiResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets whether the API request completed successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code returned by the server.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the raw response body returned from the server.
        /// </summary>
        /// <remarks>
        /// Stores unprocessed response payload as raw JSON/string data.
        /// </remarks>
        public string ResponseBody { get; set; }

        /// <summary>
        /// Gets or sets backward-compatible response body access.
        /// </summary>
        /// <remarks>
        /// Maintained for compatibility with older framework systems.
        /// Internally maps to <see cref="ResponseBody"/>.
        /// </remarks>
        public string Body
        {
            get => ResponseBody;
            set => ResponseBody = value;
        }

        /// <summary>
        /// Gets or sets the error message if request fails.
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Initializes a new API response with response details.
        /// </summary>
        /// <param name="isSuccess">
        /// Indicates whether request completed successfully.
        /// </param>
        /// <param name="statusCode">
        /// HTTP status code returned by server.
        /// </param>
        /// <param name="responseBody">
        /// Raw response body content.
        /// </param>
        /// <param name="errorMessage">
        /// Error message if request failed.
        /// </param>
        public ApiResponse(
            bool isSuccess,
            int statusCode,
            string responseBody,
            string errorMessage = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            ResponseBody = responseBody;
            ErrorMessage = errorMessage;
        }

        #endregion
    }

    /// <summary>
    /// Represents a strongly typed API response model.
    /// </summary>
    /// <typeparam name="T">
    /// Parsed response data type.
    /// </typeparam>
    /// <remarks>
    /// Extends <see cref="ApiResponse"/> by providing automatic
    /// strongly typed response data support.
    /// </remarks>
    public class ApiResponse<T> : ApiResponse
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets parsed response data object.
        /// </summary>
        public T Data { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ApiResponse{T}"/> class.
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Initializes a new typed API response with response details.
        /// </summary>
        /// <param name="isSuccess">
        /// Indicates whether request completed successfully.
        /// </param>
        /// <param name="statusCode">
        /// HTTP status code returned by server.
        /// </param>
        /// <param name="responseBody">
        /// Raw response body content.
        /// </param>
        /// <param name="errorMessage">
        /// Error message if request failed.
        /// </param>
        public ApiResponse(
            bool isSuccess,
            int statusCode,
            string responseBody,
            string errorMessage = null)
            : base(
                isSuccess,
                statusCode,
                responseBody,
                errorMessage)
        {
        }

        #endregion
    }
}