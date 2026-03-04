🚀 MayaMystic API Framework
<p align="center"> <img src="https://raw.githubusercontent.com/HarshPatel19011995/API-Framework-Plugin/main/.github/banner.png" width="850"/> </p> <p align="center"> <b>Enterprise-grade middleware-based API networking framework for Unity</b> </p> <p align="center">








</p>
📌 Overview

MayaMystic API Framework is a production-ready networking SDK for Unity.

It introduces a middleware pipeline architecture allowing developers to build scalable API systems with clean architecture.

The framework provides built-in solutions for:

Authentication

Logging

Retry strategies

Endpoint resolution

Request lifecycle management

⚡ Quick Start
1️⃣ Install via Git

Open Unity Package Manager

Window → Package Manager

Click:

+ → Add package from Git URL

Paste:

https://github.com/HarshPatel19011995/API-Framework-Plugin.git#v1.1.0
2️⃣ Create ApiManager
var apiManager = new ApiManager();
3️⃣ Register Middleware
apiManager.UseMiddleware(new LoggingMiddleware());
apiManager.UseMiddleware(new AuthMiddleware(tokenProvider));
apiManager.UseMiddleware(new SmartRetryMiddleware());
4️⃣ Execute API Request
await handler.ExecuteAsync();
✨ Key Features
Feature	Description
Async ApiManager	Centralized API request execution
Middleware Pipeline	Modular request processing
Authentication Middleware	Automatic Bearer token injection
Smart Retry System	Handles transient network failures
Token Provider	Abstract token management
Endpoint Resolver	Flexible API endpoint configuration
JSON Serialization	Powered by Newtonsoft JSON
🧠 Architecture

The framework follows a middleware pipeline architecture inspired by modern backend systems.

ApiHandler
     ↓
ApiManager
     ↓
Middleware Pipeline
     ├── LoggingMiddleware
     ├── AuthMiddleware
     ├── SmartRetryMiddleware
     ↓
HttpClient
     ↓
Remote Server
Benefits

Clean separation of concerns

Extensible networking architecture

Middleware driven request lifecycle

Production-ready scalability

🔄 API Request Lifecycle
Client Code
     ↓
ApiHandler
     ↓
ApiManager
     ↓
Middleware Processing
     ↓
HTTP Request
     ↓
Server Response
     ↓
JSON Deserialization
     ↓
Typed Model
     ↓
Result Returned
⚙ Core Components
🔹 ApiManager

Responsible for executing API requests.

Features:

Async request execution

Request timeout handling

Cancellation support

Middleware-driven request processing

Example:

var apiManager = new ApiManager();
🔹 Middleware Pipeline

Middleware processes requests before and after HTTP execution.

Example:

apiManager.UseMiddleware(new LoggingMiddleware());
apiManager.UseMiddleware(new AuthMiddleware(tokenProvider));
apiManager.UseMiddleware(new SmartRetryMiddleware());

Supported middleware:

Logging Middleware

Authentication Middleware

Retry Middleware

Custom Middleware

🔹 Smart Retry Middleware

Automatically retries transient network failures.

Retry status codes:

408
500
502
503
504

Retry strategy:

Attempt 1 → 500ms
Attempt 2 → 1000ms
Attempt 3 → 2000ms
🔹 Authentication Middleware

Automatically injects authentication headers.

Authorization: Bearer <token>

Supported token types:

Static API keys

JWT tokens

Runtime tokens

Persisted tokens

🔹 Token Provider

Token management abstraction.

public interface ITokenProvider
{
    string GetToken();
}

Example implementation:

public class StaticTokenProvider : ITokenProvider
{
    private readonly string token;

    public StaticTokenProvider(string token)
    {
        this.token = token;
    }

    public string GetToken() => token;
}
🔹 Endpoint Resolver

Provides flexible API endpoint configuration.

public interface IApiEndpointResolver
{
    string GetFullUrl(string endpointKey);
}

Example:

public class ProjectApiConfig : ScriptableObject, IApiEndpointResolver
{
    public string BaseUrl;
    public string Login;

    public string GetFullUrl(string endpointKey)
    {
        return endpointKey switch
        {
            nameof(Login) => BaseUrl + Login,
            _ => string.Empty
        };
    }
}
📦 Package Information
Property	Value
Package Name	com.mayamystic.apiframework
Version	1.1.0
Minimum Unity Version	2021.3 LTS
Dependency	com.unity.nuget.newtonsoft-json
License	Proprietary – MayaMystic
📁 Package Structure
Runtime/
 ├── Core/
 │    ├── Network/
 │    ├── Middleware/
 │    ├── Interfaces/
 │    ├── Utilities/
 │    └── Base/

Samples~/
Documentation~/
📚 Documentation

Full documentation available here:

👉
https://harshpatel19011995.github.io/API-Framework-Plugin/

Includes:

Getting Started

Architecture Overview

Middleware System

Authentication System

Smart Retry System

API Reference

🧪 Samples

Example implementation included in:

Samples~/BasicUsage

Demonstrates:

Creating ApiManager

Registering middleware

Performing API requests

🗺 Roadmap
Version	Planned Features
v1.2	Token refresh middleware
v1.2	Environment configuration
v1.3	Request logging abstraction
v1.4	API metrics system
v2.0	Plugin transport architecture
📜 Changelog

View release history:

https://github.com/HarshPatel19011995/API-Framework-Plugin/blob/main/CHANGELOG.md

📄 License

Proprietary – MayaMystic
All rights reserved.

👤 Author

Harsh Patel
MayaMystic

GitHub
https://github.com/HarshPatel19011995

⭐ Contributing

Currently maintained internally.
External contributions may be accepted in future releases.