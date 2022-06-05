CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE IF NOT EXISTS nodecode_user
(
    id             UUID        NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    username       VARCHAR(20) NOT NULL,
    provider       VARCHAR(32) NOT NULL
);

