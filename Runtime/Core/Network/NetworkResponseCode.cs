/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : NetworkResponseCode.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.3.0
 * 
 *  Description :
 *  Standardized HTTP and network response codes used
 *  throughout the MayaMystic API Framework.
 * 
 *  Features :
 *  - Standard HTTP status codes
 *  - Framework-specific network errors
 *  - Backward compatibility aliases
 *  - Middleware-friendly response handling
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

namespace MayaMystic.ApiFramework.Core.Network
{
    /// <summary>
    /// Standardized network response codes.
    /// </summary>
    /// <remarks>
    /// Includes:
    /// - HTTP success codes
    /// - Client error codes
    /// - Server error codes
    /// - Framework-specific network errors
    /// 
    /// Backward compatibility aliases are preserved
    /// for older framework systems.
    /// </remarks>
    public enum NetworkResponseCode
    {
        #region Success

        /// <summary>
        /// HTTP 200 OK.
        /// </summary>
        OK = 200,

        /// <summary>
        /// Backward compatibility alias for OK.
        /// </summary>
        Ok = OK,

        /// <summary>
        /// HTTP 201 Created.
        /// </summary>
        Created = 201,

        /// <summary>
        /// HTTP 202 Accepted.
        /// </summary>
        Accepted = 202,

        /// <summary>
        /// HTTP 204 No Content.
        /// </summary>
        NoContent = 204,

        #endregion

        #region Client Errors

        /// <summary>
        /// HTTP 400 Bad Request.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// HTTP 401 Unauthorized.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// HTTP 403 Forbidden.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// HTTP 404 Not Found.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// HTTP 408 Request Timeout.
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// Backward compatibility alias for RequestTimeout.
        /// </summary>
        Timeout = RequestTimeout,

        /// <summary>
        /// HTTP 409 Conflict.
        /// </summary>
        Conflict = 409,

        #endregion

        #region Server Errors

        /// <summary>
        /// HTTP 500 Internal Server Error.
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// HTTP 502 Bad Gateway.
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// HTTP 503 Service Unavailable.
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// HTTP 504 Gateway Timeout.
        /// </summary>
        GatewayTimeout = 504,

        #endregion

        #region Framework Errors

        /// <summary>
        /// Unknown framework error.
        /// </summary>
        UnknownError = 1000,

        /// <summary>
        /// JSON serialization or deserialization failure.
        /// </summary>
        SerializationError = 1001,

        /// <summary>
        /// Network connection unavailable.
        /// </summary>
        NetworkUnavailable = 1002,

        /// <summary>
        /// Request manually cancelled.
        /// </summary>
        RequestCancelled = 1003

        #endregion
    }
}