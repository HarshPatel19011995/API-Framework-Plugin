/**************************************************************************
 *  Project     : MayaMystic API Framework
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
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
            var response = await ApiManager.SendAsync(BuildRequestParams());
            RouteResponse(response);
        }

        private void RouteResponse(ApiResponse response)
        {
            if (response == null)
            {
                OnServerError("Null response received.");
                return;
            }

            if (response.IsSuccess)
            {
                switch ((NetworkResponseCode)response.StatusCode)
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

                    case NetworkResponseCode.InternalServerError:
                    case NetworkResponseCode.Timeout:
                    default:
                        OnServerError(response.ErrorMessage);
                        break;
                }
            }
            else
            {
                OnServerError(response.ErrorMessage);
            }
        }

        // ----------------------------------------
        // Override these in derived classes
        // ----------------------------------------

        protected virtual void OnSuccess(string json) { }

        protected virtual void OnBadRequest(string json) { }

        protected virtual void OnUnauthorized(string json) { }

        protected virtual void OnServerError(string error) { }
    }
}