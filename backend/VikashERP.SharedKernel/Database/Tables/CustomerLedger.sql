-- Table: customer_ledger
-- Description: Financial ledger tracking debits/credits for customers

CREATE TABLE customer_ledger (
    id SERIAL PRIMARY KEY,
    customer_id integer NOT NULL,
    transaction_date timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    transaction_type character varying(50) NOT NULL,
    reference_id integer NULL,
    debit numeric(12,2) NOT NULL DEFAULT 0,
    credit numeric(12,2) NOT NULL DEFAULT 0,
    running_balance numeric(12,2) NOT NULL DEFAULT 0,
    remarks text NULL,
    CONSTRAINT "FK_customer_ledger_Customers" FOREIGN KEY (customer_id) REFERENCES "Customers" ("Id") ON DELETE CASCADE
);
