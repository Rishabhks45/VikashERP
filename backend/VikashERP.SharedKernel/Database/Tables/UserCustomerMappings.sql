CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: UserCustomerMappings
-- Description: Links Users to Customers (UUID primary keys)

CREATE TABLE "UserCustomerMappings" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL,
    customer_id UUID NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NULL,
    CONSTRAINT "FK_UserCustomerMappings_Users" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserCustomerMappings_Customers" FOREIGN KEY (customer_id) REFERENCES "Customers" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_UserCustomerMappings_UserId" ON "UserCustomerMappings" (user_id);
CREATE INDEX "IX_UserCustomerMappings_CustomerId" ON "UserCustomerMappings" (customer_id);
