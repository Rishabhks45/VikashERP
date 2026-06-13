CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: ProductSubImages
-- Description: Additional images for products

CREATE TABLE "ProductSubImages" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "ProductId" UUID NOT NULL,
    "ImageUrl" character varying(500) NOT NULL,
    "Description" character varying(1000) NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_ProductSubImages_Products" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE,
    "CreatedBy" UUID NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedBy" UUID NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE
);

CREATE INDEX "IX_ProductSubImages_ProductId_DisplayOrder" ON "ProductSubImages" ("ProductId", "DisplayOrder");
