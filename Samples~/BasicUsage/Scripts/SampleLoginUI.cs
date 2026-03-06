using UnityEngine;
using MayaMystic.ApiFramework.Core.Network;

namespace MayaMystic.ApiFramework.Samples
{
    public class SampleLoginUI : MonoBehaviour
    {
        public SampleApiConfig ApiConfig;

        private ApiManager apiManager;

        private void Awake()
        {
            apiManager = new ApiManager();
        }

        public async void OnLoginClicked()
        {
            var handler = new SampleLoginHandler(
                apiManager,
                ApiConfig,
                "1234",
                "9999",
                "test@example.com"
            );

            await handler.ExecuteAsync();
        }
    }
}