# Changelog

All notable changes to **MayaMystic API Framework** are documented in this file.

This project follows a structured changelog format to track improvements and new features across releases.

---
## [1.2.0] - 2026-03-05

### Added
- Introduced `ApiBodyType` enum for flexible request body handling.
- Added support for `application/x-www-form-urlencoded` requests.
- Added `FormFields` dictionary in `ApiRequestParams`.
- Added `AddFormField()` helper method.

### Improved
- Cleaner request body handling logic.
- Updated SampleLoginHandler to demonstrate:
  - FormUrlEncoded
  - JSON
  - Multipart request examples.

### Fixed
- Resolved 400 errors when calling APIs expecting form-urlencoded bodies.

## [1.1.0] - 2026-03-02

### ✨ Added
- Middleware Pipeline Architecture
- `LoggingMiddleware`
- `AuthMiddleware`
- `SmartRetryMiddleware`
- Token Provider abstraction (`ITokenProvider`)
- Endpoint Resolver interface (`IApiEndpointResolver`)
- Active API request tracking
- Request cancellation support
- Configurable request timeout handling

### 🚀 Improved
- Clear separation between infrastructure and request handling
- Middleware execution pipeline readability
- Authentication middleware architecture
- Internal request lifecycle flow

---

## [1.0.0] - 2026-03-01

### ✨ Added
- Initial `ApiManager` implementation using `HttpClient`
- Basic API request execution system
- JSON serialization support (Newtonsoft JSON)
- Core networking infrastructure
- Foundation for future middleware architecture

---
