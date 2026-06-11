# Vikash Iron & Steel ERP - Backend API Architecture Reference

This is the architectural blueprint for the ASP.NET Core Web API backend using Clean Architecture guidelines.

## Project Structure Overview

```text
backend/
├── VikashERP.Domain/
│   ├── Entities/               -- Database model mapping (Customer, Invoice, StockLedger)
│   ├── Enums/                  -- System enums (TransactionType, DeliveryStatus, PaymentMode)
│   └── Interfaces/             -- Core abstraction layer (IRepository, IUnitOfWork)
├── VikashERP.Application/
│   ├── Behaviors/              -- Pipeline logging & validation middleware
│   ├── Dtos/                   -- Request/Response Data Transfer Objects
│   ├── Interfaces/             -- App-specific interfaces (ICalculationEngine, INotificationService)
│   ├── Services/               -- Domain-specific business services
│   └── Features/               -- MediatR CQRS Command & Query handlers
│       ├── Customers/          -- GetLedgerQuery, AdjustCreditCommand
│       ├── Invoices/           -- CreateInvoiceCommand, GetInvoicePdfQuery
│       └── Stock/              -- UpdateStockCommand, GetLowStockAlertsQuery
├── VikashERP.Infrastructure/
│   ├── Data/                   -- PostgreSQL EF Core DbContext, Migrations
│   ├── Repositories/           -- Repository concrete implementations
│   └── Services/               -- External integrations (WhatsApp Gateway, UPI QR generator)
└── VikashERP.API/
    ├── Controllers/            -- REST API Endpoints (SalesController, InventoryController)
    ├── Middleware/             -- Exception handling, JWT validation
    ├── Program.cs              -- Dependency injections & App configurations
    └── appsettings.json        -- Connection strings and environment settings
```

---

## Technical Architectural Patterns

### 1. CQRS Pattern (MediatR)
We segregate Read and Write operations to achieve high performance and maintainability.
- **Commands (Write)**: Trigger operations that modify data (e.g. `CreateInvoiceCommand`). These run atomic DB transactions encapsulating:
  - Validating credit limits.
  - Adding financial transactions to `customer_ledger`.
  - Deducting quantities/weights in `stock_ledger`.
- **Queries (Read)**: Fast, direct retrieval of data (e.g. `GetCustomerLedgerQuery`), utilizing EF Core `.AsNoTracking()` for rapid database fetch.

### 2. Transaction Integrity & Unit of Work
Ledger systems must not suffer from partial writes (e.g. invoice created but stock not decremented). To prevent this, the `UnitOfWork` guarantees that database commits run within a serializable database transaction:

```csharp
using var transaction = await _unitOfWork.BeginTransactionAsync();
try
{
    // 1. Insert Invoice
    await _invoiceRepository.AddAsync(invoice);

    // 2. Perform Stock Ledger Deductions
    foreach(var item in invoice.Items) {
        await _stockService.DeductStockAsync(item.VariantId, item.QtyPcs, item.WeightKg);
    }

    // 3. Update Customer Account Balance & Customer Ledger
    await _customerService.DebitLedgerAsync(invoice.CustomerId, invoice.TotalAmount);

    await _unitOfWork.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch (Exception)
{
    await transaction.RollbackAsync();
    throw;
}
```

---

## Calculations Engine Logic

### 1. Piece-to-Weight Conversions
Steel products are often priced per KG but managed in Pieces.
- **Formula**: $Weight = Qty (Pcs) \times WeightFactor$.
- **Implementation**: The variant lookup holds the `UnitPcsToKg` conversion factor. 
- **Override Handling**: When weighbridge scales differ (due to environmental scaling or scale calibrations), an override boolean allows the driver/loader to input the scale weight directly, logging the weight discrepancy as a metadata field for analytics.

### 2. Cutting & Scrap Calculations
- **Cutting Loss**: Handled by writing off small fraction differences to a specialized stock transaction: `stock_ledger.transaction_type = 'CUTTING_LOSS'`.
- **Scrap Handling**: If a product variant is sheared, the residual steel is booked as scrap inventory (`godown_id = SCRAP_GODOWN`) so that materials are not unaccounted for.
