CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: Expenses
-- Description: Records daily operational overheads for Profit & Loss (P&L) tracking

CREATE TABLE "Expenses" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "ExpenseDate" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "Category" character varying(100) NOT NULL,
    "Amount" numeric(12,2) NOT NULL,
    "PaymentMode" character varying(50) NOT NULL,
    "Remarks" character varying(1000) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedBy" UUID NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedBy" UUID NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE
);

CREATE INDEX "IX_Expenses_ExpenseDate_Category" ON "Expenses" ("ExpenseDate", "Category");
