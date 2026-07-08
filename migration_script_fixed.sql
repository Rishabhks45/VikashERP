START TRANSACTION;


















































































































































































CREATE TABLE "Holidays" (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" character varying(100) NOT NULL,
    "Date" date NOT NULL,
    "IsRecurring" boolean NOT NULL DEFAULT FALSE,
    "Description" character varying(500),
    created_at timestamp with time zone NOT NULL,
    created_by uuid,
    updated_at timestamp with time zone,
    updated_by uuid,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL,
    CONSTRAINT "PK_Holidays" PRIMARY KEY (id)
);

CREATE TABLE timezones (
    timezone_id uuid NOT NULL,
    iana_id text NOT NULL,
    display_name text NOT NULL,
    abbreviation text,
    is_default boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by uuid,
    updated_at timestamp with time zone,
    updated_by uuid,
    is_active boolean NOT NULL,
    is_deleted boolean NOT NULL,
    CONSTRAINT "PK_timezones" PRIMARY KEY (timezone_id)
);

CREATE UNIQUE INDEX "IX_timezones_iana_id" ON timezones (iana_id) WHERE "is_deleted" = false;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260626121926_AddTimezonesTable', '10.0.8');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260702210912_IncreaseProfilePictureUrlLength', '10.0.8');

COMMIT;

START TRANSACTION;
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260707161739_UpdateEntityDefaults', '10.0.8');

COMMIT;

