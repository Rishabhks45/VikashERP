-- Table: Users
-- Description: Core user/authentication accounts table

CREATE TABLE "Users" (
    "Id" uuid NOT NULL PRIMARY KEY,
    "FirstName" character varying(255) NOT NULL,
    "LastName" character varying(255) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PasswordHash" text NOT NULL,
    "ProfilePictureUrl" character varying(500) NULL,
    "Role" character varying(50) NOT NULL,
    "RefreshToken" text NULL,
    "RefreshTokenExpiry" timestamp with time zone NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "LastLoginAt" timestamp with time zone NULL
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
