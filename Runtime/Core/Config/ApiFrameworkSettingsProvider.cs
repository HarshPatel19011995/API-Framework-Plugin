/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiFrameworkSettingsProvider.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Provides centralized access to framework settings asset.
 * 
 *  Features :
 *  - Lazy-loaded settings access
 *  - Centralized framework configuration
 *  - Runtime settings retrieval
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using UnityEngine;

namespace MayaMystic.ApiFramework.Core.Config
{
    /// <summary>
    /// Provides centralized framework settings access.
    /// </summary>
    /// <remarks>
    /// Handles runtime loading and caching of the
    /// <see cref="ApiFrameworkSettingsSO"/> asset.
    /// 
    /// Uses Unity Resources loading system for
    /// framework-wide configuration access.
    /// </remarks>
    public static class ApiFrameworkSettingsProvider
    {
        #region Private Variables

        /// <summary>
        /// Cached framework settings instance.
        /// </summary>
        private static ApiFrameworkSettingsSO settings;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets framework settings instance.
        /// </summary>
        /// <remarks>
        /// Automatically loads and caches settings asset
        /// from:
        /// 
        /// Resources/MayaMystic/ApiFrameworkSettings
        /// </remarks>
        public static ApiFrameworkSettingsSO Settings
        {
            get
            {
                if (settings == null)
                {
                    settings =
                        Resources.Load<ApiFrameworkSettingsSO>(
                            "MayaMystic/ApiFrameworkSettings");

#if UNITY_EDITOR
                    if (settings == null)
                    {
                        Debug.LogWarning(
                            "[ApiFrameworkSettingsProvider] " +
                            "ApiFrameworkSettings asset not found.");
                    }
#endif
                }

                return settings;
            }
        }

        #endregion
    }
}