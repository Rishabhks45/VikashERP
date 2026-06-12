-- Table: stock_ledger
-- Description: Inventory stock movement ledger

CREATE TABLE stock_ledger (
    id SERIAL PRIMARY KEY,
    variant_id integer NOT NULL,
    godown_id integer NOT NULL,
    transaction_type character varying(50) NOT NULL,
    reference_id integer NULL,
    qty_pcs integer NOT NULL DEFAULT 0,
    weight_kg numeric(12,3) NOT NULL DEFAULT 0,
    running_pcs integer NOT NULL DEFAULT 0,
    running_weight_kg numeric(12,3) NOT NULL DEFAULT 0,
    remarks text NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_stock_ledger_product_variants" FOREIGN KEY (variant_id) REFERENCES product_variants (id) ON DELETE RESTRICT,
    CONSTRAINT "FK_stock_ledger_godowns" FOREIGN KEY (godown_id) REFERENCES godowns (id) ON DELETE RESTRICT
);
