-- Table: categories
-- Description: Product category master

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name character varying(100) NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX idx_categories_name ON categories (name);
