# VikashERP.Web Project Modules, APIs, and DTOs

This document outlines the various modules present in the `VikashERP.Web` frontend project, along with the API endpoints they consume and the Data Transfer Objects (DTOs) used for communication with the backend.

## 1. Sales Module
- **Service**: `SalesWebService`
- **Models File**: `SalesModels.cs`
- **API Endpoints**:
  - `GET /api/sales` - Get all invoices
  - `GET /api/sales/{id}` - Get invoice details by ID
  - `POST /api/sales` - Create a new invoice
  - `PUT /api/sales/{id}` - Update an invoice
  - `POST /api/sales/{id}/approve` - Approve an invoice
  - `PUT /api/sales/{id}/payment` - Update payment details
- **Primary DTOs**: `InvoiceListDto`, `InvoiceDetailDto`, `CreateInvoiceModel`

## 2. Purchases Module
- **Service**: `PurchaseWebService`
- **Models File**: `PurchaseModels.cs`
- **API Endpoints**:
  - `GET /api/purchases` - Get all purchase orders
  - `GET /api/purchases/{id}` - Get purchase details
  - `POST /api/purchases` - Create a purchase order
  - `PUT /api/purchases/{id}` - Update a purchase
  - `POST /api/purchases/{id}/approve` - Approve a purchase
- **Primary DTOs**: `PurchaseListDto`, `PurchaseDetailDto`, `CreatePurchaseModel`

## 3. Products & Inventory Module
- **Service**: `ProductWebService`, `InventoryWebService`
- **Models File**: `ProductModels.cs`, `InventoryModels.cs`
- **API Endpoints**:
  - `GET /api/products` - List products
  - `GET /api/products/{id}` - Product details
  - `POST /api/products` - Create product
  - `GET /api/inventory` - View inventory stock
- **Primary DTOs**: `ProductListDto`, `ProductDetailDto`, `CreateProductModel`, `InventoryItemDto`

## 4. Customers & Customer Shops Module
- **Service**: `CustomerWebService`, `CustomerShopService`
- **Models File**: `CustomerListDto.cs`, `CustomerShopModels.cs`
- **API Endpoints**:
  - `GET /api/customers` - List customers
  - `GET /api/customers/{id}` - Customer details
  - `GET /api/customershops` - List customer shops/sites
- **Primary DTOs**: `CustomerListDto`, `CustomerDetailDto`, `CustomerShopDto`

## 5. Suppliers Module
- **Service**: `SupplierWebService`
- **Models File**: `SupplierModels.cs`
- **API Endpoints**:
  - `GET /api/suppliers` - List suppliers
  - `POST /api/suppliers` - Create supplier
  - `GET /api/suppliers/{id}` - Get supplier details
- **Primary DTOs**: `SupplierListDto`, `SupplierDetailDto`, `CreateSupplierModel`

## 6. Authentication & User Profile Module
- **Service**: `AuthService`, `UserProfileService`
- **Models File**: `UserModels.cs`
- **API Endpoints**:
  - `POST /api/auth/login` - Authenticate user
  - `POST /api/auth/refresh` - Refresh JWT token
  - `GET /api/users/profile` - Get current user profile
- **Primary DTOs**: `LoginRequest`, `LoginResponse`, `UserProfileDto`

## 7. Expenses Module
- **Service**: `ExpenseWebService`
- **Models File**: `ExpenseModels.cs`
- **API Endpoints**:
  - `GET /api/expenses` - List expenses
  - `POST /api/expenses` - Record an expense
- **Primary DTOs**: `ExpenseListDto`, `CreateExpenseModel`

## 8. Brokers Module
- **Service**: `BrokerWebService`
- **Models File**: `Brokers/*`
- **API Endpoints**:
  - `GET /api/brokers` - List brokers
  - `POST /api/brokers` - Create a broker profile
- **Primary DTOs**: `BrokerListDto`, `BrokerDetailDto`

## 9. Dashboard Module
- **Models File**: `DashboardModels.cs`
- **API Endpoints**:
  - `GET /api/dashboard/summary` - Get summary statistics
- **Primary DTOs**: `DashboardSummaryDto`

## 10. System Configuration (Holidays, Timezones, Branding)
- **Services**: `HolidayWebService`, `TimezoneService`, `OrganizationBrandingService`
- **Models Files**: `HolidayModels.cs`, `TimezoneListItemDto.cs`, `OrganizationPublicModel.cs`
- **API Endpoints**:
  - `GET /api/holidays` - Get system holidays
  - `GET /api/timezones` - Get available timezones
  - `GET /api/organization/branding` - Get organization theme/branding
- **Primary DTOs**: `HolidayDto`, `TimezoneListItemDto`, `OrganizationPublicModel`

> [!TIP]
> **Architecture Note**: The `VikashERP.Web` project accesses these endpoints using `IHttpClientFactory` inside the respective WebService classes, passing JWT tokens obtained from the `AuthService` via custom `AuthenticationStateProvider` handlers.
