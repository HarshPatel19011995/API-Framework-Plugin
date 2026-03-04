MayaMystic API Framework

Reusable, enterprise-grade API networking framework for Unity projects.

Version: 1.1.0
Minimum Unity: 2021.3 LTS

----------------------------------------

CORE FEATURES

• Async ApiManager (HttpClient based)
• Middleware Pipeline Architecture
• Logging Middleware
• Authentication Middleware
• Smart Retry Middleware (Exponential Backoff)
• Token Provider Abstraction
• Endpoint Resolver System
• Newtonsoft JSON Serialization

----------------------------------------

ARCHITECTURE FLOW

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

----------------------------------------

PACKAGE STRUCTURE

Runtime/
 ├── Core/
 │    ├── Network/
 │    ├── Middleware/
 │    ├── Interfaces/
 │    ├── Utilities/
 │    └── Base/
Samples~/

----------------------------------------

Author: Harsh Patel
License: Proprietary – MayaMystic