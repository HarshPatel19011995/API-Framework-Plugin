/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : ApiHandlerBase.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.3.0
 * 
 *  Description :
 *  Base class for all API request handlers.
 *  Provides standardized request execution and response routing.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Utilities;

namespace MayaMystic.ApiFramework.Core.Base
{
    public abstract class ApiHandlerBase
    {
        protected readonly ApiManager ApiManager;

        protected ApiHandlerBase(ApiManager apiManager)
        {
            ApiManager = apiManager;
        }

        protected abstract ApiRequestParams BuildRequestParams();

        public async Task ExecuteAsync()
        {
            var requestParams = BuildRequestParams();

            if (requestParams == null)
            {
                OnServerError("Request parameters are null.");
                return;
            }

            var response = await ApiManager.SendAsync(requestParams);

            OnAnyResponse(response);

            RouteResponse(response);
        }

        private void RouteResponse(ApiResponse response)
        {
            if (response == null)
            {
                OnServerError("Null response received.");
                return;
            }

            var statusCode = (NetworkResponseCode)response.StatusCode;

            if (!response.IsSuccess)
            {
                OnServerError(response.ErrorMessage);
                return;
            }

            switch (statusCode)
            {
                case NetworkResponseCode.Ok:
                    OnSuccess(response.Body);
                    break;

                case NetworkResponseCode.BadRequest:
                    OnBadRequest(response.Body);
                    break;

                case NetworkResponseCode.Unauthorized:
                    OnUnauthorized(response.Body);
                    break;

                case NetworkResponseCode.Timeout:
                case NetworkResponseCode.InternalServerError:
                default:
                    OnServerError(response.ErrorMessage);
                    break;
            }
        }

        // ------------------------------------------------
        // Global Response Hook (optional override)
        // ------------------------------------------------

        protected virtual void OnAnyResponse(ApiResponse response) { }

        // ------------------------------------------------
        // Override these in derived classes
        // ------------------------------------------------

        protected virtual void OnSuccess(string json) { }

        protected virtual void OnBadRequest(string json) { }

        protected virtual void OnUnauthorized(string json) { }

        protected virtual void OnServerError(string error) { }
    }
}