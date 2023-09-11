# netcore-boilerplate

Boilerplate of API in ~~`.NET Core 3.1`~~ `.NET 7`

| GitHub | Codecov |
|:-------------:|:-------------:|
| [![Build & Test](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/dotnetcore.yml) [![Build docker image](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/docker.yml/badge.svg)](https://github.com/lkurzyniec/netcore-boilerplate/actions/workflows/docker.yml) | [![codecov](https://codecov.io/gh/lkurzyniec/netcore-boilerplate/branch/master/graph/badge.svg)](https://codecov.io/gh/lkurzyniec/netcore-boilerplate) |

Boilerplate is a piece of code that helps you to quickly kick-off a project or start writing your source code. It is kind of a template - instead
of starting an empty project and adding the same snippets each time, you can use the boilerplate that already contains such code.

## Source code contains

1. [Central Package Management (CPM)](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
1. ~~[Autofac]~~(https://autofac.org/) (Removed in [PR19](https://github.com/lkurzyniec/netcore-boilerplate/pull/19))
1. [Swagger](https://swagger.io/) + [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle)
1. [FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet) (Feature Flags, Feature Toggles)
1. [HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)
1. [EF Core](https://docs.microsoft.com/ef/)
    * [MySQL provider from Pomelo Foundation](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)
    * [MsSQL from Microsoft](https://github.com/aspnet/EntityFrameworkCore/)
1. [Dapper](https://github.com/DapperLib/Dapper)
    * [Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/)
1. Tests
    * Integration tests with InMemory database
        * [FluentAssertions]
        * [xUnit]
        * [Verify](https://github.com/VerifyTests/Verify/)
        * [Verify.Http](https://github.com/VerifyTests/Verify.Http)
        * TestServer
    * Unit tests
        * [AutoFixture](https://github.com/AutoFixture/AutoFixture)
        * [FluentAssertions]
        * [Moq](https://github.com/moq/moq4)
        * [Moq.AutoMock](https://github.com/moq/Moq.AutoMocker)
        * [xUnit]
    * Architectural tests (conventional tests)
        * [NetArchTest](https://github.com/BenMorris/NetArchTest)
        * [xUnit]
    * ~~Load tests~~ (Removed in [PR135](https://github.com/lkurzyniec/netcore-boilerplate/pull/135))
        * ~~[NBomber]~~(https://nbomber.com/)
1. Code quality
    * [EditorConfig](https://editorconfig.org/) ([.editorconfig](.editorconfig))
    * Analyzers
        * [Microsoft.CodeAnalysis.Analyzers](https://github.com/dotnet/roslyn-analyzers)
        * [Microsoft.AspNetCore.Mvc.Api.Analyzers](https://github.com/aspnet/AspNetCore/tree/master/src/Analyzers)
        * [Microsoft.VisualStudio.Threading.Analyzers](https://github.com/microsoft/vs-threading)
    * CodeAnalysisRules [LittleFish.ruleset](LittleFish.ruleset)
    * Code coverage
        * [Coverlet](https://github.com/tonerdo/coverlet)
        * [Codecov](https://codecov.io/)
    * CI Code analysis with [CodeQL](https://codeql.github.com/)
1. Docker
    * [Dockerfile](dockerfile)
    * [Docker-compose](docker-compose.yml)
        * `mysql:8` with DB initialization
        * `mcr.microsoft.com/mssql/server:2017-latest` with DB initialization
        * `netcore-boilerplate:local`
1. [Serilog](https://serilog.net/)
    * Sink: [Async](https://github.com/serilog/serilog-sinks-async)
1. [DbUp](http://dbup.github.io/) as a db migration tool
1. Continuous integration
    * ~~[Travis CI]~~(https://travis-ci.org/) ([travisci.yml](https://github.com/lkurzyniec/netcore-boilerplate/blob/bf65154b63f6a10d6753045c49cd378e53907816/.travis.yml))
    * [GitHub Actions](https://github.com/features/actions)
        * [dotnetcore.yml](.github/workflows/dotnetcore.yml)
        * [codeql-analysis.yml](.github/workflows/codeql-analysis.yml)
        * [docker.yml](.github/workflows/docker.yml)

## Architecture

### Api

[LittleFish.Api](src/LittleFish.Api)

* The entry point of the app - [Program.cs](src/LittleFish.Api/Program.cs)
* Simple Startup class - [Startup.cs](src/LittleFish.Api/Startup.cs)
  * MvcCore
  * DbContext (with MySQL)
  * DbContext (with MsSQL)
  * Swagger and SwaggerUI (Swashbuckle)
  * HostedService and HttpClient
  * FeatureManagement
  * HealthChecks
    * MySQL
    * MsSQL
* Filters
  * Simple `ApiKey` Authorization filter - [ApiKeyAuthorizationFilter.cs](src/LittleFish.Api/Infrastructure/Filters/ApiKeyAuthorizationFilter.cs)
  * Action filter to validate `ModelState` - [ValidateModelStateFilter.cs](src/LittleFish.Api/Infrastructure/Filters/ValidateModelStateFilter.cs)
  * Global exception filter - [HttpGlobalExceptionFilter.cs](src/LittleFish.Api/Infrastructure/Filters/HttpGlobalExceptionFilter.cs)
* Configurations
  * `Serilog` configuration place - [SerilogConfigurator.cs](src/LittleFish.Api/Infrastructure/Configurations/SerilogConfigurator.cs)
  * `Swagger` registration place - [SwaggerRegistration.cs](src/LittleFish.Api/Infrastructure/Registrations/SwaggerRegistration.cs)
* Simple exemplary API controllers - [EmployeesController.cs](src/LittleFish.Api/Controllers/EmployeesController.cs), [CarsController.cs](src/LittleFish.Api/Controllers/CarsController.cs)
* Example of BackgroundService - [PingWebsiteBackgroundService.cs](src/LittleFish.Api/BackgroundServices/PingWebsiteBackgroundService.cs)

![LittleFish.Api](.assets/api.png "LittleFish.Api")

### Core

[LittleFish.Core](src/LittleFish.Core)

* Models
  * Dto models
  * DB models
  * AppSettings models - [Settings](src/LittleFish.Core/Settings)
* DbContexts
  * MySQL DbContext - [EmployeesContext.cs](src/LittleFish.Core/EmployeesContext.cs)
  * MsSQL DbContext - [CarsContext.cs](src/LittleFish.Core/CarsContext.cs)
* Core registrations - [CoreRegistrations.cs](src/LittleFish.Core/Registrations/CoreRegistrations.cs)
* Exemplary MySQL repository - [EmployeeRepository.cs](src/LittleFish.Core/Repositories/EmployeeRepository.cs)
* Exemplary MsSQL service - [CarService.cs](src/LittleFish.Core/Services/CarService.cs)

![LittleFish.Core](.assets/core.png "LittleFish.Core")

## DB Migrations

[LittleFish.Db](src/LittleFish.Db)

* Console application as a simple db migration tool - [Program.cs](src/LittleFish.Db/Program.cs)
* Sample migration scripts, both `.sql` and `.cs` - [S001_AddCarTypesTable.sql](src/LittleFish.Db/Scripts/Sql/S001_AddCarTypesTable.sql), [S002_ModifySomeRows.cs](src/LittleFish.Db/Scripts/Code/S002_ModifySomeRows.cs)

![LittleFish.Db](.assets/db.png "LittleFish.Db")

## Tests

### Integration tests

[LittleFish.Api.IntegrationTests](test/LittleFish.Api.IntegrationTests)

* Infrastructure
  * Fixture with TestServer - [TestServerClientFixture.cs](test/LittleFish.Api.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
  * TestStartup with InMemory databases - [TestStartup.cs](test/LittleFish.Api.IntegrationTests/Infrastructure/TestStartup.cs)
  * Simple data feeders - [EmployeeContextDataFeeder.cs](test/LittleFish.Api.IntegrationTests/Infrastructure/DataFeeders/EmployeeContextDataFeeder.cs), [CarsContextDataFeeder.cs](test/LittleFish.Api.IntegrationTests/Infrastructure/DataFeeders/CarsContextDataFeeder.cs)
* Exemplary tests - [EmployeesTests.cs](test/LittleFish.Api.IntegrationTests/EmployeesTests.cs), [CarsTests.cs](test/LittleFish.Api.IntegrationTests/CarsTests.cs)

![LittleFish.Api.IntegrationTests](.assets/itests.png "LittleFish.Api.IntegrationTests")

### Unit tests

[LittleFish.Api.UnitTests](test/LittleFish.Api.UnitTests)

* Exemplary tests - [EmployeesControllerTests.cs](test/LittleFish.Api.UnitTests/Controllers/EmployeesControllerTests.cs)
* Unit tests of `ApiKeyAuthorizationFilter.cs` - [ApiKeyAuthorizationFilterTests.cs](test/LittleFish.Api.UnitTests/Infrastructure/Filters/ApiKeyAuthorizationFilterTests.cs)

[LittleFish.Core.UnitTests](test/LittleFish.Core.UnitTests)

* Extension methods to mock `DbSet` faster - [EnumerableExtensions.cs](test/LittleFish.Core.UnitTests/Extensions/EnumerableExtensions.cs)
* Exemplary tests - [EmployeeRepositoryTests.cs](test/LittleFish.Core.UnitTests/Repositories/EmployeeRepositoryTests.cs), [CarServiceTests.cs](test/LittleFish.Core.UnitTests/Services/CarServiceTests.cs)

![LittleFish.Core.UnitTests](.assets/utests.png "LittleFish.Core.UnitTests")

### Architectural tests

[LittleFish.ArchitecturalTests](test/LittleFish.ArchitecturalTests)

* Exemplary tests - [ApiArchitecturalTests.cs](test/LittleFish.ArchitecturalTests/ApiArchitecturalTests.cs), [CoreArchitecturalTests.cs](test/LittleFish.ArchitecturalTests/CoreArchitecturalTests.cs)

![LittleFish.ArchitecturalTests](.assets/atests.png "LittleFish.ArchitecturalTests")

## Books module

Totally separate module, developed with a modular monolith approach.

### Module

The code organized around features (vertical slices).

[LittleFish.BooksModule](src/LittleFish.BooksModule)

* Features
  * Delete book - [Endpoint.cs](src/LittleFish.BooksModule/Features/DeleteBook/Endpoint.cs), [Command.cs](src/LittleFish.BooksModule/Features/DeleteBook/Command.cs)
  * Get book - [Endpoint.cs](src/LittleFish.BooksModule/Features/GetBook/Endpoint.cs), [Query.cs](src/LittleFish.BooksModule/Features/GetBook/Query.cs)
  * Get books - [Endpoint.cs](src/LittleFish.BooksModule/Features/GetBooks/Endpoint.cs), [Query.cs](src/LittleFish.BooksModule/Features/GetBooks/Query.cs)
  * Upsert book - [Endpoint.cs](src/LittleFish.BooksModule/Features/UpsertBook/Endpoint.cs), [Command.cs](src/LittleFish.BooksModule/Features/UpsertBook/Command.cs)
* Sqlite db initializer - [DbInitializer.cs](src/LittleFish.BooksModule/Infrastructure/DbInitializer.cs)
* Module configuration place - [BooksModuleConfigurations.cs](src/LittleFish.BooksModule/BooksModuleConfigurations.cs)

![LittleFish.BooksModule](.assets/books.png "LittleFish.BooksModule")

### Integration Tests

[LittleFish.BooksModule.IntegrationTests](test/LittleFish.BooksModule.IntegrationTests)

* Infrastructure
  * Fixture with TestServer - [TestServerClientFixture.cs](test/LittleFish.BooksModule.IntegrationTests/Infrastructure/TestServerClientFixture.cs)
  * Very simple data feeder - [BooksDataFeeder.cs](test/LittleFish.BooksModule.IntegrationTests/Infrastructure/DataFeeders/BooksDataFeeder.cs)
* Exemplary tests - [BooksTests.cs](test/LittleFish.BooksModule.IntegrationTests/BooksTests.cs)

![LittleFish.BooksModule.IntegrationTests](.assets/books-tests.png "LittleFish.BooksModule.IntegrationTests")

## How to adapt to your project

Generally it is totally up to you! But in case you do not have any plan, You can follow below simple steps:

1. Download/clone/fork repository :arrow_heading_down:
1. Remove components and/or classes that you do not need to :fire:
1. Rename files (e.g. sln, csproj, ruleset), folders, namespaces etc :memo:
1. Appreciate the work :star:

## Build the solution

Just execute `dotnet build` in the root directory, it takes `LittleFish.sln` and build everything.

## Start the application

### Standalone

At first, you need to have up and running [MySQL](https://www.mysql.com/downloads/) and [MsSQL](https://www.microsoft.com/sql-server/sql-server-downloads) database servers on localhost with initialized
database by [mysql script](db/mysql/mysql-employees.sql) and [mssql script](db/mssql/mssql-cars.sql).

Then the application (API) can be started by `dotnet run` command executed in the `src/LittleFish.Api` directory.
By default it will be available under http://localhost:5000/, but keep in mind that documentation is available under
http://localhost:5000/swagger/.

### Docker (recommended)

Just run `docker-compose up` command in the root directory and after successful start of services visit http://localhost:5000/swagger/.
To check that API has connection to both MySQL and MsSQL databases visit http://localhost:5000/health/.

> When running on `Linux` (i.e. [WSL](https://learn.microsoft.com/en-us/windows/wsl/install)), make sure that all docker files
([dockerfile](dockerfile), [docker-compose](docker-compose.yml) and all [mssql files](db/mssql)) have line endings `LF`.

### Migrations

When the entire environment is up and running, you can additionally run a migration tool to add some new schema objects into MsSQL DB. To do that, go to `src/LittleFish.Db` directory and execute `dotnet run` command.

## Run unit tests

Run `dotnet test` command in the root directory, it will look for test projects in `LittleFish.sln` and run them.

## To Do

* any idea? Please create an issue.

## Be like a star, give me a star! :star:

If:

* you like this repo/code,
* you learn something,
* you are using it in your project/application,

then please give me a `star`, appreciate my work. Thanks!

## Buy me a coffee! :coffee:

[![Buy me a coffee](https://cdn.buymeacoffee.com/buttons/lato-blue.png)](https://www.buymeacoffee.com/lkurzyniec)

[FluentAssertions]: https://fluentassertions.com/
[xUnit]: https://xunit.net/
# little-fish
