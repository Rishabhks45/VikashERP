CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: deliveries
-- Description: Delivery/dispatch tracking for invoices

CREATE TABLE deliveries (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    invoice_id UUID NOT NULL,
    vehicle_number character varying(20) NOT NULL,
    driver_name character varying(100) NOT NULL,
    driver_phone character varying(20) NULL,
    delivery_status character varying(50) NOT NULL DEFAULT 'PENDING',
    delivery_challan_number character varying(100) NULL,
    loading_charge numeric(10,2) NOT NULL DEFAULT 0,
    freight_charge numeric(10,2) NOT NULL DEFAULT 0,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_deliveries_invoices" FOREIGN KEY (invoice_id) REFERENCES invoices (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX idx_deliveries_challan_number ON deliveries (delivery_challan_number);
