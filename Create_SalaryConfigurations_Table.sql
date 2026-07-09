CREATE TABLE IF NOT EXISTS "SalaryConfigurations" (
    id uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "BasicSalary" numeric NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid,
    updated_at timestamp with time zone,
    updated_by uuid,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL,
    CONSTRAINT "PK_SalaryConfigurations" PRIMARY KEY (id),
    CONSTRAINT "FK_SalaryConfigurations_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" (id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_SalaryConfigurations_UserId" ON "SalaryConfigurations" ("UserId");
