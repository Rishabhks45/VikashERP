CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: staff
-- Description: Staff/employee master records

CREATE TABLE staff (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    role character varying(50) NOT NULL,
    salary_per_month numeric(10,2) NOT NULL DEFAULT 0,
    phone character varying(20) NOT NULL,
    hire_date date NOT NULL DEFAULT CURRENT_DATE
);

CREATE UNIQUE INDEX idx_staff_phone ON staff (phone);
