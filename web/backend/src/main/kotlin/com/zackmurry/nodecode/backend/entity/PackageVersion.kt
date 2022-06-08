package com.zackmurry.nodecode.backend.entity

import java.sql.Timestamp

/*

CREATE TABLE IF NOT EXISTS package_version
(
    package_name VARCHAR(32) NOT NULL REFERENCES package ON DELETE CASCADE,
    version VARCHAR(16) NOT NULL DEFAULT '0.1.0',
    time_published TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

 */

data class PackageVersion(
    val id: String,
    val packageName: String,
    val version: String,
    val timePublished: Timestamp
)
