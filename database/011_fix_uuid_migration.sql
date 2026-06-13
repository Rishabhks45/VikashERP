-- Complete UUID migration after partial run of 010 (EF PascalCase schema).
-- psql -U postgres -d Test123 -f database/011_fix_uuid_migration.sql

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Ensure Customers.Id_new is populated
UPDATE "Customers" SET "Id_new" = gen_random_uuid() WHERE "Id_new" IS NULL;

-- Clean partial UserCustomerMappings columns from failed 010
ALTER TABLE "UserCustomerMappings" DROP COLUMN IF EXISTS customer_id;

-- Stage new FK UUID columns on child tables
ALTER TABLE "UserCustomerMappings" ADD COLUMN IF NOT EXISTS "CustomerId_new" UUID;
UPDATE "UserCustomerMappings" m
SET "CustomerId_new" = c."Id_new"
FROM "Customers" c
WHERE m."CustomerId" = c."Id";

ALTER TABLE "CustomerLedgers" ADD COLUMN IF NOT EXISTS "CustomerId_new" UUID;
UPDATE "CustomerLedgers" l
SET "CustomerId_new" = c."Id_new"
FROM "Customers" c
WHERE l."CustomerId" = c."Id";

ALTER TABLE "Invoices" ADD COLUMN IF NOT EXISTS "CustomerId_new" UUID;
UPDATE "Invoices" i
SET "CustomerId_new" = c."Id_new"
FROM "Customers" c
WHERE i."CustomerId" = c."Id";

-- Drop FK constraints referencing Customers.Id
ALTER TABLE "CustomerLedgers" DROP CONSTRAINT IF EXISTS "FK_CustomerLedgers_Customers_CustomerId";
ALTER TABLE "Invoices" DROP CONSTRAINT IF EXISTS "FK_Invoices_Customers_CustomerId";
ALTER TABLE "UserCustomerMappings" DROP CONSTRAINT IF EXISTS "FK_UserCustomerMappings_Customers_CustomerId";

-- Swap Customers primary key to UUID
ALTER TABLE "Customers" DROP CONSTRAINT IF EXISTS "PK_Customers";
ALTER TABLE "Customers" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Customers" RENAME COLUMN "Id" TO "Id_old";
ALTER TABLE "Customers" RENAME COLUMN "Id_new" TO "Id";
ALTER TABLE "Customers" ADD CONSTRAINT "PK_Customers" PRIMARY KEY ("Id");
ALTER TABLE "Customers" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();
ALTER TABLE "Customers" DROP COLUMN "Id_old";
DROP SEQUENCE IF EXISTS "Customers_Id_seq";

-- UserCustomerMappings: ensure Id is UUID PK
UPDATE "UserCustomerMappings" SET "Id" = gen_random_uuid() WHERE "Id" IS NULL;
ALTER TABLE "UserCustomerMappings" DROP COLUMN IF EXISTS "CustomerId";
ALTER TABLE "UserCustomerMappings" RENAME COLUMN "CustomerId_new" TO "CustomerId";
ALTER TABLE "UserCustomerMappings" DROP CONSTRAINT IF EXISTS "PK_UserCustomerMappings";
ALTER TABLE "UserCustomerMappings" ADD CONSTRAINT "PK_UserCustomerMappings" PRIMARY KEY ("Id");
ALTER TABLE "UserCustomerMappings" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();
ALTER TABLE "UserCustomerMappings"
    ADD CONSTRAINT "FK_UserCustomerMappings_Customers_CustomerId"
    FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE;

-- CustomerLedgers FK swap
ALTER TABLE "CustomerLedgers" DROP COLUMN IF EXISTS "CustomerId";
ALTER TABLE "CustomerLedgers" RENAME COLUMN "CustomerId_new" TO "CustomerId";
ALTER TABLE "CustomerLedgers"
    ADD CONSTRAINT "FK_CustomerLedgers_Customers_CustomerId"
    FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE;

-- Invoices FK swap
ALTER TABLE "Invoices" DROP COLUMN IF EXISTS "CustomerId";
ALTER TABLE "Invoices" RENAME COLUMN "CustomerId_new" TO "CustomerId";
ALTER TABLE "Invoices"
    ADD CONSTRAINT "FK_Invoices_Customers_CustomerId"
    FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE RESTRICT;
