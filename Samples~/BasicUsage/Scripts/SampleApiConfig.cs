using UnityEngine;
using MayaMystic.ApiFramework.Core.Interfaces;

namespace MayaMystic.ApiFramework.Samples
{
    [CreateAssetMenu(menuName = "MayaMystic/Sample API Config")]
    public class SampleApiConfig : ScriptableObject, IApiEndpointResolver
    {
        public string BaseUrl = "https://yourapi.com";
        public string Login = "/login";

        public string GetFullUrl(string endpointKey)
        {
            return endpointKey switch
            {
                nameof(Login) => BaseUrl + Login,
                _ => string.Empty
            };
        }
    }
}