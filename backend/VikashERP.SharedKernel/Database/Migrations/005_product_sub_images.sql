-- Live DB migration (Test123): multiple product sub-images with description

CREATE TABLE IF NOT EXISTS "ProductSubImages" (
    "Id" SERIAL PRIMARY KEY,
    "ProductId" integer NOT NULL,
    "ImageUrl" character varying(500) NOT NULL,
    "Description" character varying(1000) NULL,
    "DisplayOrder" integer NOT NULL DEFAULT 0,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_ProductSubImages_Products" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_ProductSubImages_ProductId_DisplayOrder"
    ON "ProductSubImages" ("ProductId", "DisplayOrder");

-- Move any legacy single sub-image into the new table
INSERT INTO "ProductSubImages" ("ProductId", "ImageUrl", "Description", "DisplayOrder")
SELECT "Id", "SubImageUrl", NULL, 0
FROM "Products"
WHERE "SubImageUrl" IS NOT NULL
  AND TRIM("SubImageUrl") <> ''
  AND NOT EXISTS (
      SELECT 1 FROM "ProductSubImages" psi WHERE psi."ProductId" = "Products"."Id"
  );

ALTER TABLE "Products" DROP COLUMN IF EXISTS "SubImageUrl";
