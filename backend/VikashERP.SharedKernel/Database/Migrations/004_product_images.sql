-- Live DB migration (Test123): product main + sub image URLs
ALTER TABLE "Products"
    ADD COLUMN IF NOT EXISTS "ProductImageUrl" character varying(500) NULL,
    ADD COLUMN IF NOT EXISTS "SubImageUrl" character varying(500) NULL;
