CREATE TABLE "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Username" VARCHAR(100) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(256) NOT NULL
);

CREATE EXTENSION IF NOT EXISTS "pgcrypto";
