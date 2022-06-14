package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.PackageVersionDao
import com.zackmurry.nodecode.backend.entity.PackageVersion
import com.zackmurry.nodecode.backend.exception.BadRequestException
import com.zackmurry.nodecode.backend.exception.ConflictException
import com.zackmurry.nodecode.backend.exception.InternalServerException
import com.zackmurry.nodecode.backend.model.PackageVersionResponse
import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Value
import org.springframework.stereotype.Service
import org.springframework.util.StringUtils
import org.springframework.web.multipart.MultipartFile
import java.io.File
import java.io.IOException
import java.nio.file.Files
import java.nio.file.Path
import java.nio.file.Paths
import java.nio.file.StandardCopyOption
import java.sql.Timestamp
import java.time.Instant
import javax.annotation.PostConstruct

private val logger = LoggerFactory.getLogger(PackageVersionService::class.java)
private val SEM_VER_REGEX = Regex("[0-9]+\\.[0-9]+\\.[0-9]+")

@Service
class PackageVersionService(
    val packageVersionDao: PackageVersionDao,
    @Value("\${app.upload.dir:\${user.home}/nodecode_uploads}") private val uploadDirectory: String
) {

    @PostConstruct
    private fun init() {
        val f = File(uploadDirectory)
        if (!f.exists()) {
            logger.info("Creating uploads directory at {}", uploadDirectory)
            if (!f.mkdirs()) {
                logger.error("Failed to create uploads directory at {}", uploadDirectory)
            }
        }
    }

    fun getPackageVersionsByPackage(packageName: String): List<PackageVersionResponse> {
        val pkgVersions = packageVersionDao.findAllByPackageNameOrderByTimePublishedDesc(packageName)
        return pkgVersions.map {
            PackageVersionResponse(it.version, it.timePublished.toString())
        }
    }

    fun resolvePathForPackage(name: String, version: String): Path {
        val fileName = "${name}_v${version}.nodecode"
        return Paths.get("${uploadDirectory}${File.separator}${StringUtils.cleanPath(fileName)}")
    }

    fun addVersionToPackage(name: String, version: String, file: MultipartFile) {
        if (file.originalFilename == null || !file.originalFilename!!.endsWith(".nodecode")) {
            throw BadRequestException()
        }
        if (!SEM_VER_REGEX.matches(version)) {
            throw BadRequestException()
        }
        val pkgVerId = "${name}_v${version}"
        if (packageVersionDao.existsById(pkgVerId)) {
            throw ConflictException()
        }
        val packageVersion = PackageVersion(pkgVerId, name, version, Timestamp.from(Instant.now()))
        packageVersionDao.save(packageVersion)
        try {
            val copyLocation = resolvePathForPackage(name, version)
            if (copyLocation.toFile().exists()) {
                logger.error("Upload of {} failed: version is not found in database but exists in filesystem", pkgVerId)
                throw InternalServerException()
            }
            Files.copy(file.inputStream, copyLocation, StandardCopyOption.REPLACE_EXISTING)
        } catch (e: IOException) {
            logger.error("Exception occurred while uploading {}", pkgVerId, e)
        }
    }
}