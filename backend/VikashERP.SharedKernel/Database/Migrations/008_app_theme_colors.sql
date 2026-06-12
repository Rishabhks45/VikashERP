-- Lock app theme colors to the fixed indigo + slate palette
UPDATE "Organizations"
SET
    "PrimaryColor" = '#6366f1',
    "SecondaryColor" = '#0f172a',
    "UpdatedAt" = CURRENT_TIMESTAMP
WHERE "Id" = 1;
