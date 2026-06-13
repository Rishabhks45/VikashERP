-- Migrate integer primary keys to UUID (development / Test123).
-- BACKUP the database before running.
-- psql -U postgres -d Test123 -f database/010_migrate_ids_to_uuid.sql

CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ========== Customers ==========
ALTER TABLE "Customers" ADD COLUMN IF NOT EXISTS "Id_new" UUID;
UPDATE "Customers" SET "Id_new" = gen_random_uuid() WHERE "Id_new" IS NULL;

-- ========== UserCustomerMappings ==========
ALTER TABLE "UserCustomerMappings" ADD COLUMN IF NOT EXISTS "Id_new" UUID;
ALTER TABLE "UserCustomerMappings" ADD COLUMN IF NOT EXISTS customer_id_new UUID;

UPDATE "UserCustomerMappings" m
SET "Id_new" = gen_random_uuid(),
    customer_id_new = c."Id_new"
FROM "Customers" c
WHERE m.customer_id = c."Id";

ALTER TABLE "UserCustomerMappings" DROP CONSTRAINT IF EXISTS "FK_UserCustomerMappings_Customers";
ALTER TABLE "UserCustomerMappings" DROP COLUMN IF EXISTS customer_id;
ALTER TABLE "UserCustomerMappings" RENAME COLUMN customer_id_new TO customer_id;

ALTER TABLE "Customers" DROP CONSTRAINT IF EXISTS "Customers_pkey" CASCADE;
ALTER TABLE "Customers" DROP COLUMN IF EXISTS "Id";
ALTER TABLE "Customers" RENAME COLUMN "Id_new" TO "Id";
ALTER TABLE "Customers" ADD PRIMARY KEY ("Id");
ALTER TABLE "Customers" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

ALTER TABLE "UserCustomerMappings" DROP CONSTRAINT IF EXISTS "UserCustomerMappings_pkey";
ALTER TABLE "UserCustomerMappings" DROP COLUMN IF EXISTS "Id";
ALTER TABLE "UserCustomerMappings" RENAME COLUMN "Id_new" TO "Id";
ALTER TABLE "UserCustomerMappings" ADD PRIMARY KEY ("Id");
ALTER TABLE "UserCustomerMappings" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();
ALTER TABLE "UserCustomerMappings"
    ADD CONSTRAINT "FK_UserCustomerMappings_Customers"
    FOREIGN KEY (customer_id) REFERENCES "Customers" ("Id") ON DELETE CASCADE;

-- ========== email_templates ==========
ALTER TABLE email_templates ADD COLUMN IF NOT EXISTS id_new UUID;
UPDATE email_templates SET id_new = gen_random_uuid() WHERE id_new IS NULL;
ALTER TABLE email_templates DROP CONSTRAINT IF EXISTS email_templates_pkey;
ALTER TABLE email_templates DROP COLUMN IF EXISTS id;
ALTER TABLE email_templates RENAME COLUMN id_new TO id;
ALTER TABLE email_templates ADD PRIMARY KEY (id);
ALTER TABLE email_templates ALTER COLUMN id SET DEFAULT gen_random_uuid();

-- ========== Organizations (singleton) ==========
ALTER TABLE "Organizations" ADD COLUMN IF NOT EXISTS "Id_new" UUID;
UPDATE "Organizations" SET "Id_new" = '00000000-0000-0000-0000-000000000001'::uuid WHERE "Id_new" IS NULL;
ALTER TABLE "Organizations" DROP CONSTRAINT IF EXISTS "Organizations_pkey";
ALTER TABLE "Organizations" DROP COLUMN IF EXISTS "Id";
ALTER TABLE "Organizations" RENAME COLUMN "Id_new" TO "Id";
ALTER TABLE "Organizations" ADD PRIMARY KEY ("Id");
ALTER TABLE "Organizations" ALTER COLUMN "Id" SET DEFAULT '00000000-0000-0000-0000-000000000001'::uuid;

-- ========== Child tables with no rows (safe column swap) ==========
DO $$
DECLARE
    row_count bigint;
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'invoices') THEN
        SELECT COUNT(*) INTO row_count FROM invoices;
        IF row_count = 0 THEN
            ALTER TABLE invoices DROP CONSTRAINT IF EXISTS "FK_invoices_Customers";
            ALTER TABLE invoices DROP COLUMN IF EXISTS id;
            ALTER TABLE invoices DROP COLUMN IF EXISTS customer_id;
            ALTER TABLE invoices ADD COLUMN id UUID PRIMARY KEY DEFAULT gen_random_uuid();
            ALTER TABLE invoices ADD COLUMN customer_id UUID NOT NULL REFERENCES "Customers"("Id") ON DELETE RESTRICT;
        END IF;
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'customer_ledger') THEN
        SELECT COUNT(*) INTO row_count FROM customer_ledger;
        IF row_count = 0 THEN
            ALTER TABLE customer_ledger DROP CONSTRAINT IF EXISTS "FK_customer_ledger_Customers";
            ALTER TABLE customer_ledger DROP COLUMN IF EXISTS id;
            ALTER TABLE customer_ledger DROP COLUMN IF EXISTS customer_id;
            ALTER TABLE customer_ledger DROP COLUMN IF EXISTS reference_id;
            ALTER TABLE customer_ledger ADD COLUMN id UUID PRIMARY KEY DEFAULT gen_random_uuid();
            ALTER TABLE customer_ledger ADD COLUMN customer_id UUID NOT NULL REFERENCES "Customers"("Id") ON DELETE CASCADE;
            ALTER TABLE customer_ledger ADD COLUMN reference_id UUID NULL;
        END IF;
    END IF;
END $$;
