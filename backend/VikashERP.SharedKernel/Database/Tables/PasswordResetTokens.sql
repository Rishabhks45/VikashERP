-- Table: PasswordResetTokens
-- Description: Stores password reset tokens for user accounts

CREATE TABLE "PasswordResetTokens" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Token" text NOT NULL,
    "ExpiresAtUtc" timestamp with time zone NOT NULL,
    "IsUsed" boolean NOT NULL DEFAULT FALSE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_PasswordResetTokens_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);
