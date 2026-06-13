-- Table: Customers
-- Description: Customer master records (UUID primary key)

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE "Customers" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "AccountNumber" character varying(20) NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "CompanyName" character varying(255) NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(255) NULL,
    "Gstin" character varying(15) NULL,
    "Address" text NULL,
    "DefaultPaymentMode" character varying(20) NOT NULL,
    "CreditLimit" numeric(12,2) NOT NULL DEFAULT 0,
    "CurrentBalance" numeric(12,2) NOT NULL DEFAULT 0,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX "IX_Customers_AccountNumber" ON "Customers" ("AccountNumber");
