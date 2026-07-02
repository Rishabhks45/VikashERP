# VikashERP.Mobile — Module Development Guide

Ye guide batati hai ki naya module (jaise "Sales", "Labour", "Purchases") banate waqt kaunsi files kis folder mein banani hain. Har naye module ke liye is checklist ko follow karo — consistency bani rahegi.

---

## Folder structure (reference)

```
VikashERP.Mobile/
├── Components/
│   ├── Layout/              → MainLayout, NavMenu, BottomNav (already done)
│   └── Shared/               → Reusable UI pieces (cards, badges, loaders)
├── Pages/
│   └── {ModuleName}/          → Route-able .razor pages for this module
├── Services/
│   ├── Interfaces/            → I{Module}Service.cs
│   └── {Module}Service.cs
├── Models/
│   └── {Module}Dto.cs / {Module}Models.cs
├── State/                     → Shared/cached state for a module (optional)
└── MauiProgram.cs             → Register new service here
```

---

## Checklist — naya module banate waqt

Example: maan lo module ka naam **"Labour"** hai.

### 1. Model (`Models/LabourModels.cs`)
- API se jo data aayega uske DTOs yahan define karo.
- Naming: `LabourDto`, `MarkAttendanceRequest`, `LabourAttendanceResponse` etc.
- Agar API project ke saath DTOs shared hain (`VikashERP.Shared`), to naye se mat banao — wahi reference karo.

```
Models/
└── LabourModels.cs
```

### 2. Service interface (`Services/Interfaces/ILabourService.cs`)
- Is module ke liye kaunsi API calls honi hain, unke method signatures yahan.

```csharp
public interface ILabourService
{
    Task<List<LabourDto>> GetAllAsync();
    Task<bool> MarkAttendanceAsync(int labourId, bool present);
}
```

### 3. Service implementation (`Services/LabourService.cs`)
- `ApiClient` use karke actual HTTP calls yahan likhein.
- Errors ko try-catch karke user-friendly message throw karo (UI mein snackbar dikhane ke liye).

```
Services/
├── Interfaces/
│   └── ILabourService.cs
└── LabourService.cs
```

### 4. Register service in `MauiProgram.cs`
- Har naye service ko yahan DI container mein register karna **mat bhoolna** — warna `@inject` fail hoga.

```csharp
builder.Services.AddScoped<ILabourService, LabourService>();
```

### 5. Pages (`Pages/Labour/`)
- Module ke saare screens is subfolder mein.
- Common pattern: List page + Detail/Create page.

```
Pages/
└── Labour/
    ├── LabourList.razor          (@page "/labour")
    ├── LabourAttendance.razor    (@page "/labour-attendance")
    └── LabourDetail.razor        (@page "/labour/{id:int}")
```

- Har page ke top pe:
```razor
@page "/labour-attendance"
@attribute [Authorize(Roles = "Manager,Admin,SuperAdmin")]
@inject ILabourService LabourService
```

### 6. Shared UI components (`Components/Shared/`)
- Agar module ke andar koi UI piece repeat ho raha hai (jaise ek "worker row" card), use alag component banao — copy-paste mat karo.

```
Components/
└── Shared/
    └── LabourRowCard.razor
```

### 7. Navigation entries
- **Sidebar:** `Components/Layout/NavMenu.razor` mein naya `MudNavLink` add karo, sahi `AuthorizeView Roles` ke andar.
- **Bottom nav:** Agar module bottom nav mein bhi chahiye (sirf 4-5 sabse zyada use hone wale modules), `Components/Layout/BottomNav.razor` mein add karo.

### 8. Loading & error states
- Har list/detail page mein 3 states handle karo:
  - Loading → `MudProgressCircular` ya skeleton
  - Empty → "No records found" jaisa message
  - Error → Snackbar ya inline error message (`ISnackbar` inject karke)

### 9. Role-based access
- Decide karo kaunse roles is module ko access kar sakte hain (aapke master doc ke hisaab se).
- Page-level: `@attribute [Authorize(Roles = "...")]`
- Menu-level: `<AuthorizeView Roles="...">`

---

## Quick reference — file checklist per module

| Step | File | Folder |
|---|---|---|
| 1 | `{Module}Models.cs` | `Models/` |
| 2 | `I{Module}Service.cs` | `Services/Interfaces/` |
| 3 | `{Module}Service.cs` | `Services/` |
| 4 | DI registration | `MauiProgram.cs` |
| 5 | `{Module}List.razor`, `{Module}Detail.razor` etc. | `Pages/{Module}/` |
| 6 | Reusable card/row components (if needed) | `Components/Shared/` |
| 7 | Sidebar link | `Components/Layout/NavMenu.razor` |
| 8 | Bottom nav link (if top-level module) | `Components/Layout/BottomNav.razor` |

---

## Naming conventions

- **Folders/Pages:** PascalCase, plural for lists (`Sales`, `Purchases`, `Labour`)
- **Routes:** kebab-case (`/labour-attendance`, `/quick-payments`)
- **DTOs:** suffix with `Dto` (`SalesInvoiceDto`) or `Request`/`Response` for API payloads
- **Services:** `I{Module}Service` / `{Module}Service`
- **Components:** PascalCase, descriptive (`LabourRowCard`, `StatCard`)

---

## Before marking a module "done"

- [ ] Model(s) created and match API response shape
- [ ] Service interface + implementation done, uses `ApiClient`
- [ ] Registered in `MauiProgram.cs`
- [ ] Page(s) created under `Pages/{Module}/`
- [ ] Role-based access added (page + nav)
- [ ] Loading / empty / error states handled
- [ ] Sidebar and/or bottom nav updated
- [ ] Tested on Android emulator (remember: use `10.0.2.2` not `localhost` for local API testing)
