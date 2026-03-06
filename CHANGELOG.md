# Changelog

All notable changes to **MayaMystic API Framework** are documented in this file.

This project follows a structured changelog format to track improvements and new features across releases.

---

## [1.3.0] - 2026-03-06

### ✨ Added
- Support for multiple request body types via `ApiBodyType`
- `application/x-www-form-urlencoded` request support
- `FormFields` dictionary in `ApiRequestParams`
- `AddFormField()` helper method

### 🚀 Improved
- Refactored request body handling logic in `ApiRequestParams`
- Improved `LoggingMiddleware` to support new body types
- Updated sample request handler demonstrating JSON, FormUrlEncoded, and Multipart requests

### 🐛 Fixed
- Fixed HTTP 400 errors when calling APIs expecting form-urlencoded request bodies

---

## [1.2.0] - 2026-03-05

### ✨ Added
- `ApiBodyType` enum for flexible request body handling
- Multipart request support
- Multipart binary upload support

### 🚀 Improved
- Enhanced request building logic
- Cleaner internal request parameter handling

---

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