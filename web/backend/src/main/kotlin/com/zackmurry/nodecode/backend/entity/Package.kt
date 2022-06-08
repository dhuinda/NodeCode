package com.zackmurry.nodecode.backend.entity

import java.sql.Timestamp
import java.util.*
import javax.persistence.Entity
import javax.persistence.Id

/*

CREATE TABLE IF NOT EXISTS package
(
    name VARCHAR(32) NOT NULL PRIMARY KEY, -- ex: linked-list
    author_id VARCHAR(20) NOT NULL REFERENCES nodecode_user ON DELETE CASCADE,
    description VARCHAR(500) NOT NULL DEFAULT '',
    last_updated TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    documentation_url TEXT,
    repository_url TEXT,
    downloads INTEGER NOT NULL DEFAULT 0
);
 */
@Entity
data class Package(
    @Id var name: String,
    var authorId: UUID,
    var description: String,
    var lastUpdated: Timestamp,
    var documentationUrl: String?,
    var repositoryUrl: String?,
    var downloads: Int
)
