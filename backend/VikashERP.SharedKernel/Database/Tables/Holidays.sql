-- Table: Holidays
-- Description: Holidays configuration for the calendar dashboard

CREATE TABLE "Holidays" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" character varying(100) NOT NULL,
    "Date" date NOT NULL,
    "IsRecurring" boolean NOT NULL DEFAULT FALSE,
    "Description" character varying(500) NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL,
    "CreatedBy" UUID NULL,
    "UpdatedBy" UUID NULL,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE
);
