package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.Package
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.data.jpa.repository.Modifying
import org.springframework.data.jpa.repository.Query
import org.springframework.transaction.annotation.Transactional
import java.util.*

@Transactional
interface PackageDao : JpaRepository<Package, String> {

    fun findAllByAuthorIdOrderByLastUpdatedDesc(authorId: UUID): List<Package>

    @Modifying
    @Query("UPDATE package SET last_updated = :time WHERE name = :id", nativeQuery = true)
    fun updateLastUpdatedById(id: String, time: Long = System.currentTimeMillis())

    @Modifying
    @Query("UPDATE package SET latest_version = :latestVersion, last_updated = :time WHERE name = :id", nativeQuery = true)
    fun updateLatestVersionById(id: String, latestVersion: String, time: Long = System.currentTimeMillis())

    @Modifying
    @Query("UPDATE package SET downloads = downloads + 1 WHERE name = :id", nativeQuery = true)
    fun incrementDownloadsById(id: String): Any

}
