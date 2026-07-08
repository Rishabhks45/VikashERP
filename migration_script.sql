START TRANSACTION;
ALTER TABLE "Users" RENAME COLUMN "Id" TO id;

ALTER TABLE "Users" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Users" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Users" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Users" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Users" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Users" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "Id" TO id;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "UserCustomerMappings" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Suppliers" RENAME COLUMN "Id" TO id;

ALTER TABLE "Suppliers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Suppliers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Suppliers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Suppliers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Suppliers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Suppliers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "Id" TO id;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "SupplierLedgers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "StockLedgers" RENAME COLUMN "Id" TO id;

ALTER TABLE "StockLedgers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "StockLedgers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "StockLedgers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "StockLedgers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "StockLedgers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "StockLedgers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "StaffSalaries" RENAME COLUMN "Id" TO id;

ALTER TABLE "StaffSalaries" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "StaffSalaries" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "StaffSalaries" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "StaffSalaries" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "StaffSalaries" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "StaffSalaries" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Staff" RENAME COLUMN "Id" TO id;

ALTER TABLE "Staff" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Staff" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Staff" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Staff" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Staff" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Staff" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "Id" TO id;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "PurchaseEntryItems" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "Id" TO id;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "PurchaseEntries" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "ProductVariants" RENAME COLUMN "Id" TO id;

ALTER TABLE "ProductVariants" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "ProductVariants" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "ProductVariants" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "ProductVariants" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "ProductVariants" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "ProductVariants" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "ProductSubImages" RENAME COLUMN "Id" TO id;

ALTER TABLE "ProductSubImages" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "ProductSubImages" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "ProductSubImages" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "ProductSubImages" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "ProductSubImages" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "ProductSubImages" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Products" RENAME COLUMN "Id" TO id;

ALTER TABLE "Products" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Products" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Products" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Products" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Products" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Products" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "Id" TO id;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "PasswordResetTokens" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Organizations" RENAME COLUMN "Id" TO id;

ALTER TABLE "Organizations" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Organizations" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Organizations" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Organizations" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Organizations" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Organizations" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Invoices" RENAME COLUMN "Id" TO id;

ALTER TABLE "Invoices" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Invoices" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Invoices" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Invoices" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Invoices" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Invoices" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "InvoiceItems" RENAME COLUMN "Id" TO id;

ALTER TABLE "InvoiceItems" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "InvoiceItems" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "InvoiceItems" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "InvoiceItems" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "InvoiceItems" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "InvoiceItems" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Godowns" RENAME COLUMN "Id" TO id;

ALTER TABLE "Godowns" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Godowns" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Godowns" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Godowns" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Godowns" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Godowns" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Expenses" RENAME COLUMN "Id" TO id;

ALTER TABLE "Expenses" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Expenses" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Expenses" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Expenses" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Expenses" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Expenses" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE email_templates RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE email_templates RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE email_templates RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Deliveries" RENAME COLUMN "Id" TO id;

ALTER TABLE "Deliveries" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Deliveries" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Deliveries" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Deliveries" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Deliveries" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Deliveries" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Customers" RENAME COLUMN "Id" TO id;

ALTER TABLE "Customers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Customers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Customers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Customers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Customers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Customers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "Id" TO id;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "CustomerLedgers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Categories" RENAME COLUMN "Id" TO id;

ALTER TABLE "Categories" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Categories" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Categories" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Categories" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Categories" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Categories" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Brokers" RENAME COLUMN "Id" TO id;

ALTER TABLE "Brokers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Brokers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Brokers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Brokers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Brokers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Brokers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "Id" TO id;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "BrokerLedgers" RENAME COLUMN "CreatedAt" TO created_at;

ALTER TABLE "Attendances" RENAME COLUMN "Id" TO id;

ALTER TABLE "Attendances" RENAME COLUMN "UpdatedBy" TO updated_by;

ALTER TABLE "Attendances" RENAME COLUMN "UpdatedAt" TO updated_at;

ALTER TABLE "Attendances" RENAME COLUMN "IsDeleted" TO is_deleted;

ALTER TABLE "Attendances" RENAME COLUMN "IsActive" TO is_active;

ALTER TABLE "Attendances" RENAME COLUMN "CreatedBy" TO created_by;

ALTER TABLE "Attendances" RENAME COLUMN "CreatedAt" TO created_at;

CREATE TABLE "Holidays" (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" character varying(100) NOT NULL,
    "Date" date NOT NULL,
    "IsRecurring" boolean NOT NULL DEFAULT FALSE,
    "Description" character varying(500),
    created_at timestamp with time zone NOT NULL,
    created_by uuid,
    updated_at timestamp with time zone,
    updated_by uuid,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL,
    CONSTRAINT "PK_Holidays" PRIMARY KEY (id)
);

CREATE TABLE timezones (
    timezone_id uuid NOT NULL,
    iana_id text NOT NULL,
    display_name text NOT NULL,
    abbreviation text,
    is_default boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid,
    updated_at timestamp with time zone,
    updated_by uuid,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL,
    CONSTRAINT "PK_timezones" PRIMARY KEY (timezone_id)
);

CREATE UNIQUE INDEX "IX_timezones_iana_id" ON timezones (iana_id) WHERE "is_deleted" = false;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260626121926_AddTimezonesTable', '10.0.8');

COMMIT;

START TRANSACTION;
ALTER TABLE "Users" ALTER COLUMN "ProfilePictureUrl" TYPE text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260702210912_IncreaseProfilePictureUrlLength', '10.0.8');

COMMIT;

START TRANSACTION;
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260707161739_UpdateEntityDefaults', '10.0.8');

COMMIT;

