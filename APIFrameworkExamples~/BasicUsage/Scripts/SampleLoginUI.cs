/**************************************************************************
 * 
 *  Project     : MayaMystic API Framework
 *  File        : SampleLoginUI.cs
 *  Author      : Harsh Patel
 *  Company     : MayaMystic
 *  Version     : 1.4.0
 * 
 *  Description :
 *  Sample login UI demonstrating:
 * 
 *  - ApiManager usage
 *  - Middleware setup
 *  - Typed API responses
 *  - Login request flow
 *  - Framework logging
 *  - Retry middleware
 * 
 *  Copyright © 2026 MayaMystic. All Rights Reserved.
 * 
 **************************************************************************/

using System.Threading.Tasks;
using MayaMystic.ApiFramework.Core.Managers;
using MayaMystic.ApiFramework.Core.Middleware;
using MayaMystic.ApiFramework.Core.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MayaMystic.ApiFramework.Samples.BasicUsage
{
    /// <summary>
    /// Sample login UI controller.
    /// </summary>
    /// <remarks>
    /// Demonstrates:
    /// - Middleware registration
    /// - Login request execution
    /// - Typed response handling
    /// - Basic UI interaction
    /// </remarks>
    public class SampleLoginUI : MonoBehaviour
    {
        #region Inspector References

        [Header("UI References")]

        [SerializeField]
        private TMP_InputField emailInput;

        [SerializeField]
        private TMP_InputField passwordInput;

        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private TMP_Text resultText;

        #endregion

        #region Private Variables

        /// <summary>
        /// Framework API manager instance.
        /// </summary>
        private ApiManager apiManager;

        /// <summary>
        /// Sample login handler.
        /// </summary>
        private SampleLoginHandler loginHandler;

        #endregion

        #region Unity Lifecycle

        /// <summary>
        /// Unity start callback.
        /// </summary>
        private void Start()
        {
            InitializeFramework();

            if (loginButton != null)
            {
                loginButton.onClick.AddListener(
                    OnLoginClicked);
            }
        }

        /// <summary>
        /// Unity destroy callback.
        /// </summary>
        private void OnDestroy()
        {
            loginButton?.onClick.RemoveListener(
                OnLoginClicked);

            apiManager?.Dispose();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes framework systems.
        /// </summary>
        private void InitializeFramework()
        {
            // ------------------------------------------------
            // Create API Manager
            // ------------------------------------------------

            apiManager =
                new ApiManager();

            // ------------------------------------------------
            // Register Middleware
            // ------------------------------------------------

            apiManager.UseMiddleware(
                new SmartRetryMiddleware());

            apiManager.UseMiddleware(
                new LoggingMiddleware(
                    logRequestBody: true,
                    logResponseBody: true));

            // ------------------------------------------------
            // Create Handler
            // ------------------------------------------------

            loginHandler =
                new SampleLoginHandler(
                    apiManager);
        }

        #endregion

        #region UI Events

        /// <summary>
        /// Invoked when login button is clicked.
        /// </summary>
        private async void OnLoginClicked()
        {
            await ExecuteLoginAsync();
        }

        #endregion

        #region Login Flow

        /// <summary>
        /// Executes login request flow.
        /// </summary>
        private async Task ExecuteLoginAsync()
        {
            // ------------------------------------------------
            // Validate Input
            // ------------------------------------------------

            if (string.IsNullOrWhiteSpace(
                    emailInput.text) ||

                string.IsNullOrWhiteSpace(
                    passwordInput.text))
            {
                SetResult(
                    "Email and password are required.",
                    true);

                return;
            }

            // ------------------------------------------------
            // Disable UI
            // ------------------------------------------------

            SetInteractable(false);

            SetResult(
                "Sending login request...",
                false);

            // ------------------------------------------------
            // Execute Login Request
            // ------------------------------------------------

            ApiResponse<LoginResponse> response =
                await loginHandler.LoginAsync(
                    emailInput.text,
                    passwordInput.text);

            // ------------------------------------------------
            // Handle Response
            // ------------------------------------------------

            if (response.IsSuccess &&
                response.Data != null)
            {
                SetResult(
                    $"Login Success!\n\nToken:\n{response.Data.Token}",
                    false);
            }
            else
            {
                SetResult(
                    $"Login Failed:\n{response.ErrorMessage}",
                    true);
            }

            // ------------------------------------------------
            // Re-enable UI
            // ------------------------------------------------

            SetInteractable(true);
        }

        #endregion

        #region UI Helpers

        /// <summary>
        /// Updates result label.
        /// </summary>
        /// <param name="message">
        /// Result message.
        /// </param>
        /// <param name="isError">
        /// Should display as error.
        /// </param>
        private void SetResult(
            string message,
            bool isError)
        {
            if (resultText == null)
                return;

            resultText.text = message;

            resultText.color =
                isError
                    ? Color.red
                    : Color.green;
        }

        /// <summary>
        /// Sets UI interactable state.
        /// </summary>
        /// <param name="value">
        /// Interactable state.
        /// </param>
        private void SetInteractable(
            bool value)
        {
            if (loginButton != null)
            {
                loginButton.interactable =
                    value;
            }
        }

        #endregion
    }
}