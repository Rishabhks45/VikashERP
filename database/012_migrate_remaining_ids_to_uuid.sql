-- Migrate remaining ERP tables from integer Id to UUID (empty tables only).
-- Run after 011_fix_uuid_migration.sql
-- psql -U postgres -d Test123 -f database/012_migrate_remaining_ids_to_uuid.sql

CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE OR REPLACE FUNCTION migrate_empty_table_pk_to_uuid(p_table text) RETURNS void AS $$
DECLARE
    row_count bigint;
BEGIN
    EXECUTE format('SELECT COUNT(*) FROM %I', p_table) INTO row_count;
    IF row_count > 0 THEN
        RAISE NOTICE 'Skipping % (% rows)', p_table, row_count;
        RETURN;
    END IF;

    EXECUTE format('ALTER TABLE %I DROP CONSTRAINT IF EXISTS %I', p_table, 'PK_' || p_table);
    EXECUTE format('ALTER TABLE %I ALTER COLUMN "Id" DROP IDENTITY IF EXISTS', p_table);
    EXECUTE format('ALTER TABLE %I DROP COLUMN IF EXISTS "Id"', p_table);
    EXECUTE format('ALTER TABLE %I ADD COLUMN "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid()', p_table);
    EXECUTE format('DROP SEQUENCE IF EXISTS %I', p_table || '_Id_seq');
    RAISE NOTICE 'Migrated % to UUID PK', p_table;
END;
$$ LANGUAGE plpgsql;

SELECT migrate_empty_table_pk_to_uuid('Categories');
SELECT migrate_empty_table_pk_to_uuid('Godowns');
SELECT migrate_empty_table_pk_to_uuid('Suppliers');
SELECT migrate_empty_table_pk_to_uuid('Staff');
SELECT migrate_empty_table_pk_to_uuid('Products');
SELECT migrate_empty_table_pk_to_uuid('ProductSubImages');
SELECT migrate_empty_table_pk_to_uuid('ProductVariants');
SELECT migrate_empty_table_pk_to_uuid('Invoices');
SELECT migrate_empty_table_pk_to_uuid('InvoiceItems');
SELECT migrate_empty_table_pk_to_uuid('CustomerLedgers');
SELECT migrate_empty_table_pk_to_uuid('StockLedgers');
SELECT migrate_empty_table_pk_to_uuid('SupplierLedgers');
SELECT migrate_empty_table_pk_to_uuid('Deliveries');
SELECT migrate_empty_table_pk_to_uuid('Attendances');
SELECT migrate_empty_table_pk_to_uuid('StaffSalaries');

DROP FUNCTION migrate_empty_table_pk_to_uuid(text);

-- Migrate integer FK columns on empty child tables
DO $$
DECLARE
    row_count bigint;
BEGIN
    SELECT COUNT(*) INTO row_count FROM "Products";
    IF row_count = 0 AND EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'Products' AND column_name = 'CategoryId' AND data_type = 'integer') THEN
        ALTER TABLE "Products" DROP COLUMN IF EXISTS "CategoryId";
        ALTER TABLE "Products" ADD COLUMN "CategoryId" UUID NOT NULL;
    END IF;

    SELECT COUNT(*) INTO row_count FROM "ProductVariants";
    IF row_count = 0 AND EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'ProductVariants' AND column_name = 'ProductId' AND data_type = 'integer') THEN
        ALTER TABLE "ProductVariants" DROP COLUMN IF EXISTS "ProductId";
        ALTER TABLE "ProductVariants" ADD COLUMN "ProductId" UUID NOT NULL;
    END IF;

    SELECT COUNT(*) INTO row_count FROM "ProductSubImages";
    IF row_count = 0 AND EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'ProductSubImages' AND column_name = 'ProductId' AND data_type = 'integer') THEN
        ALTER TABLE "ProductSubImages" DROP COLUMN IF EXISTS "ProductId";
        ALTER TABLE "ProductSubImages" ADD COLUMN "ProductId" UUID NOT NULL;
    END IF;

    SELECT COUNT(*) INTO row_count FROM "InvoiceItems";
    IF row_count = 0 THEN
        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'InvoiceItems' AND column_name = 'InvoiceId' AND data_type = 'integer') THEN
            ALTER TABLE "InvoiceItems" DROP COLUMN IF EXISTS "InvoiceId";
            ALTER TABLE "InvoiceItems" ADD COLUMN "InvoiceId" UUID NOT NULL;
        END IF;
        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'InvoiceItems' AND column_name = 'VariantId' AND data_type = 'integer') THEN
            ALTER TABLE "InvoiceItems" DROP COLUMN IF EXISTS "VariantId";
            ALTER TABLE "InvoiceItems" ADD COLUMN "VariantId" UUID NOT NULL;
        END IF;
    END IF;

    SELECT COUNT(*) INTO row_count FROM "CustomerLedgers";
    IF row_count = 0 AND EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'CustomerLedgers' AND column_name = 'ReferenceId' AND data_type = 'integer') THEN
        ALTER TABLE "CustomerLedgers" DROP COLUMN IF EXISTS "ReferenceId";
        ALTER TABLE "CustomerLedgers" ADD COLUMN "ReferenceId" UUID NULL;
    END IF;

    SELECT COUNT(*) INTO row_count FROM "StockLedgers";
    IF row_count = 0 THEN
        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'StockLedgers' AND column_name = 'VariantId' AND data_type = 'integer') THEN
            ALTER TABLE "StockLedgers" DROP COLUMN IF EXISTS "VariantId";
            ALTER TABLE "StockLedgers" ADD COLUMN "VariantId" UUID NOT NULL;
        END IF;
        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'StockLedgers' AND column_name = 'GodownId' AND data_type = 'integer') THEN
            ALTER TABLE "StockLedgers" DROP COLUMN IF EXISTS "GodownId";
            ALTER TABLE "StockLedgers" ADD COLUMN "GodownId" UUID NOT NULL;
        END IF;
        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'StockLedgers' AND column_name = 'ReferenceId' AND data_type = 'integer') THEN
            ALTER TABLE "StockLedgers" DROP COLUMN IF EXISTS "ReferenceId";
            ALTER TABLE "StockLedgers" ADD COLUMN "ReferenceId" UUID NULL;
        END IF;
    END IF;
END $$;
