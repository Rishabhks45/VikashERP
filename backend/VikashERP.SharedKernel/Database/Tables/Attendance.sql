CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: attendance
-- Description: Daily attendance records for staff

CREATE TABLE attendance (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    staff_id UUID NOT NULL,
    work_date date NOT NULL DEFAULT CURRENT_DATE,
    status character varying(20) NOT NULL,
    check_in time NULL,
    check_out time NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_attendance_staff" FOREIGN KEY (staff_id) REFERENCES staff (id) ON DELETE CASCADE,
    created_by UUID NULL,
    updated_at timestamp with time zone NULL,
    updated_by UUID NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    is_deleted boolean NOT NULL DEFAULT FALSE
);

CREATE UNIQUE INDEX idx_attendance_staff_date ON attendance (staff_id, work_date);
