-- This file contains the schema definitions for the User tables

CREATE TABLE "Users" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "FirstName" character varying(255) NOT NULL,
    "LastName" character varying(255) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "ProfilePictureUrl" character varying(500) NULL,
    "Role" character varying(50) NULL,
    "RefreshToken" text NULL,
    "RefreshTokenExpiry" timestamp with time zone NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE TABLE user_customer_mappings (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    customer_id INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NULL,
    CONSTRAINT "FK_user_customer_mappings_Users" FOREIGN KEY (user_id) REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_user_customer_mappings_customers" FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX idx_user_customer_mappings_user_id ON user_customer_mappings (user_id);
CREATE INDEX idx_user_customer_mappings_customer_id ON user_customer_mappings (customer_id);

CREATE TABLE "PasswordResetTokens" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Token" text NOT NULL,
    "ExpiresAtUtc" timestamp with time zone NOT NULL,
    "IsUsed" boolean NOT NULL DEFAULT FALSE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_PasswordResetTokens_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);
