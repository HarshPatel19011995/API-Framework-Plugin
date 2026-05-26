/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : SampleLoginHandler.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Sample login API handler demonstrating:
 * 
 *  - Fluent request builder usage
 *  - Typed API responses
 *  - JSON serialization
 *  - Login request handling
 *  - Framework middleware usage
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Managers;
using MayaMystic.ApiFramework.Core.Network;
using MayaMystic.ApiFramework.Core.Utilities;

namespace MayaMystic.ApiFramework.Samples.BasicUsage
{
    /// <summary>
    /// Sample login API handler.
    /// </summary>
    /// <remarks>
    /// Demonstrates:
    /// - Request creation
    /// - JSON serialization
    /// - Typed API responses
    /// - Login request flow
    /// </remarks>
    public class SampleLoginHandler
    {
        #region Private Variables

        /// <summary>
        /// Framework API manager instance.
        /// </summary>
        private readonly ApiManager apiManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SampleLoginHandler"/> class.
        /// </summary>
        /// <param name="apiManager">
        /// Framework API manager instance.
        /// </param>
        public SampleLoginHandler(
            ApiManager apiManager)
        {
            this.apiManager = apiManager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes sample login request.
        /// </summary>
        /// <param name="email">
        /// User email address.
        /// </param>
        /// <param name="password">
        /// User password.
        /// </param>
        /// <returns>
        /// Typed login API response.
        /// </returns>
        public async Task<ApiResponse<LoginResponse>>
            LoginAsync(
                string email,
                string password)
        {
            // ------------------------------------------------
            // Build Request Body
            // ------------------------------------------------

            LoginRequest requestBody =
                new LoginRequest
                {
                    Email = email,
                    Password = password
                };

            // ------------------------------------------------
            // Serialize JSON
            // ------------------------------------------------

            string json =
                JsonUtilityService.Serialize(
                    requestBody);

            // ------------------------------------------------
            // Create Request
            // ------------------------------------------------

            ApiResponse<LoginResponse> response =
                await ApiRequest
                    .Create(
                        "https://reqres.in/api/login",
                        apiManager)
                    .Post()
                    .WithHeader(
                        "Accept",
                        "application/json")
                    .WithJson(json)

                    // Optional Request Overrides
                    //.WithTimeout(10)
                    //.WithRetry(5, 1000)

                    .SendAsync<LoginResponse>();

            return response;
        }

        #endregion
    }

    /// <summary>
    /// Sample login request body.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Sample login response model.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Authentication token.
        /// </summary>
        public string Token { get; set; }
    }
}