-- Live DB migration (Test123): PascalCase tables used by EF Core
-- Run: psql -h localhost -U postgres -d Test123 -f this file

-- 001: profile picture + user/customer mapping
ALTER TABLE "Users"
    ADD COLUMN IF NOT EXISTS "ProfilePictureUrl" character varying(500) NULL;

CREATE TABLE IF NOT EXISTS "UserCustomerMappings" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "CustomerId" integer NOT NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL,
    CONSTRAINT "FK_UserCustomerMappings_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserCustomerMappings_Customers" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_UserCustomerMappings_UserId" ON "UserCustomerMappings" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_UserCustomerMappings_CustomerId" ON "UserCustomerMappings" ("CustomerId");

-- 002: customer first/last name + default payment mode
ALTER TABLE "Customers"
    ADD COLUMN IF NOT EXISTS "FirstName" character varying(100),
    ADD COLUMN IF NOT EXISTS "LastName" character varying(100) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS "DefaultPaymentMode" character varying(20) NOT NULL DEFAULT 'A/C';

DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_schema = 'public'
          AND table_name = 'Customers'
          AND column_name = 'Name'
    ) THEN
        UPDATE "Customers"
        SET "FirstName" = COALESCE(NULLIF(TRIM("FirstName"), ''), NULLIF(TRIM("Name"), ''), 'Unknown'),
            "LastName" = COALESCE(NULLIF(TRIM("LastName"), ''), '');

        ALTER TABLE "Customers" DROP COLUMN "Name";
    END IF;
END $$;

UPDATE "Customers"
SET "FirstName" = COALESCE(NULLIF(TRIM("FirstName"), ''), 'Unknown')
WHERE "FirstName" IS NULL OR TRIM("FirstName") = '';

ALTER TABLE "Customers"
    ALTER COLUMN "FirstName" SET NOT NULL;
