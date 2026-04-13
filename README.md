# AutomationExercise Test Suite

Multi-layer test automation framework for https://automationexercise.com built with C# and .NET 8.

It covers:
- API testing
- UI browser testing (Playwright)
- End-to-end purchase flow testing
- Data-driven scenarios
- Negative and boundary cases

## Table of Contents

1. [What This Project Includes](#what-this-project-includes)
2. [Architecture Overview](#architecture-overview)
3. [Technology Stack](#technology-stack)
4. [Prerequisites](#prerequisites)
5. [Quick Start](#quick-start)
6. [Configuration and Environments](#configuration-and-environments)
7. [How to Run Tests](#how-to-run-tests)
8. [Reports and Artifacts](#reports-and-artifacts)
9. [CI Pipeline](#ci-pipeline)
10. [Extending the Framework](#extending-the-framework)
11. [Troubleshooting](#troubleshooting)
12. [Repository Notes](#repository-notes)

## What This Project Includes

The test suite is organized by test intent:

- API tests under `Tests/API`
- UI tests under `Tests/UI`
- E2E flow tests under `Tests/E2E`
- Data-driven tests under `Tests/DataDriven`
- Negative tests under `Tests/Negative`

Test implementation follows layered separation:

- Test classes orchestrate scenarios
- Page Objects and API Clients encapsulate interactions
- Infrastructure provides DI, browser lifecycle, logging, and retry behavior
- Builders and utilities generate data and common helpers
- Configuration is strongly-typed and environment-aware

## Architecture Overview

```text
src/AutomationExercise.Tests/
├── ApiClients/
│   ├── Base/
│   ├── Models/Responses/
│   ├── BrandApiClient.cs
│   ├── ProductApiClient.cs
│   └── UserApiClient.cs
├── Configuration/
│   ├── AppSettings.cs
│   ├── appsettings.json
│   └── appsettings.Staging.json
├── Infrastructure/
│   ├── Base/
│   ├── Browser/
│   ├── DI/
│   ├── Logging/
│   └── Retry/
├── PageObjects/
│   ├── Base/
│   ├── Components/
│   ├── CartPage.cs
│   ├── CheckoutPage.cs
│   ├── LoginPage.cs
│   ├── ProductsPage.cs
│   └── RegisterPage.cs
├── TestData/Builders/
├── Tests/
│   ├── API/
│   ├── DataDriven/
│   ├── E2E/
│   ├── Negative/
│   └── UI/
├── Utilities/
├── allureConfig.json
└── parallel.runsettings
```

## Technology Stack

| Concern | Tooling |
|---|---|
| Runtime | .NET 8 |
| Test Runner | NUnit |
| UI Automation | Microsoft Playwright |
| API Client | RestSharp |
| Assertions | FluentAssertions |
| Test Data | Bogus |
| Reporting | Allure.NUnit |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Configuration | Microsoft.Extensions.Configuration |
| Logging | Microsoft.Extensions.Logging |

## Prerequisites

Install the following before running tests locally:

- .NET 8 SDK
- PowerShell (Windows includes it by default; macOS/Linux should have `pwsh`)
- Allure CLI (optional but recommended for local report viewing)

Allure installation examples:

```bash
# macOS
brew install allure

# Windows (Scoop)
scoop install allure
```

## Quick Start

1. Restore dependencies:

```bash
dotnet restore
```

2. Build the solution:

```bash
dotnet build --configuration Release
```

3. Install Playwright browsers (required for UI/E2E):

Windows PowerShell:

```powershell
.\src\AutomationExercise.Tests\bin\Release\net8.0\playwright.ps1 install --with-deps chromium firefox
```

macOS/Linux:

```bash
pwsh src/AutomationExercise.Tests/bin/Release/net8.0/playwright.ps1 install --with-deps chromium firefox
```

4. Run all tests:

```bash
dotnet test
```

## Configuration and Environments

Configuration loading order:

1. `Configuration/appsettings.json`
2. `Configuration/appsettings.{ENVIRONMENT}.json` (optional)
3. Environment variables

Environment selection is controlled by `ENVIRONMENT`.

Examples:

Windows PowerShell:

```powershell
$env:ENVIRONMENT = "Staging"
dotnet test
```

macOS/Linux:

```bash
ENVIRONMENT=Staging dotnet test
```

### Key Settings

`App` section:
- `BaseUrl`
- `ApiBaseUrl`
- `DefaultTimeoutMs`
- `Environment`

`Browser` section:
- `Type` (`Chromium`, `Firefox`, `Webkit`)
- `Headless`
- `SlowMo`
- `ViewportWidth`, `ViewportHeight`
- `RecordVideo`
- `VideoDir`

`Api` section:
- `TimeoutSeconds`
- `MaxRetries`
- `RetryDelayMs`

`Credentials` section:
- `ValidEmail`
- `ValidPassword`

### Common Environment Variable Overrides

Because the project uses `AddEnvironmentVariables()`, nested settings can be overridden with double underscores:

```powershell
$env:Browser__Type = "Firefox"
$env:Browser__Headless = "false"
dotnet test --filter "TestCategory=UI"
```

## How to Run Tests

Run everything:

```bash
dotnet test
```

Run by category:

```bash
dotnet test --filter "TestCategory=API"
dotnet test --filter "TestCategory=UI"
dotnet test --filter "TestCategory=E2E"
dotnet test --filter "TestCategory=DataDriven"
dotnet test --filter "TestCategory=Negative"
```

Run a single test class:

```bash
dotnet test --filter "FullyQualifiedName~AuthenticationTests"
```

Run with runsettings (parallel workers + default ENVIRONMENT):

```bash
dotnet test --settings src/AutomationExercise.Tests/parallel.runsettings
```

Set worker count inline without runsettings:

```bash
dotnet test -- NUnit.NumberOfTestWorkers=4
```

## Reports and Artifacts

Allure result files are generated in:

- `allure-results`

Serve report locally:

```bash
allure serve allure-results
```

Additional artifacts:

- Failure screenshots are attached to Allure for UI/E2E failures
- Optional browser videos are written to `test-results/videos` when `Browser.RecordVideo=true`

## CI Pipeline

Pipeline file:

- `.github/workflows/test-pipeline.yml`

Triggers:

- Push to `main` and `develop`
- Pull requests to `main`
- Nightly schedule (`0 1 * * *`)
- Manual `workflow_dispatch`

Jobs:

1. `build`
2. `api-tests`
3. `ui-tests`
4. `e2e-tests`
5. `report`

Manual run inputs:

- `category`: `all`, `API`, `UI`, `E2E`
- `environment`: target configuration name (for example `Development`, `Staging`)

The `report` job downloads all Allure artifacts, generates HTML output, and deploys it to `gh-pages` when the branch is `main`.

## Extending the Framework

### Add a New Page Object

1. Create a class under `PageObjects/` inheriting `BasePage`.
2. Keep selectors private and expose intent-driven methods.
3. Override `IsLoadedAsync()` for page readiness checks.

### Add a New API Client

1. Create client class under `ApiClients/` inheriting `BaseApiClient`.
2. Add endpoint methods using existing execute/deserialize flow.
3. Register the client in `Infrastructure/DI/ServiceCollectionExtensions.cs`.

### Add a New Test Suite

1. Create test file under the appropriate `Tests/*` folder.
2. Inherit `BaseApiTest` or `BaseUiTest`.
3. Add `[Category("...")]` to support filtering in CLI and CI.
4. Add Allure metadata attributes for clear reporting.

### Add a New Environment

1. Add `Configuration/appsettings.<Name>.json`.
2. Set `ENVIRONMENT=<Name>` before running tests.

## Troubleshooting

### Playwright script not found

Build first (`dotnet build -c Release`), then run the generated `playwright.ps1` from `bin/Release/net8.0`.

### Browser does not switch in CI/local

Use config binding keys rather than custom variable names:

- `Browser__Type=Firefox` (works)
- `BROWSER_TYPE=Firefox` (not consumed by current config binding)

### Staging values not applied

Ensure `ENVIRONMENT` exactly matches the settings file suffix, for example `Staging` -> `appsettings.Staging.json`.

### Allure report command fails

Verify Allure CLI is installed and available in your PATH, then rerun `allure serve allure-results`.

## Repository Notes

- This repository currently does not include a top-level `LICENSE` file.
- Badge URLs in this README should be updated to your actual GitHub repository path if you publish this project.
