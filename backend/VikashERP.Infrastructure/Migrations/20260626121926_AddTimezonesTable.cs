using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikashERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimezonesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerLedgers_Brokers_BrokerId1",
                table: "BrokerLedgers");

            migrationBuilder.DropIndex(
                name: "IX_BrokerLedgers_BrokerId1",
                table: "BrokerLedgers");

            migrationBuilder.DropColumn(
                name: "BrokerId1",
                table: "BrokerLedgers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Users",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserCustomerMappings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "UserCustomerMappings",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "UserCustomerMappings",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "UserCustomerMappings",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "UserCustomerMappings",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "UserCustomerMappings",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserCustomerMappings",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Suppliers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Suppliers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Suppliers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Suppliers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Suppliers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Suppliers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Suppliers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SupplierLedgers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "SupplierLedgers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "SupplierLedgers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "SupplierLedgers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "SupplierLedgers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "SupplierLedgers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SupplierLedgers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StockLedgers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "StockLedgers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "StockLedgers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "StockLedgers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "StockLedgers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "StockLedgers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "StockLedgers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StaffSalaries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "StaffSalaries",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "StaffSalaries",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "StaffSalaries",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "StaffSalaries",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "StaffSalaries",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "StaffSalaries",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Staff",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Staff",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Staff",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Staff",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Staff",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Staff",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Staff",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseEntryItems",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "PurchaseEntryItems",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PurchaseEntryItems",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PurchaseEntryItems",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "PurchaseEntryItems",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "PurchaseEntryItems",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseEntryItems",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseEntries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "PurchaseEntries",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PurchaseEntries",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PurchaseEntries",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "PurchaseEntries",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "PurchaseEntries",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseEntries",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductVariants",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ProductVariants",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ProductVariants",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ProductVariants",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ProductVariants",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ProductVariants",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProductVariants",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductSubImages",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ProductSubImages",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ProductSubImages",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ProductSubImages",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ProductSubImages",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ProductSubImages",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProductSubImages",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Products",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Products",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Products",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Products",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PasswordResetTokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "PasswordResetTokens",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "PasswordResetTokens",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PasswordResetTokens",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "PasswordResetTokens",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "PasswordResetTokens",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PasswordResetTokens",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Organizations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Organizations",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Organizations",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Organizations",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Organizations",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Organizations",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Organizations",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Invoices",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Invoices",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Invoices",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Invoices",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Invoices",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Invoices",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Invoices",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InvoiceItems",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "InvoiceItems",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "InvoiceItems",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "InvoiceItems",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "InvoiceItems",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "InvoiceItems",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "InvoiceItems",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Godowns",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Godowns",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Godowns",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Godowns",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Godowns",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Godowns",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Godowns",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Expenses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Expenses",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Expenses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Expenses",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Expenses",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Expenses",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Expenses",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "email_templates",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "email_templates",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "email_templates",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Deliveries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Deliveries",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Deliveries",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Deliveries",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Deliveries",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Deliveries",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Deliveries",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Customers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Customers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Customers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Customers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Customers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Customers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CustomerLedgers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "CustomerLedgers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "CustomerLedgers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "CustomerLedgers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CustomerLedgers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CustomerLedgers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CustomerLedgers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Categories",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Categories",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Categories",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Categories",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Categories",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Categories",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Brokers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Brokers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Brokers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Brokers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Brokers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Brokers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Brokers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BrokerLedgers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "BrokerLedgers",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "BrokerLedgers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "BrokerLedgers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "BrokerLedgers",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "BrokerLedgers",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BrokerLedgers",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attendances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Attendances",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Attendances",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Attendances",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Attendances",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Attendances",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Attendances",
                newName: "created_at");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "timezones",
                columns: table => new
                {
                    timezone_id = table.Column<Guid>(type: "uuid", nullable: false),
                    iana_id = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    abbreviation = table.Column<string>(type: "text", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timezones", x => x.timezone_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_timezones_iana_id",
                table: "timezones",
                column: "iana_id",
                unique: true,
                filter: "\"is_deleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "timezones");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Users",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Users",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserCustomerMappings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "UserCustomerMappings",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "UserCustomerMappings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "UserCustomerMappings",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "UserCustomerMappings",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "UserCustomerMappings",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "UserCustomerMappings",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Suppliers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Suppliers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Suppliers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Suppliers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Suppliers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Suppliers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Suppliers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SupplierLedgers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "SupplierLedgers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "SupplierLedgers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "SupplierLedgers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "SupplierLedgers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "SupplierLedgers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "SupplierLedgers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StockLedgers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "StockLedgers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "StockLedgers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StockLedgers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "StockLedgers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "StockLedgers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "StockLedgers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StaffSalaries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "StaffSalaries",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "StaffSalaries",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StaffSalaries",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "StaffSalaries",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "StaffSalaries",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "StaffSalaries",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Staff",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Staff",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Staff",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Staff",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Staff",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Staff",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Staff",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PurchaseEntryItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "PurchaseEntryItems",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PurchaseEntryItems",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "PurchaseEntryItems",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "PurchaseEntryItems",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "PurchaseEntryItems",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PurchaseEntryItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PurchaseEntries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "PurchaseEntries",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PurchaseEntries",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "PurchaseEntries",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "PurchaseEntries",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "PurchaseEntries",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PurchaseEntries",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ProductVariants",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "ProductVariants",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ProductVariants",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ProductVariants",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ProductVariants",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "ProductVariants",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ProductVariants",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ProductSubImages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "ProductSubImages",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ProductSubImages",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ProductSubImages",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ProductSubImages",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "ProductSubImages",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ProductSubImages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Products",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Products",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Products",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PasswordResetTokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "PasswordResetTokens",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PasswordResetTokens",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "PasswordResetTokens",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "PasswordResetTokens",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "PasswordResetTokens",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PasswordResetTokens",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Organizations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Organizations",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Organizations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Organizations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Organizations",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Organizations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Organizations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Invoices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Invoices",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Invoices",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Invoices",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Invoices",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Invoices",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Invoices",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "InvoiceItems",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "InvoiceItems",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "InvoiceItems",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "InvoiceItems",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "InvoiceItems",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "InvoiceItems",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "InvoiceItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Godowns",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Godowns",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Godowns",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Godowns",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Godowns",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Godowns",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Godowns",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Expenses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Expenses",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Expenses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Expenses",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Expenses",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Expenses",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Expenses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "email_templates",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "email_templates",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "email_templates",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Deliveries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Deliveries",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Deliveries",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Deliveries",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Deliveries",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Deliveries",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Deliveries",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Customers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Customers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Customers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Customers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Customers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Customers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Customers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CustomerLedgers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "CustomerLedgers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "CustomerLedgers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "CustomerLedgers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "CustomerLedgers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "CustomerLedgers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "CustomerLedgers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Categories",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Categories",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Categories",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Categories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Categories",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Categories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Brokers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Brokers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Brokers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Brokers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Brokers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Brokers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Brokers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BrokerLedgers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "BrokerLedgers",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "BrokerLedgers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "BrokerLedgers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "BrokerLedgers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "BrokerLedgers",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "BrokerLedgers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Attendances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Attendances",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Attendances",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Attendances",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Attendances",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "Attendances",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Attendances",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<Guid>(
                name: "BrokerId1",
                table: "BrokerLedgers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerLedgers_BrokerId1",
                table: "BrokerLedgers",
                column: "BrokerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerLedgers_Brokers_BrokerId1",
                table: "BrokerLedgers",
                column: "BrokerId1",
                principalTable: "Brokers",
                principalColumn: "Id");
        }
    }
}
