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
 **************************************************************************/

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MayaMystic.ApiFramework.Core.Utilities
{
    public static class JsonUtilityService
    {
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

        /// <summary>
        /// Serialize object to JSON string.
        /// </summary>
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                return string.Empty;

            try
            {
                return JsonConvert.SerializeObject(obj, DefaultSettings);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[JSON Serialize Error] {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            try
            {
                return JsonConvert.DeserializeObject<T>(json, DefaultSettings);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[JSON Deserialize Error] {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Try deserialize without throwing exception.
        /// </summary>
        public static bool TryDeserialize<T>(string json, out T result)
        {
            result = default;

            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                result = JsonConvert.DeserializeObject<T>(json, DefaultSettings);
                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[JSON TryDeserialize Error] {ex.Message}");
                return false;
            }
        }
    }
}