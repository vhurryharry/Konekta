# Other Documentation

- [Actionstep JSON:API Conformance](./docs/jsonapi-serialiser.md).

## Konekta App (formerly WorkCloud Apps)

This WCA.Web solution should work both with Visual Studio 2019 (VS2019) as well as running via `dotnet run` on the command line.

### Dependencies

First, there are a set of prerequisites you will need whether or not you are
using Visual Studio. Make sure to install these prerequisites before proceeding.

- **[NodeJS](https://nodejs.org/)** = v12.12.0 This provides the platform on which the
  build tooling runs. This may be downloaded and installed from the NodeJS
  website.
- **NPM** = v6.11.3 This is installed with NodeJS
- **Microsoft SQL Server Express** or higher is needed to support full text indexes. Ensure you also have the full-text services installed to you instance. **LocalDB** does not support full text indexes and cannot be used.
  - Make sure you check the box for **Full Text Indexing**.
  - The database name is configured in WCA.Web/appsettings.json. You will need to create it.
  - You may have to run migrations manually before tables are created. `WCA.Web.exe migratedb` will do this, but there is a "chicken/egg" problem in that the build process for WCA.Web includes generating nswag/openapi definitions by invoking WCA.Web.exe, which will throw an exception if it cannot connect to the database... You can break the cycle by commenting out the NSwagOpenAPI and NSwagTypeScriptTypes steps in WCA.Web.csproj, building the project, and running migrations (and then uncommenting those steps again)
- **Dotnet Core SDK** (see [./global.json](global.json) for specific version) available at [https://dotnet.microsoft.com/download/dotnet-core/3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- **Visual Studio 2019**. Currently tested with VS2019 *v16.4.4*.
  - Or you can also install the npm windows-build-tools package:
   `npm install --global windows-build-tools`
- **[Azure Functions v3](https://docs.microsoft.com/en-au/azure/azure-functions/functions-run-local)** (tested with v3.0.2106 of the CLI).

### Dev HTTPS Certificates

This project is designed to use HTTPS even for development. This is easy to set up with the new `dotnet-dev-certs` tooling. To trust the development certificate, run the following commands:

```shell
dotnet tool install -g dotnet-dev-certs
dotnet dev-certs https --trust
```

After you've run the above commands to install the tooling and trust the certificates, the project should run as normal, including redirect from HTTP to HTTPS.

### Running the App in Visual Studio 2019

1. Open the solution file `WCA.sln`.
2. Check your startup projects. Run `WCA.Web` for the frontend/backend app. Run `WCA.AzureFunctions` for functions..
3. Press `F5` or click the `Run` button in Visual Studio. Visual Studio will
   launch your browser for you and navigate to the correct URL.

### Running The App without Visual Studio

1. Restore packages

   ```shell
   dotnet restore
   cd src\WCA.Web
   dotnet run
   ```

2. Browse to [https://localhost:5001/wca](https://localhost:5001/wca) to see the app. You can make changes in the code found under `src` and the browser should auto-refresh itself as you save files.

#### Access the Azure KeyVault from local environment

1. Please make sure that the Azure CLI is installed. You can find more information [here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest).

2. Login to Azure with Azure CLI using `az login` command.

3. You may also need to log in to Azure using the "Cloud Explorer" pane in Visual Studio.

### Running the web app with Azure Functions

1. Install required packages - Ensure you have Node.js installed

   ``` shell
   npm install -g azure-functions-core-tools@3
   ```

2. Open the solution file `WCA.sln`
3. Set the debug profile of `WCA.AzureFunctions` to `FunctionsCLI`
4. Configure solution to run the following in multiple start-up projects: `WCA.AzureFunctions` and `WCA.Web`
5. Debug the solution by hitting `F5` or clicking `Start` button

### Adding a new Azure Function

#### Disable in staging slot

When adding a new Azure Function, you must include the Disable attribute as follows. This is required to ensure that all functions are correctly disabled in the staging slot. The way this works is that the [constructor parameter refers to a Boolean app setting](https://docs.microsoft.com/en-us/azure/azure-functions/disable-function#other-methods). We use the value `ALL_JOBS_DISABLED` which is set to `false` in the `staging` slot.
``` csharp
[Disable("ALL_JOBS_DISABLED")]
```

#### Naming

Please always use `nameof()` to specify the name of functions. This avoids mismatches during refactoring.

``` csharp
[FunctionName(nameof(EmailToSMSTimerJob))]
```

### Entity Framework Migrations

Migrations and other database related code should be in the `WCA.Data` project.
To add migrations, you must use the `--startup-project` parameter to specify
the Web project. This is required so that Entity Framework can find the
migrations assembly and related DbContexts.
****
For example, the initial add for IdentityServer4 DbContexts:

```shell
cd src\WCA.Data
dotnet ef migrations add MyMigration --startup-project ..\WCA.Web --context WCADbContext
```

### Production Build / Bundling

There is a build configured on Azure Pipelines that uses the [azure-pipelines.yaml](./azure-pipelines.yaml) configuration. Refer to this build definition for details on what is required to build for production.

## Security / Authorisation and Feature Flags

Currently there is only minimal authorisation set up in the application. For the most part functionality is limited to authenticated users with no additional authorisation. There are a few exceptions however, which are listed below.

### Initialise Security

There are some security roles configured using ASPNet Identity Roles.

To use the configured security roles, you must initialise them. This can be done by running the web project with some parameters.

#### Initialise / create roles

```cmd
WCA.Web.exe initsecurity
```

You can also list users, list roles, add, and remove users from roles with similar commands. For a list of commands see [WCA.Web/Program.cs](./src/WCA.Web/Program.cs) or type:

```cmd
WCA.Web.exe -h
```

### Access to Roles in front-end code

Roles for the currently logged on user are made available via the [/api/Account/CurrentUser](src\WCA.Web\Controllers\api\AccountController.cs) endpoint.

In the Aurelia SPA, these roles are cached by the [account-service.ts](src\WCA.Web\ClientApp\services\account-service.ts) service. You can then use the `isUserInRole(role: string)` method on accountService to check if a user is in a given role.

Use the `Roles` class which contains string constants for currently known roles.

For example, to set `showAllPages` depending on whether a user is in the GlobalAdministrator role, you could use something like the following:

```TypeScript
import { autoinject } from 'aurelia-framework';
import { AccountService, Roles } from '../services/account-service';

@autoinject
export class MyPage {
    public showAllPages: boolean = false;

    constructor(private accountService: AccountService) {
    }

    public created(owningView: View, myView: View) {
        this.accountService.updateLoggedInStatusFromServer();
        this.showAllPages = this.accountService.isUserInRole(Roles.GlobalAdministrator);
    }
}
```

### Feature flags

For simple toggling of new functionality we can use the following roles which are defined in [SecurityRoles.cs](src\WCA.Core\Security\SecurityRoles.cs):

1. AlphaTester
2. BetaTester

## Front-end

There are two SPAs:
- **ClientApp** original SPA, hosted at `/wca/`. Primarily an Aurelia app, with some React components.
- **client-app** new SPA based on `create-react-app` with TypeScript, hosted at `/`. All new content must go here. We will eventually migrate functionality from the old SPA to this one, and then remove `ClientApp`.

## Testing

For an overview, please first read [Shift Left to Make Testing Fast and Reliable](https://docs.microsoft.com/en-us/azure/devops/learn/devops-at-microsoft/shift-left-make-testing-fast-reliable).

The intention is for the test projects to be used as follows:

| Project | Purpose / Type of Tests  |
| -- | -- |
| Client test projects (e.g. WCA.Actionstep.Client.Tests) | *Unit* tests for the specific project that is referenced.|
| WCA.UnitTests | *Unit Tests*, equivalent of **L0** tests as described in the article above. All test **must** be in-memory only\*, without dependencies on either SQL or the File System. Tests in this project should make use of Fakes, Mocks, or Stubs to test core functionality.<br /><br />From the article: *Broad class of fast in-memory unit tests. An L0 test is a unit test to most people. That is a test that depends on code in the assembly under test and nothing else.*<br /><br />*\* Currently these tests do use external dependencies, these must either be refactored or moved to the WCA.IntegrationTests project.*|
| WCA.IntegrationTets | Equivalent of **L1** tests as described in the article above. "Integration" refers to integrating project components, such as full service dependencies. In this project it is suitable to use **WebApplicationFactory**. Refer to [Integration tests in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0) for more details. May use a real SQL instance, but doesn't require an app deployment |
| Cypress e2e | End-to-end tests, equivalent to **L2** tests. In our case these will really be **L2** as these tests currently run against our "test" environment (https://app-test.konekta.com.au/) which connects to equivalent "test" / "staging" 3rd party services.<br /><br />Reminder: Cypress tests can also talk straight to the WCA.Web API, so tests don't all need to use the UI. |
| Cypress smoke tests (not yet implemented) | A limited set of end-to-end tests designed to run against the real production deployment. Equivalent to **L3** tests. This should contain few tests to verify that a deployment has run successfully. Some lightweight tests to ensure all services have been configured correctly and are available. E.g. can a user sign-in, do pages load.|


We use xUnit as the unit test framework for .NET. Where practical we try to follow the ASP.NET / .NET guidelines. Please read the following:
- [.NET Core Unit Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)


### Running End-to-end (e2e) Cypress Tests

This project uses Cypress as the e2e testing engine in the release pipeline. These tests run against the test system at https://app-test.konekta.com.au.

To run the cypress test locally, follow these steps.

1. Open src/WCA.Web/appsettings.json file and add this.
  ```
  "TestUserCredential": {
    "Username": "dev@workcloudapplications.com",
    "Password": "3AmD$1*gh%8XDS3yu%gIZEo#9oYri%4le8R^zOixhk!5f6PrZ4LGYH@Je7N%EIg%G7GgCPdyybxn020&mZJl!zpiIcQWHLyRue5w"
  }
  ```

2. Open src/WCA.Web/cypress.json file and replace the content with this.
  ```
  {
    "integrationFolder": "./cypress/integration",
    "baseUrl": "http://appwca-test.azurewebsites.net",
    "env": {
        "urlEnv": "staging"
    },
    "chromeWebSecurity": false,
    "pageLoadTimeout": 100000
  }
  ```

3. Run "npm test" in the src/WCA.Web directory. It will launch the cypress UI.
  To run the cypress test with the headless browser, then run "npx cypress run".