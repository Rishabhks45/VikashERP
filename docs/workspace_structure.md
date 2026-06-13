# Workspace Structure

This document records the intended arrangement of the active workspace.

## Root

| Path | Purpose |
| --- | --- |
| `VikashERP.sln` | Active solution file. |
| `backend/` | API, application, domain, infrastructure, shared services. |
| `VikashERP.Web/` | Blazor Server web application. |
| `mobile-app/` | MAUI mobile application. |
| `database/` | Full schema blueprint and incremental database scripts. |
| `docs/` | Documentation and diagrams. |
| `existing project/` | Local reference copy only. |

## Web Project

| Path | Purpose |
| --- | --- |
| `Components/Pages/` | Routeable pages. |
| `Components/Pages/Auth/` | Authentication pages. |
| `Components/Pages/Admin/` | Admin pages. |
| `Components/Layout/` | Main layout, auth layout, nav, reconnect modal. |
| `Services/` | UI/API client services. |
| `Services/Interfaces/` | Web service abstractions. |
| `wwwroot/` | Static assets, CSS, JS helpers. |

## Backend Project

| Path | Purpose |
| --- | --- |
| `VikashERP.API` | API host and controllers. |
| `VikashERP.Application` | Use cases, DTOs, interfaces, MediatR handlers. |
| `VikashERP.Domain` | Domain entities and core contracts. |
| `VikashERP.Infrastructure` | EF Core context, repositories, infrastructure services. |
| `VikashERP.SharedKernel` | Shared services, settings, enums, cross-cutting interfaces. |

## Cleanup Rules

- Keep generated files out of source control: `bin/`, `obj/`, `.vs/`.
- Keep local reference code out of normal commits unless actively migrating it.
- Do not store production secrets in committed `appsettings*.json`.
- Prefer adding new routeable Blazor screens under `Components/Pages/{Area}`.
