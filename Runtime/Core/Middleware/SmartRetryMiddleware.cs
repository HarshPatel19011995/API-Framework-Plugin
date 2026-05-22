/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *
 *  Description :
 *  Smart retry middleware with exponential backoff and
 *  status-code aware retry logic.
 **************************************************************************/

using System;
using System.Threading.Tasks;
using UnityEngine;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Core.Middleware
{
	public class SmartRetryMiddleware : IApiMiddleware
	{
		private readonly int maxRetries;
		private readonly int baseDelayMs;
		private readonly bool useJitter;

		public SmartRetryMiddleware(
			int maxRetries = 3,
			int baseDelayMs = 500,
			bool useJitter = true)
		{
			this.maxRetries = maxRetries;
			this.baseDelayMs = baseDelayMs;
			this.useJitter = useJitter;
		}

		public async Task<ApiResponse> InvokeAsync(
			ApiRequestParams requestParams,
			MiddlewareDelegate next)
		{
			int attempt = 0;

			while (true)
			{
				var response = await next(requestParams);

				// No retry needed

				if (!ShouldRetry(response, attempt))
				{
					return response;
				}

				attempt++;

				int delay = CalculateDelay(attempt);

				Debug.LogWarning(
					$"[API RETRY] " +
					$"Attempt: {attempt}/{maxRetries} | " +
					$"StatusCode: {response?.StatusCode} | " +
					$"Delay: {delay}ms | " +
					$"URL: {requestParams.Url}"
				);

				await Task.Delay(delay);
			}
		}

		private bool ShouldRetry(
			ApiResponse response,
			int attempt)
		{
			// Max retries reached

			if (attempt >= maxRetries)
			{
				Debug.LogError(
					$"[API RETRY FAILED] " +
					$"Max retry attempts reached."
				);

				return false;
			}

			// Null response

			if (response == null)
			{
				return true;
			}

			// Successful request

			if (response.IsSuccess)
			{
				return false;
			}

			int code = response.StatusCode;

			// Retry only transient/server/network errors

			return code == 408 ||   // Timeout
				   code == 500 ||   // Internal Server Error
				   code == 502 ||   // Bad Gateway
				   code == 503 ||   // Service Unavailable
				   code == 504;     // Gateway Timeout
		}

		private int CalculateDelay(int attempt)
		{
			// Exponential backoff

			int delay =
				baseDelayMs * (int)Math.Pow(2, attempt);

			// Add jitter to avoid retry spikes

			if (useJitter)
			{
				var random = new System.Random();

				delay += random.Next(0, 250);
			}

			return delay;
		}
	}
}