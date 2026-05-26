/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiEnvironmentConfig.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  ScriptableObject-based API environment configuration.
 * 
 *  Stores:
 *  - Base URL
 *  - Environment information
 *  - Endpoint mappings
 *  - API versioning
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace MayaMystic.ApiFramework.Core.Config
{
    /// <summary>
    /// ScriptableObject-based API environment configuration.
    /// </summary>
    /// <remarks>
    /// Stores environment-specific networking configuration.
    /// 
    /// Supports:
    /// - Development environment
    /// - Staging environment
    /// - Production environment
    /// 
    /// Used by endpoint resolver and request systems.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "ApiEnvironmentConfig",
        menuName = "MayaMystic/API Framework/Environment Config")]
    public class ApiEnvironmentConfig :
        ScriptableObject
    {
        #region Environment

        [Header("Environment")]

        [Tooltip("Environment display name.")]
        public string EnvironmentName =
            "Development";

        [Tooltip("Base server URL.")]
        public string BaseUrl =
            "https://api.example.com";

        [Tooltip("API version identifier.")]
        public string ApiVersion =
            "v1";

        [Tooltip("Allow insecure SSL certificates.")]
        public bool AllowInsecureCertificates =
            false;

        #endregion

        #region Endpoints

        [Header("Endpoints")]

        [Tooltip("API endpoint mappings.")]
        public List<ApiEndpointEntry> Endpoints =
            new();

        #endregion
    }
}