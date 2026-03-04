MayaMystic API Framework - Basic Usage Sample

Steps:

1. Import this sample from Package Manager.
2. Create Sample API Config (Right Click → Create → MayaMystic → Sample API Config).
3. Set Base URL and endpoints.
4. Add SampleLoginUI to a GameObject.
5. Assign ApiConfig.
6. Call OnLoginClicked() from button.


AUTHENTICATION SAMPLE

This SDK supports both static and dynamic token usage.

1. Static Token (API key style)
   - Use SampleStaticTokenProvider
   - Suitable for fixed API key systems

2. Dynamic Token (Login-based JWT)
   - Use SampleRuntimeTokenProvider
   - Call SetToken() after login

Register middleware:
apiManager.UseMiddleware(new AuthMiddleware(tokenProvider));

Auth is automatically injected into every request.