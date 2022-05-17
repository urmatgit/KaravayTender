 <img align="left" width="116" height="116" src="https://raw.githubusercontent.com/jasontaylordev/CleanArchitecture/main/.github/icon.png" />
 
 # Clean Architecture Solution For Razor Pages
![.NET Core](https://github.com/jasontaylordev/CleanArchitecture/workflows/.NET%20Core/badge.svg) [![Clean.Architecture.Solution.Template NuGet Package](https://img.shields.io/badge/nuget-1.1.1-blue)](https://www.nuget.org/packages/Clean.Architecture.Solution.Template) [![NuGet](https://img.shields.io/nuget/dt/Clean.Architecture.Solution.Template.svg)](https://www.nuget.org/packages/Clean.Architecture.Solution.Template) [![Twitter adssdFollow](https://img.shields.io/twitter/follow/jasontaylordev.svg?style=social&label=Follow)](https://twitter.com/new163cn)

<br/>

This is a solution template for creating a Razor Page App with ASP.NET Core following the principles of Clean Architecture. With this architecture you can build Razor Page-based web applications efficiently and quickly. 

## Learn about Clean Architecture

[![Clean Architecture with ASP.NET Core 3.0 • Jason Taylor • GOTO 2019](https://img.youtube.com/vi/dK4Yb6-LxAk/0.jpg)](https://www.youtube.com/watch?v=dK4Yb6-LxAk)

## Demonstration [http://razor.i247365.net/](http://razor.i247365.net/)
* default admin: administrator/123456
* default user: demo/123456

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

[![Clean Architecture Solution For Razor Page Development](https://github.com/neozhu/RazorPageCleanArchitecture/blob/main/doc/screenshot/2021-08-11_19-28-59.png?raw=true)](https://www.youtube.com/watch?v=NcMd5W3C63A)

## Technologies

* [ASP.NET Core 5](https://devblogs.microsoft.com/aspnet/announcing-asp-net-core-in-net-5/)
* [Entity Framework Core 5](https://docs.microsoft.com/en-us/ef/core/)
* [SmartAdmin - Responsive WebApp](https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0/)
* [Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-5.0&tabs=visual-studio)
* [Jquery EasyUI](https://www.jeasyui.com/)
* [MediatR](https://github.com/jbogard/MediatR)
* [LazyCache](https://github.com/alastairtree/LazyCache)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)
* [Docker](https://www.docker.com/)

## Code Generator tools
Generate the necessary application code efficiently.
[![How to](https://github.com/neozhu/CleanArchitectureCodeGenerator/raw/main/art/nuget.png)](https://www.youtube.com/watch?v=Hp6cjdfgMT8)

Github Sourcecode: https://github.com/neozhu/CleanArchitectureCodeGenerator
## Getting Started

The easiest way to get started is to install the [NuGet package](https://www.nuget.org/packages/Clean.Architecture.Solution.Template) and run `dotnet new ca-sln`:

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Run `dotnet new --install Clean.Architecture.Solution.Template` to install the project template
3. Create a folder for your solution and cd into it (the template will use it as project name)
4. Run `dotnet new ca-sln` to create a new project
5. Navigate to `src/WebUI/ClientApp` and run `npm install`
6. Navigate to `src/WebUI/ClientApp` and run `npm start` to launch the front end (Angular)
7. Navigate to `src/WebUI` and run `dotnet run` to launch the back end (ASP.NET Core Web API)

Check out my [blog post](https://jasontaylor.dev/clean-architecture-getting-started/) for more information.

### Docker Configuration

In order to get Docker working, you will need to add a temporary SSL cert and mount a volume to hold that cert.
You can find [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1) that describe the steps required for Windows, macOS, and Linux.

For Windows:
The following will need to be executed from your terminal to create a cert
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

NOTE: When using PowerShell, replace %USERPROFILE% with $env:USERPROFILE.

FOR macOS:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

FOR Linux:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`

In order to build and run the docker containers, execute `docker-compose -f 'docker-compose.yml' up --build` from the root of the solution where you find the docker-compose.yml file.  You can also use "Docker Compose" from Visual Studio for Debugging purposes.
Then open http://localhost:5000 on your browser.

To disable Docker in Visual Studio, right-click on the **docker-compose** file in the **Solution Explorer** and select **Unload Project**.

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **WebUI/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/WebUI`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### [SmartAdmin - Responsive WebApp](https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0)
## <img width="80" src="https://github.com/neozhu/RazorPageCleanArchitecture/blob/main/doc/screenshot/7375350.png?raw=true" /> [Purchases](https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0)
– an advanced UI Bootstrap 4 Admin and Dashboard – is built for the next generation. Its’ exceptional design contains vast collection of assorted reusable UI components integrated with latest jQuery plugins optimized to suit every modern web application project worldwide.

## Support

If you are having problems, please let us know by [raising a new issue](https://github.com/jasontaylordev/CleanArchitecture/issues/new/choose).

## License

This project is licensed with the [MIT license](LICENSE).
