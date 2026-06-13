CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: Products
-- Description: Product master records

CREATE TABLE "Products" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "CategoryId" UUID NOT NULL,
    "Name" character varying(255) NOT NULL,
    "HsnCode" character varying(10) NULL,
    "ProductImageUrl" character varying(500) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Products_Categories" FOREIGN KEY ("CategoryId") REFERENCES categories (id) ON DELETE RESTRICT,
    "CreatedBy" UUID NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedBy" UUID NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE
);
