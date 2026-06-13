-- Drop existing unique indexes
DROP INDEX IF EXISTS "IX_Categories_Name";
DROP INDEX IF EXISTS "IX_Users_Email";
DROP INDEX IF EXISTS "IX_Godowns_Name";
DROP INDEX IF EXISTS "IX_Customers_AccountNumber";
DROP INDEX IF EXISTS "IX_Invoices_InvoiceNumber";
DROP INDEX IF EXISTS "IX_Deliveries_DeliveryChallanNumber";
DROP INDEX IF EXISTS "IX_Staff_Phone";
DROP INDEX IF EXISTS "IX_Attendances_StaffId_WorkDate";
DROP INDEX IF EXISTS "IX_UserCustomerMappings_UserId";
DROP INDEX IF EXISTS "IX_ProductVariants_ProductId_Size_Thickness";

-- Recreate them as partial indexes ignoring deleted rows
CREATE UNIQUE INDEX "IX_Categories_Name" ON "Categories" ("Name") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Godowns_Name" ON "Godowns" ("Name") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Customers_AccountNumber" ON "Customers" ("AccountNumber") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Invoices_InvoiceNumber" ON "Invoices" ("InvoiceNumber") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Deliveries_DeliveryChallanNumber" ON "Deliveries" ("DeliveryChallanNumber") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Staff_Phone" ON "Staff" ("Phone") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_Attendances_StaffId_WorkDate" ON "Attendances" ("StaffId", "WorkDate") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_UserCustomerMappings_UserId" ON "UserCustomerMappings" ("UserId") WHERE "IsDeleted" = false;
CREATE UNIQUE INDEX "IX_ProductVariants_ProductId_Size_Thickness" ON "ProductVariants" ("ProductId", "Size", "Thickness") WHERE "IsDeleted" = false;
