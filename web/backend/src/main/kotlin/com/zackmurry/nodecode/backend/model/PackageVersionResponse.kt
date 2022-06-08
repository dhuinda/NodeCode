package com.zackmurry.nodecode.backend.model

/*
CREATE TABLE IF NOT EXISTS package_version
(
    package_name VARCHAR(32) NOT NULL REFERENCES package ON DELETE CASCADE,
    version VARCHAR(16) NOT NULL DEFAULT '0.1.0',
    time_published TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
 */

data class PackageVersionResponse(
    var version: String,
    var timePublished: String
)
