-- Migration: customer first/last name + default payment mode (Cash / A/C)
-- Run against an existing VikashERP database (PostgreSQL).

ALTER TABLE customers
    ADD COLUMN IF NOT EXISTS first_name VARCHAR(100),
    ADD COLUMN IF NOT EXISTS last_name VARCHAR(100) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS default_payment_mode VARCHAR(20) NOT NULL DEFAULT 'A/C';

DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'public'
          AND table_name = 'customers'
          AND column_name = 'name'
    ) THEN
        UPDATE customers
        SET first_name = COALESCE(NULLIF(TRIM(first_name), ''), NULLIF(TRIM(name), ''), 'Unknown'),
            last_name = COALESCE(NULLIF(TRIM(last_name), ''), '');

        ALTER TABLE customers DROP COLUMN name;
    END IF;
END $$;

UPDATE customers
SET first_name = COALESCE(NULLIF(TRIM(first_name), ''), 'Unknown')
WHERE first_name IS NULL OR TRIM(first_name) = '';

ALTER TABLE customers
    ALTER COLUMN first_name SET NOT NULL;
