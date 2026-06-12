-- Table: staff_salaries
-- Description: Salary payment records for staff

CREATE TABLE staff_salaries (
    id SERIAL PRIMARY KEY,
    staff_id integer NOT NULL,
    payment_date date NOT NULL,
    amount_paid numeric(10,2) NOT NULL DEFAULT 0,
    payment_mode character varying(50) NOT NULL,
    remarks text NULL,
    CONSTRAINT "FK_staff_salaries_staff" FOREIGN KEY (staff_id) REFERENCES staff (id) ON DELETE CASCADE
);
