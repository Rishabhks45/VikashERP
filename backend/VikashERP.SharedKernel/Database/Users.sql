-- This file contains the schema definitions for the User tables

CREATE TABLE "Users" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "FirstName" character varying(255) NOT NULL,
    "LastName" character varying(255) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" character varying(50) NULL,
    "RefreshToken" text NULL,
    "RefreshTokenExpiry" timestamp with time zone NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE TABLE "PasswordResetTokens" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "Token" text NOT NULL,
    "ExpiresAtUtc" timestamp with time zone NOT NULL,
    "IsUsed" boolean NOT NULL DEFAULT FALSE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_PasswordResetTokens_Users" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);
