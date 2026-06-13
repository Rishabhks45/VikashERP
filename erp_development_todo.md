# Vikash Iron And Steel ERP - Development Roadmap & TODO

This document tracks the overall development progress of the ERP system based on the architectural blueprint.

## Phase 1: Foundation & Master Data
- [x] Configure Base Architecture & Clean Architecture Layers
- [x] Create Premium System Settings UI (`SystemSettings.razor`)
- [x] Create Material Categories UI (`Categories.razor`)
- [x] **Category Backend Integration**
  - Create `CategoriesController` and CQRS MediatR handlers
  - Connect UI to real PostgreSQL Database
- [ ] **Product Master Module**
  - Create Products UI (List, Add, Edit)
  - Implement Product Hierarchy (Product -> Variant -> Size -> Thickness)
  - Backend API for Products
- [ ] **Other Master Data**
  - Customer Master
  - Supplier Master
  - Unit Master

## Phase 2: Core Inventory Engine
- [ ] **Stock Management**
  - Purchase Entry & Stock Inward UI
  - Stock Outward & Transfer UI
- [ ] **Calculation Engine**
  - Implement Dual Unit tracking (Piece + KG conversion)
  - Handle cutting loss and scrap
- [ ] **Monitoring**
  - Live Stock Dashboard
  - Low Stock Notification System

## Phase 3: Sales, Billing & Accounts
- [ ] **Sales Flow**
  - Quotation Generation
  - Sales Order & Challan Generation
  - GST Invoice Generation
- [ ] **Ledger Architecture**
  - Customer Ledger implementation
  - Auto-due calculation based on payments
  - Advance / Partial payment entries
  - Cash & Expense ledgers

## Phase 4: Delivery & Operations
- [ ] **Delivery Management**
  - Delivery Challans
  - Driver & Vehicle tracking
  - Pending Delivery dashboard
- [ ] **Staff Management**
  - Attendance & Shift tracking
  - Salary calculations
  - Activity tracking logs

## Phase 5: Reporting & Future Enhancements
- [ ] Dashboard Analytics (Top products, Monthly sales charts)
- [ ] Daily Reports / End of Day summaries
- [ ] PDF & Excel Export capabilities
- [ ] .NET MAUI Mobile App setup
- [ ] WhatsApp Integration for Invoice/Due alerts
