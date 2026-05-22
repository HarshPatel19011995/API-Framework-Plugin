using MayaMystic.ApiFramework.Core.Managers;
using MayaMystic.ApiFramework.Core.Middleware;
using MayaMystic.ApiFramework.Core.Network;
using UnityEngine;
namespace MayaMystic.ApiFramework.Samples
{
    public class SampleLoginUI : MonoBehaviour
    {
        public SampleApiConfig ApiConfig;

        private ApiManager apiManager;

        private void Awake()
        {
			apiManager = new ApiManager();

			apiManager.UseMiddleware(
				new SmartRetryMiddleware(
					maxRetries: 3,
					baseDelayMs: 1000
				)
			);
		}
        
        [ContextMenu("Login")]
		public async void OnLoginClicked()
        {
            var handler = new SampleLoginHandler(
                apiManager,
                ApiConfig,
                "666",
                "1234",
                "test@example.com"
            );

            await handler.ExecuteAsync();
        }
    }
}