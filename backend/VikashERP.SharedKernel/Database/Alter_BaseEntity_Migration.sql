-- Migration Script to add BaseEntity columns to all 21 existing tables
-- Safely uses ADD COLUMN IF NOT EXISTS to prevent errors if running multiple times.
-- Uses EXACT table names mapped by EF Core in ApplicationDbContext.

ALTER TABLE "Users"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Categories"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Attendances"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "CustomerLedgers"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Customers"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Deliveries"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "email_templates"
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Godowns"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "InvoiceItems"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Invoices"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Organizations"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "PasswordResetTokens"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "ProductSubImages"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "ProductVariants"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Products"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Staff"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "StaffSalaries"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "StockLedgers"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "SupplierLedgers"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "Suppliers"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE "UserCustomerMappings"
    ADD COLUMN IF NOT EXISTS "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ADD COLUMN IF NOT EXISTS "CreatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedAt" timestamp with time zone NULL,
    ADD COLUMN IF NOT EXISTS "UpdatedBy" uuid NULL,
    ADD COLUMN IF NOT EXISTS "IsActive" boolean NOT NULL DEFAULT TRUE,
    ADD COLUMN IF NOT EXISTS "IsDeleted" boolean NOT NULL DEFAULT FALSE;
