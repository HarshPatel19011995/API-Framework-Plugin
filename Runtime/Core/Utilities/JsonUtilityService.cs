/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : JsonUtilityService.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.0.0
 * 
 *  Description :
 *  Centralized JSON serialization/deserialization utility using
 *  Newtonsoft.Json with enterprise-grade configuration.
 * 
 *  Features :
 *  - Centralized serialization settings
 *  - CamelCase naming strategy
 *  - Safe deserialization
 *  - Exception-safe parsing
 *  - Serializer abstraction layer
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace MayaMystic.ApiFramework.Core.Utilities
{
    /// <summary>
    /// Provides centralized JSON serialization and deserialization utilities.
    /// </summary>
    /// <remarks>
    /// This service acts as the primary abstraction layer between the
    /// framework and Newtonsoft.Json implementation.
    /// 
    /// Future serializer replacements can be implemented here without
    /// modifying the networking pipeline.
    /// </remarks>
    public static class JsonUtilityService
    {
        #region Private Fields

        /// <summary>
        /// Default serializer configuration used across the framework.
        /// </summary>
        private static readonly JsonSerializerSettings DefaultSettings =
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes an object into a JSON string.
        /// </summary>
        /// <typeparam name="T">
        /// Object type to serialize.
        /// </typeparam>
        /// <param name="obj">
        /// Target object instance.
        /// </param>
        /// <returns>
        /// Serialized JSON string.
        /// Returns empty string if serialization fails.
        /// </returns>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
            {
                Debug.LogWarning(
                    "[JsonUtilityService] Serialize failed. Object is null.");

                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(
                    obj,
                    DefaultSettings);
            }
            catch (JsonException ex)
            {
                Debug.LogError(
                    $"[JsonUtilityService] Serialize Error: {ex.Message}");

                return string.Empty;
            }
        }

        /// <summary>
        /// Serializes an object into a formatted JSON string.
        /// </summary>
        /// <typeparam name="T">
        /// Object type to serialize.
        /// </typeparam>
        /// <param name="obj">
        /// Target object instance.
        /// </param>
        /// <param name="indented">
        /// Should output JSON be indented for readability.
        /// </param>
        /// <returns>
        /// Formatted JSON string.
        /// </returns>
        public static string Serialize<T>(
            T obj,
            bool indented)
        {
            if (obj == null)
            {
                Debug.LogWarning(
                    "[JsonUtilityService] Serialize failed. Object is null.");

                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(
                    obj,
                    indented
                        ? Formatting.Indented
                        : Formatting.None,
                    DefaultSettings);
            }
            catch (JsonException ex)
            {
                Debug.LogError(
                    $"[JsonUtilityService] Serialize Error: {ex.Message}");

                return string.Empty;
            }
        }

        /// <summary>
        /// Deserializes JSON string into target object type.
        /// </summary>
        /// <typeparam name="T">
        /// Target object type.
        /// </typeparam>
        /// <param name="json">
        /// Source JSON string.
        /// </param>
        /// <returns>
        /// Parsed object instance.
        /// Returns default value if parsing fails.
        /// </returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning(
                    "[JsonUtilityService] Deserialize failed. JSON is null or empty.");

                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(
                    json,
                    DefaultSettings);
            }
            catch (JsonException ex)
            {
                Debug.LogError(
                    $"[JsonUtilityService] Deserialize Error: {ex.Message}");

                return default;
            }
        }

        /// <summary>
        /// Attempts to deserialize JSON safely without throwing exceptions.
        /// </summary>
        /// <typeparam name="T">
        /// Target object type.
        /// </typeparam>
        /// <param name="json">
        /// Source JSON string.
        /// </param>
        /// <param name="result">
        /// Parsed object result.
        /// </param>
        /// <returns>
        /// True if deserialization succeeds; otherwise false.
        /// </returns>
        public static bool TryDeserialize<T>(
            string json,
            out T result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning(
                    "[JsonUtilityService] TryDeserialize failed. JSON is null or empty.");

                return false;
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(
                    json,
                    DefaultSettings);

                return true;
            }
            catch (JsonException ex)
            {
                Debug.LogError(
                    $"[JsonUtilityService] TryDeserialize Error: {ex.Message}");

                return false;
            }
        }

        #endregion
    }
}