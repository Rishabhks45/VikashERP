-- Fix Identity Columns

-- Categories
ALTER TABLE "Categories" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Categories" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Categories" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Categories" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Products
ALTER TABLE "Products" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Products" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Products" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Products" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- ProductVariants
ALTER TABLE "ProductVariants" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "ProductVariants" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "ProductVariants" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "ProductVariants" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Godowns
ALTER TABLE "Godowns" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Godowns" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Godowns" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Godowns" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Invoices
ALTER TABLE "Invoices" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Invoices" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Invoices" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Invoices" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Staff
ALTER TABLE "Staff" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Staff" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Staff" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Staff" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Suppliers
ALTER TABLE "Suppliers" ALTER COLUMN "Id" DROP IDENTITY IF EXISTS;
ALTER TABLE "Suppliers" ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE "Suppliers" ALTER COLUMN "Id" TYPE uuid USING CAST(lpad(to_hex("Id"), 32, '0') AS uuid);
ALTER TABLE "Suppliers" ALTER COLUMN "Id" SET DEFAULT gen_random_uuid();

-- Now Re-add all Foreign Keys (since they were dropped but the ADD commands failed due to type mismatch earlier)
ALTER TABLE "Products" ADD CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT;
ALTER TABLE "ProductVariants" ADD CONSTRAINT "FK_ProductVariants_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE;
ALTER TABLE "ProductSubImages" ADD CONSTRAINT "FK_ProductSubImages_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE;
ALTER TABLE "Attendances" ADD CONSTRAINT "FK_Attendances_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES "Staff" ("Id") ON DELETE CASCADE;
ALTER TABLE "StaffSalaries" ADD CONSTRAINT "FK_StaffSalaries_Staff_StaffId" FOREIGN KEY ("StaffId") REFERENCES "Staff" ("Id") ON DELETE CASCADE;
ALTER TABLE "SupplierLedgers" ADD CONSTRAINT "FK_SupplierLedgers_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id") ON DELETE CASCADE;
ALTER TABLE "Deliveries" ADD CONSTRAINT "FK_Deliveries_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id") ON DELETE CASCADE;
ALTER TABLE "InvoiceItems" ADD CONSTRAINT "FK_InvoiceItems_Invoices_InvoiceId" FOREIGN KEY ("InvoiceId") REFERENCES "Invoices" ("Id") ON DELETE CASCADE;
