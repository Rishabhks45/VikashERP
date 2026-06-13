CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- Table: Organizations
-- Description: Single-tenant organization/company profile, branding, and configuration

CREATE TABLE "Organizations" (
    "Id" UUID PRIMARY KEY DEFAULT '00000000-0000-0000-0000-000000000001'::uuid,

    -- Identity & Branding
    "LegalName" character varying(255) NOT NULL,
    "DisplayName" character varying(255) NOT NULL,
    "Tagline" character varying(500) NULL,
    "LogoUrl" character varying(500) NULL,
    "FaviconUrl" character varying(500) NULL,
    "LoginBackgroundUrl" character varying(500) NULL,
    "PrimaryColor" character varying(20) NULL,
    "SecondaryColor" character varying(20) NULL,

    -- Contact & Address
    "AddressLine1" character varying(255) NULL,
    "AddressLine2" character varying(255) NULL,
    "City" character varying(100) NULL,
    "State" character varying(100) NULL,
    "PinCode" character varying(20) NULL,
    "Country" character varying(100) NOT NULL DEFAULT 'India',
    "Phone" character varying(30) NULL,
    "Email" character varying(255) NULL,
    "WebsiteUrl" character varying(500) NULL,
    "WhatsAppNumber" character varying(30) NULL,

    -- Tax & Banking
    "Gstin" character varying(15) NULL,
    "Pan" character varying(10) NULL,
    "BankName" character varying(255) NULL,
    "BankAccountName" character varying(255) NULL,
    "BankAccountNumber" character varying(50) NULL,
    "IfscCode" character varying(20) NULL,

    -- Email Identity
    "EmailFromName" character varying(255) NULL,
    "EmailFromAddress" character varying(255) NULL,

    -- SEO & Footer
    "MetaTitle" character varying(255) NULL,
    "MetaDescription" character varying(500) NULL,
    "MetaKeywords" character varying(500) NULL,
    "FooterText" character varying(1000) NULL,
    "CopyrightText" character varying(500) NULL,

    -- Social Links
    "SocialFacebookUrl" character varying(500) NULL,
    "SocialInstagramUrl" character varying(500) NULL,
    "SocialLinkedInUrl" character varying(500) NULL,
    "SocialYoutubeUrl" character varying(500) NULL,

    -- Regional Defaults
    "DefaultCurrency" character varying(10) NOT NULL DEFAULT 'INR',
    "DefaultWeightUnit" character varying(10) NOT NULL DEFAULT 'KG',
    "TimeZone" character varying(100) NOT NULL DEFAULT 'Asia/Kolkata',
    "DateFormat" character varying(30) NOT NULL DEFAULT 'dd-MM-yyyy',

    -- Feature Toggles
    "EnableCustomerPortal" boolean NOT NULL DEFAULT FALSE,
    "EnableLowStockAlerts" boolean NOT NULL DEFAULT TRUE,
    "EnablePaymentReminders" boolean NOT NULL DEFAULT TRUE,
    "EnableDailyReportEmail" boolean NOT NULL DEFAULT FALSE,
    "EnableTradeConfirmations" boolean NOT NULL DEFAULT TRUE,

    -- Meta
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "CreatedBy" UUID NULL,
    "UpdatedBy" UUID NULL,
    "IsDeleted" boolean NOT NULL DEFAULT FALSE
);
