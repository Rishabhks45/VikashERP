# Vikash Iron & Steel ERP

Development workspace for the Vikash Iron & Steel ERP platform.

## Active Solution

- `VikashERP.sln` - Visual Studio solution for the active .NET projects.
- `backend/` - ASP.NET Core API and Clean Architecture backend layers.
- `VikashERP.Web/` - Blazor Server admin/front-office web app using MudBlazor.
- `mobile-app/` - .NET MAUI mobile application shell.
- `database/` - Database schema blueprint and incremental SQL scripts.
- `docs/` - Architecture notes, workspace structure, and diagrams.

## Backend Projects

- `backend/VikashERP.API` - HTTP API host, auth, controllers, Swagger.
- `backend/VikashERP.Application` - CQRS/application contracts and handlers.
- `backend/VikashERP.Domain` - Domain entities and core interfaces.
- `backend/VikashERP.Infrastructure` - EF Core, repositories, JWT provider.
- `backend/VikashERP.SharedKernel` - shared settings, services, enums, interfaces.
- `backend/PwdGen` - local helper utility.

## Web App Areas

- `VikashERP.Web/Components/Pages/Home.razor` - executive dashboard.
- `VikashERP.Web/Components/Pages/EmployeeDashboard.razor` - employee dashboard.
- `VikashERP.Web/Components/Pages/Profile.razor` - user profile/account center.
- `VikashERP.Web/Components/Pages/Auth/` - login, forgot password, reset password.
- `VikashERP.Web/Components/Pages/Admin/` - administrative pages.
- `VikashERP.Web/Components/Layout/` - application shell, navigation, auth layout.
- `VikashERP.Web/Services/` - web client services and UI support services.

## Reference Material

The `existing project/` folder is a local reference copy of the older VikashIronix implementation. It is intentionally not part of the active solution and should not be edited unless you are migrating a specific feature.

## Build

```powershell
dotnet build VikashERP.sln
```

If the Debug web app is running in Visual Studio, build Release to avoid locked output files:

```powershell
dotnet build VikashERP.Web\VikashERP.Web.csproj -c Release
```

## Notes

- Do not commit generated `bin/`, `obj/`, or `.vs/` folders.
- Move secrets out of `appsettings.json` before publishing or sharing the repository.
- Use `docs/architecture_documentation.md` for the implementation roadmap.
