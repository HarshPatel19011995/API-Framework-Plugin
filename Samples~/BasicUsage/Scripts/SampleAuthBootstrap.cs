using UnityEngine;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Middleware;

namespace MayaMystic.ApiFramework.Samples
{
    /// <summary>
    /// Demonstrates how to register AuthMiddleware
    /// using static or dynamic token providers.
    /// </summary>
    public class SampleAuthBootstrap : MonoBehaviour
    {
        private ApiManager apiManager;
        private SampleRuntimeTokenProvider runtimeProvider;

       private void Awake()
        {
            apiManager = new ApiManager();

            // Logging middleware
            apiManager.UseMiddleware(new LoggingMiddleware());

            // OPTION 1: Static Token (API key style)
            var staticProvider = new SampleStaticTokenProvider("fixed_api_key");

            // apiManager.UseMiddleware(new AuthMiddleware(staticProvider));

            // OPTION 2: Dynamic Token (Login style)
            runtimeProvider = new SampleRuntimeTokenProvider();
            apiManager.UseMiddleware(new AuthMiddleware(runtimeProvider));
        }

        // Simulate login success
        public void OnLoginSuccess(string receivedToken)
        {
            runtimeProvider.SetToken(receivedToken);
        }
    }
}