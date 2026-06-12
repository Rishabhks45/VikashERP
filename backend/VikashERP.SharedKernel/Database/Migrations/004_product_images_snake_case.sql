-- Greenfield / snake_case schema: product images
ALTER TABLE products
    ADD COLUMN IF NOT EXISTS product_image_url VARCHAR(500) NULL,
    ADD COLUMN IF NOT EXISTS sub_image_url VARCHAR(500) NULL;
