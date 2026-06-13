CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: staff_salaries
-- Description: Salary payment records for staff

CREATE TABLE staff_salaries (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    staff_id UUID NOT NULL,
    payment_date date NOT NULL,
    amount_paid numeric(10,2) NOT NULL DEFAULT 0,
    payment_mode character varying(50) NOT NULL,
    remarks text NULL,
    CONSTRAINT "FK_staff_salaries_staff" FOREIGN KEY (staff_id) REFERENCES staff (id) ON DELETE CASCADE,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);
