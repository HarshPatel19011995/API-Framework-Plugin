/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : SmartRetryMiddleware.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Smart retry middleware with:
 *  - Exponential backoff
 *  - Retry jitter
 *  - Status-code aware retry logic
 *  - Framework settings integration
 *  - Cancellation-aware retry delays
 * 
 *  Features :
 *  - Configurable retry handling
 *  - Exponential retry backoff
 *  - Retry jitter randomization
 *  - Framework settings fallback
 *  - Request-level retry overrides
 *  - Cancellation token support
 *  - Doxygen documentation support
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System;
using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Config;
using MayaMystic.ApiFramework.Core.Network;
using UnityEngine;

namespace MayaMystic.ApiFramework.Core.Middleware
{
    /// <summary>
    /// Middleware responsible for smart retry handling.
    /// </summary>
    /// <remarks>
    /// Provides:
    /// - Exponential retry backoff
    /// - Retry jitter
    /// - Transient error retry handling
    /// - Framework-level retry configuration
    /// - Request override support
    /// 
    /// Retries only transient failures such as:
    /// - Timeout
    /// - Server unavailable
    /// - Gateway failures
    /// </remarks>
    public class SmartRetryMiddleware : IApiMiddleware
    {
        #region Private Variables

        /// <summary>
        /// Explicit retry override.
        /// </summary>
        private readonly int maxRetries;

        /// <summary>
        /// Explicit retry delay override.
        /// </summary>
        private readonly int baseDelayMs;

        /// <summary>
        /// Explicit jitter override.
        /// </summary>
        private readonly bool useJitter;

        /// <summary>
        /// Shared random generator.
        /// </summary>
        private static readonly System.Random random =
            new();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SmartRetryMiddleware"/> class.
        /// </summary>
        /// <param name="maxRetries">
        /// Retry attempt override.
        /// Use -1 for framework settings fallback.
        /// </param>
        /// <param name="baseDelayMs">
        /// Retry delay override in milliseconds.
        /// Use -1 for framework settings fallback.
        /// </param>
        /// <param name="useJitter">
        /// Enable retry jitter randomization.
        /// </param>
        public SmartRetryMiddleware(
            int maxRetries = -1,
            int baseDelayMs = -1,
            bool useJitter = true)
        {
            this.maxRetries = maxRetries;
            this.baseDelayMs = baseDelayMs;
            this.useJitter = useJitter;
        }

        #endregion

        #region Middleware Execution

        /// <summary>
        /// Executes retry middleware pipeline.
        /// </summary>
        /// <param name="requestParams">
        /// Request parameter container.
        /// </param>
        /// <param name="next">
        /// Next middleware delegate.
        /// </param>
        /// <returns>
        /// API response result.
        /// </returns>
        public async Task<ApiResponse> InvokeAsync(
            ApiRequestParams requestParams,
            MiddlewareDelegate next)
        {
            ApiFrameworkSettingsSO settings =
                ApiFrameworkSettingsProvider.Settings;

            // ------------------------------------------------
            // Retry Disabled
            // ------------------------------------------------

            if (settings != null &&
                !settings.EnableRetry)
            {
                return await next(requestParams);
            }

            // ------------------------------------------------
            // Resolve Runtime Settings
            // ------------------------------------------------

            int resolvedMaxRetries =
                ResolveMaxRetries(
                    requestParams,
                    settings);

            int resolvedDelay =
                ResolveRetryDelay(
                    requestParams,
                    settings);

            bool resolvedUseJitter =
                settings == null
                    ? useJitter
                    : settings.EnableRetryJitter;

            bool useExponentialBackoff =
                settings != null &&
                settings.UseExponentialBackoff;

            int attempt = 0;

            while (true)
            {
                ApiResponse response =
                    await next(requestParams);

                // ------------------------------------------------
                // Retry Check
                // ------------------------------------------------

                if (!ShouldRetry(
                        response,
                        attempt,
                        resolvedMaxRetries))
                {
                    return response;
                }

                attempt++;

                int delay =
                    CalculateDelay(
                        resolvedDelay,
                        attempt,
                        useExponentialBackoff,
                        resolvedUseJitter);

#if UNITY_EDITOR || DEVELOPMENT_BUILD

                if (settings == null ||
                    settings.EnableLogging)
                {
                    Debug.LogWarning(
                        $"[API RETRY] " +
                        $"Attempt: {attempt}/{resolvedMaxRetries} | " +
                        $"StatusCode: {response?.StatusCode} | " +
                        $"Delay: {delay}ms | " +
                        $"URL: {requestParams.Url}");
                }

#endif

                // ------------------------------------------------
                // Cancellation-Aware Delay
                // ------------------------------------------------

                await Task.Delay(
                    delay,
                    requestParams.CancellationToken);
            }
        }

        #endregion

        #region Retry Logic

        /// <summary>
        /// Determines whether request should retry.
        /// </summary>
        private bool ShouldRetry(
            ApiResponse response,
            int attempt,
            int maxRetryCount)
        {
            // ------------------------------------------------
            // Retry Limit Reached
            // ------------------------------------------------

            if (attempt >= maxRetryCount)
            {
                return false;
            }

            // ------------------------------------------------
            // Null Response
            // ------------------------------------------------

            if (response == null)
            {
                return true;
            }

            // ------------------------------------------------
            // Success
            // ------------------------------------------------

            if (response.IsSuccess)
            {
                return false;
            }

            int code = response.StatusCode;

            // ------------------------------------------------
            // Retry Only Transient Errors
            // ------------------------------------------------

            return code ==
                       (int)NetworkResponseCode.RequestTimeout ||

                   code ==
                       (int)NetworkResponseCode.InternalServerError ||

                   code ==
                       (int)NetworkResponseCode.BadGateway ||

                   code ==
                       (int)NetworkResponseCode.ServiceUnavailable ||

                   code ==
                       (int)NetworkResponseCode.GatewayTimeout;
        }

        #endregion

        #region Delay Calculation

        /// <summary>
        /// Calculates retry delay duration.
        /// </summary>
        private int CalculateDelay(
            int delayMs,
            int attempt,
            bool exponentialBackoff,
            bool jitter)
        {
            int delay =
                exponentialBackoff
                    ? delayMs * (int)Math.Pow(2, attempt)
                    : delayMs;

            // ------------------------------------------------
            // Retry Jitter
            // ------------------------------------------------

            if (jitter)
            {
                delay += random.Next(0, 250);
            }

            return delay;
        }

        #endregion

        #region Runtime Resolution

        /// <summary>
        /// Resolves runtime retry count.
        /// </summary>
        private int ResolveMaxRetries(
            ApiRequestParams requestParams,
            ApiFrameworkSettingsSO settings)
        {
            if (requestParams.MaxRetries >= 0)
            {
                return requestParams.MaxRetries;
            }

            if (maxRetries >= 0)
            {
                return maxRetries;
            }

            return settings != null
                ? settings.DefaultMaxRetries
                : 3;
        }

        /// <summary>
        /// Resolves runtime retry delay.
        /// </summary>
        private int ResolveRetryDelay(
            ApiRequestParams requestParams,
            ApiFrameworkSettingsSO settings)
        {
            if (requestParams.RetryDelayMilliseconds >= 0)
            {
                return requestParams.RetryDelayMilliseconds;
            }

            if (baseDelayMs >= 0)
            {
                return baseDelayMs;
            }

            return settings != null
                ? settings.DefaultRetryDelayMs
                : 500;
        }

        #endregion
    }
}