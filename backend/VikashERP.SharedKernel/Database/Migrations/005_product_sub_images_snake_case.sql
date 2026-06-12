-- Greenfield / snake_case schema: multiple product sub-images

CREATE TABLE IF NOT EXISTS product_sub_images (
    id SERIAL PRIMARY KEY,
    product_id INT NOT NULL REFERENCES products(id) ON DELETE CASCADE,
    image_url VARCHAR(500) NOT NULL,
    description VARCHAR(1000),
    display_order INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_product_sub_images_product_id ON product_sub_images (product_id, display_order);

INSERT INTO product_sub_images (product_id, image_url, description, display_order)
SELECT id, sub_image_url, NULL, 0
FROM products
WHERE sub_image_url IS NOT NULL
  AND TRIM(sub_image_url) <> ''
  AND NOT EXISTS (
      SELECT 1 FROM product_sub_images psi WHERE psi.product_id = products.id
  );

ALTER TABLE products DROP COLUMN IF EXISTS sub_image_url;
