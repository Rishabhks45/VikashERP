# Vikash Iron And Steel ERP Architecture

## Company
Vikash Iron And Steel

---

# Technology Stack

## Backend
- ASP.NET Core Web API

## Admin Panel
- Blazor Server / Blazor Web App

## Mobile App
- .NET MAUI

## Database
- PostgreSQL

---

# Core Modules

## 1. Customer Management
- Customer ledger
- Due tracking
- Payment history
- Credit management
- Order history
- Customer-wise reports

---

## 2. Inventory / Stock
- Live stock
- Multi-unit stock
- Godown stock
- Low stock alerts
- Purchase entry
- Stock movement tracking
- Stock adjustment

---

## 3. Billing / Sales
- GST invoice
- Challan
- Quotation
- Sale order
- Partial payment
- Advance payment
- Multiple payment modes
- Auto due calculation

---

## 4. Delivery Management
- Delivery status
- Driver details
- Vehicle tracking
- Pending delivery
- Delivery challan
- Transport management

---

## 5. Staff Management
- Attendance
- Salary
- Roles & permissions
- Activity tracking
- Shift timing

---

## 6. Reports & Analytics
- Daily sales
- Monthly analytics
- Profit report
- Stock reports
- Pending payments
- GST reports
- Customer ledger
- Top selling products

---

## 7. Online System
- Cloud database
- Real-time sync
- Multi-device support
- Mobile access
- Future customer portal

---

# Recommended Enterprise Architecture

```text
Client Layer
│
├── Blazor Admin Panel
├── MAUI Mobile App
├── Future Customer Portal
│
API Gateway
│
├── Authentication API
├── Sales API
├── Inventory API
├── Customer API
├── Payment API
├── Report API
├── Staff API
│
Business Layer
│
├── Domain Logic
├── Validation
├── Calculation Engine
├── Ledger Engine
│
Database Layer
│
├── PostgreSQL
├── Redis Cache
├── File Storage
```

---

# Ledger-Based Architecture

Every action should be stored in history.

Example:

```text
Customer bought material
→ ledger entry

Payment received
→ ledger entry

Stock reduced
→ stock transaction

Delivery completed
→ delivery log
```

Benefits:
- Accurate reports
- Audit tracking
- No data mismatch
- Complete history

---

# Inventory Engine

## Important Features
- Multiple units
- Weight calculations
- Piece + KG conversion
- Cutting loss handling
- Scrap handling
- Remaining stock tracking

---

# Product Structure

```text
Category
 └── Product
      └── Variant
           └── Size
                └── Thickness
                     └── Unit
```

Example:

```text
Pipe
 └── GI Pipe
      └── 1 inch
           └── 2mm
```

---

# Suggested Modules

## Authentication
- JWT Authentication
- Refresh Tokens
- Role-based permissions

---

## Master Module
- Product master
- Customer master
- Supplier master
- Unit master
- Vehicle master

---

## Inventory Module
- Purchase
- Stock inward
- Stock outward
- Transfer
- Adjustment
- Damage

---

## Sales Module
- Quotation
- Order
- Invoice
- Challan
- Return

---

## Accounts Module
- Customer ledger
- Cash ledger
- Expense ledger
- Payment entries

---

## Delivery Module
- Dispatch
- Route management
- Driver assignment
- Vehicle assignment

---

## Staff Module
- Attendance
- Salary
- Activity logs

---

## Reporting Module
- Dynamic reports
- Excel export
- PDF export
- Dashboard analytics

---

# Dashboard UI Structure

## Top Cards
- Today Sale
- Pending Amount
- Low Stock
- Delivery Pending

## Charts
- Monthly sales
- Customer dues
- Top products

## Live Activity
- Latest invoices
- Pending deliveries
- Recent payments

---

# Remaining Questions

## GST
- GST mandatory?
- E-invoice required?
- E-way bill required?

## Weight Calculation
- Auto weight calculation?
- Formula based?
- Manual override required?

## Barcode
- Barcode scanner?
- QR invoice?

## Notification System
- WhatsApp invoice
- Payment reminders
- Due alerts
- Stock alerts

## Purchase Module
- Supplier ledger
- Purchase due
- Purchase orders

## Multi-Godown
- Multiple godowns available?

## Mobile App Users
- Owner
- Staff
- Customer

## Current Software Problems
- Biggest issue?
- Slow workflow?
- Duplicate data?
- Time wasting operations?

---

# Future Scope

- AI analytics
- WhatsApp integration
- Barcode integration
- Multi-branch management
- Customer self-service portal
- Advanced reporting
- Offline sync support

