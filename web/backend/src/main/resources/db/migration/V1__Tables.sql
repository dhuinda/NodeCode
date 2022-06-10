CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS nodecode_user
(
    id         UUID        NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    username   VARCHAR(20) NOT NULL,
    provider   VARCHAR(32) NOT NULL,
    avatar_url TEXT        NOT NULL DEFAULT 'https://avatars.githubusercontent.com/u/54601764?v=4'
);

CREATE TABLE IF NOT EXISTS package
(
    name              VARCHAR(32)  NOT NULL PRIMARY KEY, -- ex: linked-list
    author_id         UUID         NOT NULL REFERENCES nodecode_user ON DELETE CASCADE,
    description       VARCHAR(500) NOT NULL DEFAULT '',
    last_updated      TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    documentation_url TEXT,
    repository_url    TEXT,
    downloads         INTEGER      NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS package_version
(
    id             VARCHAR(50) NOT NULL PRIMARY KEY, -- ex: linked-list_v0.1.0
    package_name   VARCHAR(32) NOT NULL REFERENCES package ON DELETE CASCADE,
    version        VARCHAR(16) NOT NULL DEFAULT '0.1.0',
    time_published TIMESTAMP   NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX package_version_package_name_index ON package_version (package_name);
