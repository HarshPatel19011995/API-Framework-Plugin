using MayaMystic.ApiFramework.Core.Base;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Utilities;
using MayaMystic.ApiFramework.Core.Interfaces;
using UnityEngine;

namespace MayaMystic.ApiFramework.Samples
{
    public class SampleLoginHandler : ApiHandlerBase
    {
        private readonly IApiEndpointResolver resolver;
        private readonly string userId;
        private readonly string pin;

        public SampleLoginHandler(ApiManager manager,
                                  IApiEndpointResolver resolver,
                                  string userId,
                                  string pin)
            : base(manager)
        {
            this.resolver = resolver;
            this.userId = userId;
            this.pin = pin;
        }

        protected override ApiRequestParams BuildRequestParams()
        {
            var body = new
            {
                LoginUserId = userId,
                LoginPIN = pin
            };

            return new ApiRequestParams
            {
                Url = resolver.GetFullUrl("Login"),
                Verb = HttpVerb.POST,
                Content = JsonUtilityService.Serialize(body)
            };
        }

        protected override void OnSuccess(string json)
        {
            Debug.Log("Login Success Response: " + json);
        }

        protected override void OnUnauthorized(string json)
        {
            Debug.LogError("Unauthorized: " + json);
        }

        protected override void OnServerError(string error)
        {
            Debug.LogError("Server Error: " + error);
        }
    }
}