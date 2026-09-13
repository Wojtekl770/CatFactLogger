# CatFactLogger

A small ASP.NET Core Web API that fetches random cat facts from [catfact.ninja](https://catfact.ninja/fact) and logs each one to a local text file.

Built as a technical assignment for the Netwise .NET internship program (Szkółka .NET).

## What it does

Every call to `POST /api/Fact`:
1. Sends a request to `https://catfact.ninja/fact`
2. Parses the returned cat fact
3. Appends it as a new line to a local `facts.txt` file (created automatically on first write)

## Tech stack

- .NET 10 / ASP.NET Core Web API (controllers)
- Swashbuckle (Swagger UI) for interactive API docs
- Dependency Injection via the built-in ASP.NET Core container
- `ILogger` for structured logging

## Architecture

```
Controller → Service → (API Client + Repository)
```

| Layer | Responsibility |
|---|---|
| `FactController` | Exposes `POST /api/Fact`, delegates to the service |
| `FactService` | Orchestrates the flow: calls the API client, then the repository |
| `CatFactApiClient` | Wraps `HttpClient`, talks to catfact.ninja |
| `FactRepository` | Appends facts to `facts.txt` (thread-safe via `SemaphoreSlim`) |

Every dependency is injected through an interface, which keeps each layer independently testable.

## Running locally

```bash
dotnet restore
dotnet run
```

Swagger UI opens automatically at `https://localhost:<port>/swagger`. Expand `POST /api/Fact` → **Try it out** → **Execute**.

A `facts.txt` file appears in the project's working directory after the first successful call:

```
2026-09-13 14:11:09 | Cats have been domesticated for half as long as dogs have been. | length=63
```
