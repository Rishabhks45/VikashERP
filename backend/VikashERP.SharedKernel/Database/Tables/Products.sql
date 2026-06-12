-- Table: Products
-- Description: Product master records

CREATE TABLE "Products" (
    "Id" SERIAL PRIMARY KEY,
    "CategoryId" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "HsnCode" character varying(10) NULL,
    "ProductImageUrl" character varying(500) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Products_Categories" FOREIGN KEY ("CategoryId") REFERENCES categories (id) ON DELETE RESTRICT
);
