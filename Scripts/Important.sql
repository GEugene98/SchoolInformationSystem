ALTER TABLE "Tickets" DROP COLUMN "End";

ALTER TABLE "Tickets" DROP COLUMN "Start";

ALTER TABLE "Tickets" ADD "Important" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20190206212307_Important', '2.1.3-rtm-32065');

