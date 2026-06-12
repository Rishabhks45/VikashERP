-- Apply Vikash Iron & Steel logo + brand colors to organization settings
UPDATE "Organizations"
SET
    "LegalName" = 'Vikash Iron & Steel',
    "DisplayName" = 'Vikash Iron & Steel',
    "Tagline" = 'Iron • Pipe • Plate • Sheet • GI Material',
    "LogoUrl" = '/logo.png',
    "FaviconUrl" = '/favicon.png',
    "PrimaryColor" = '#6366f1',
    "SecondaryColor" = '#0f172a',
    "UpdatedAt" = CURRENT_TIMESTAMP
WHERE "Id" = 1;
