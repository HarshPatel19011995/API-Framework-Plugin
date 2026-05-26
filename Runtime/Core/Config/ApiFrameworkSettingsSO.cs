/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiFrameworkSettingsSO.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Centralized framework configuration asset.
 * 
 *  Stores:
 *  - Retry settings
 *  - Timeout settings
 *  - Logging settings
 *  - Middleware behavior
 *  - Network defaults
 * 
 *  Features :
 *  - ScriptableObject-based configuration
 *  - Retry system configuration
 *  - Logging system configuration
 *  - Timeout management
 *  - Global framework defaults
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using UnityEngine;

namespace MayaMystic.ApiFramework.Core.Config
{
    /// <summary>
    /// Centralized API framework configuration asset.
    /// </summary>
    /// <remarks>
    /// Provides global framework-level configuration used by:
    /// 
    /// - ApiManager
    /// - Retry middleware
    /// - Logging middleware
    /// - Request systems
    /// - Networking behavior
    /// 
    /// This asset allows project-wide framework configuration
    /// without hardcoded values.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "ApiFrameworkSettings",
        menuName = "MayaMystic/API Framework/Framework Settings")]
    public class ApiFrameworkSettingsSO :
        ScriptableObject
    {
        #region Retry Settings

        [Header("Retry Settings")]

        /// <summary>
        /// Enables automatic retry handling.
        /// </summary>
        [Tooltip("Enable automatic request retry handling.")]
        public bool EnableRetry = true;

        /// <summary>
        /// Default maximum retry attempts.
        /// </summary>
        [Tooltip("Default maximum retry attempts.")]
        [Min(0)]
        public int DefaultMaxRetries = 3;

        /// <summary>
        /// Default retry delay in milliseconds.
        /// </summary>
        [Tooltip("Base retry delay in milliseconds.")]
        [Min(0)]
        public int DefaultRetryDelayMs = 500;

        /// <summary>
        /// Enables exponential retry backoff.
        /// </summary>
        [Tooltip("Enable exponential retry backoff.")]
        public bool UseExponentialBackoff = true;

        /// <summary>
        /// Enables retry delay jitter randomization.
        /// </summary>
        [Tooltip("Enable retry delay random jitter.")]
        public bool EnableRetryJitter = true;

        #endregion

        #region Timeout Settings

        [Header("Timeout Settings")]

        /// <summary>
        /// Default request timeout duration in seconds.
        /// </summary>
        [Tooltip("Default request timeout duration in seconds.")]
        [Min(1)]
        public int DefaultTimeoutSeconds = 15;

        #endregion

        #region Logging Settings

        [Header("Logging Settings")]

        /// <summary>
        /// Enables framework logging.
        /// </summary>
        [Tooltip("Enable framework logging.")]
        public bool EnableLogging = true;

        /// <summary>
        /// Enables request logging.
        /// </summary>
        [Tooltip("Enable request logging.")]
        public bool EnableRequestLogging = true;

        /// <summary>
        /// Enables response logging.
        /// </summary>
        [Tooltip("Enable response logging.")]
        public bool EnableResponseLogging = true;

        /// <summary>
        /// Enables request body logging.
        /// </summary>
        [Tooltip("Enable request body logging.")]
        public bool EnableRequestBodyLogging = false;

        /// <summary>
        /// Enables response body logging.
        /// </summary>
        [Tooltip("Enable response body logging.")]
        public bool EnableResponseBodyLogging = false;

        #endregion

        #region Network Settings

        [Header("Network Settings")]

        /// <summary>
        /// Default framework user-agent identifier.
        /// </summary>
        [Tooltip("Default user agent identifier.")]
        public string UserAgent =
            "MayaMysticApiFramework/1.4.0";

        /// <summary>
        /// Enables automatic HTTP redirect handling.
        /// </summary>
        [Tooltip("Enable automatic redirect handling.")]
        public bool AllowAutoRedirect = true;

        #endregion
    }
}