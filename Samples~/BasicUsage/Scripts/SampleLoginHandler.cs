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
        private readonly string email;

        public SampleLoginHandler(
            ApiManager manager,
            IApiEndpointResolver resolver,
            string userId,
            string pin,
            string email)
            : base(manager)
        {
            this.resolver = resolver;
            this.userId = userId;
            this.pin = pin;
            this.email = email;
        }

        protected override ApiRequestParams BuildRequestParams()
        {
            // ------------------------------------------------
            // OPTION 1 : FORM URL ENCODED (Most Login APIs)
            // ------------------------------------------------

            var request = new ApiRequestParams
            {
                Url = resolver.GetFullUrl(nameof(SampleApiConfig.Login)),
                Verb = HttpVerb.POST,
                BodyType = ApiBodyType.FormUrlEncoded
            };

            request.AddFormField("LoginUserid", userId);
            request.AddFormField("LoginPIN", pin);
            request.AddFormField("LoginUserName", email);

            return request;


            // ------------------------------------------------
            // OPTION 2 : JSON API (Example)
            // ------------------------------------------------
            /*
            var body = new
            {
                LoginUserId = userId,
                LoginPIN = pin
            };

            return new ApiRequestParams
            {
                Url = resolver.GetFullUrl("Login"),
                Verb = HttpVerb.POST,
                BodyType = ApiBodyType.Json,
                JsonContent = JsonUtilityService.Serialize(body)
            };
            */


            // ------------------------------------------------
            // OPTION 3 : Multipart Upload Example
            // ------------------------------------------------
            /*
            byte[] fileData = System.IO.File.ReadAllBytes("path/to/file");

            return new ApiRequestParams
            {
                Url = resolver.GetFullUrl("Upload"),
                Verb = HttpVerb.POST,
                BodyType = ApiBodyType.Multipart,
                MultipartBody = fileData
            };
            */
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