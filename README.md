MayaMystic API Framework


Enterprise-grade middleware-based API networking framework for Unity.

MayaMystic API Framework provides a clean, scalable architecture for building API systems in Unity applications. It introduces a middleware pipeline similar to modern backend frameworks, allowing developers to implement authentication, retries, logging, and request handling in a modular way.

🚀 Quick Start
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

4️⃣ Execute Request
await handler.ExecuteAsync();
✨ Features
Feature	Description
Async ApiManager	Centralized API request execution
Middleware Pipeline	Flexible request processing architecture
Authentication Middleware	Automatic Bearer token injection
Smart Retry Middleware	Exponential backoff retry system
Token Provider	Abstract token management
Endpoint Resolver	Centralized endpoint configuration
JSON Serialization	Powered by Newtonsoft JSON

🧠 Architecture

The framework uses a middleware pipeline architecture inspired by modern backend frameworks.

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

This allows flexible request processing and easy extension through custom middleware.

⚙ Core Components

ApiManager

The central component responsible for executing API requests.

Features:

Async request handling

Middleware pipeline execution

Request timeout management

Cancellation support

Example:

var apiManager = new ApiManager();
Middleware System

Middleware allows request processing before and after HTTP execution.

Example registration:

apiManager.UseMiddleware(new LoggingMiddleware());
apiManager.UseMiddleware(new AuthMiddleware(tokenProvider));
apiManager.UseMiddleware(new SmartRetryMiddleware());

Middleware order defines execution order.

Smart Retry Middleware

Handles transient network failures automatically.

Retry status codes:

408
500
502
503
504

Example retry timing:

Attempt 1 → 500ms
Attempt 2 → 1000ms
Attempt 3 → 2000ms
Authentication Middleware

Automatically injects authentication headers.

Authorization: Bearer <token>

Supports:

Static API keys

JWT tokens

Runtime tokens

Persisted tokens

Token Provider System

Token handling is abstracted via an interface.

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
Endpoint Resolver

Projects control endpoint resolution using an abstraction.

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

Full documentation:

https://harshpatel19011995.github.io/API-Framework-Plugin/

Documentation includes:

Getting Started

Architecture overview

Middleware pipeline

Authentication system

Smart retry system

API reference

🧪 Samples

Example usage can be found in:

Samples~/BasicUsage

The sample demonstrates:

Creating ApiManager

Registering middleware

Performing API requests

🧭 Design Principles

The framework follows modern SDK architecture principles:

Clean separation of infrastructure and business logic

Middleware-driven request lifecycle

Dependency inversion

Modular reusable components

Production-ready scalability

Architecture inspiration:

ASP.NET Core middleware pipeline

Unreal Engine subsystem design

Modern SDK architecture patterns

📜 Changelog

View release history:

https://github.com/HarshPatel19011995/API-Framework-Plugin/blob/main/CHANGELOG.md

🗺 Roadmap

Planned future improvements:

Version	Planned Features
v1.2	Token refresh middleware
v1.2	Environment configuration system
v1.3	Request logging abstraction
v1.4	API metrics & monitoring
v2.0	Plugin transport layer support

📄 License
Proprietary – MayaMystic
All rights reserved.

👤 Author
Harsh Patel
MayaMystic

GitHub
https://github.com/HarshPatel19011995