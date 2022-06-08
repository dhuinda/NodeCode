package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.PackageDao
import com.zackmurry.nodecode.backend.entity.Package
import com.zackmurry.nodecode.backend.exception.BadRequestException
import com.zackmurry.nodecode.backend.exception.ConflictException
import com.zackmurry.nodecode.backend.exception.NotFoundException
import com.zackmurry.nodecode.backend.model.PackageCreateRequest
import com.zackmurry.nodecode.backend.model.PackageResponse
import com.zackmurry.nodecode.backend.security.UserPrincipal
import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Value
import org.springframework.data.repository.findByIdOrNull
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.stereotype.Service
import org.springframework.util.StringUtils
import org.springframework.web.multipart.MultipartFile
import java.io.File
import java.nio.file.Path
import java.nio.file.Paths
import java.sql.Timestamp
import java.time.Instant
import javax.annotation.PostConstruct

private val logger = LoggerFactory.getLogger(PackageService::class.java)
private val semVerRegex = Regex("[0-9]+\\.[0-9]+\\.[0-9]+")

@Service
class PackageService(private val packageDao: PackageDao,
                     private val userService: UserService,
                     private val packageVersionService: PackageVersionService,
                     @Value("\${app.upload.dir:\${user.home}/nodecode_uploads") private val uploadDirectory: String) {

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

    fun getPackageDetails(name: String): PackageResponse {
        val pkg = packageDao.findByIdOrNull(name) ?: throw NotFoundException()
        val author = userService.getUserResponse(pkg.authorId)
        val pkgVersions = packageVersionService.getPackageVersionsByPackage(name)
        return PackageResponse.from(pkg, author, pkgVersions)
    }

    fun createPackage(request: PackageCreateRequest) {
        if (request.name.length > 32 || request.description.length > 500 || (request.documentationUrl != null && request.documentationUrl!!.length > 5000)) {
            throw BadRequestException()
        }
        if (request.name.contains("_") || request.name.contains(" ")) {
            throw BadRequestException()
        }
        if (packageDao.findByIdOrNull(request.name) == null) {
            throw ConflictException()
        }
        val userId = (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        packageDao.save(Package(request.name, userId, request.description, Timestamp.from(Instant.now()), request.documentationUrl, request.repositoryUrl, 0))
    }

    fun resolvePathForPackage(name: String, version: String): Path {
        val fileName = "${name}_v${version}.nodecode"
        return Paths.get("${uploadDirectory}${File.separator}${StringUtils.cleanPath(fileName)}")
    }

    fun addVersionToPackage(name: String, version: String, file: MultipartFile) {
        if (file.originalFilename == null || !file.originalFilename!!.endsWith(".nodecode")) {
            throw BadRequestException()
        }
        if (!semVerRegex.matches(version)) {
            throw BadRequestException()
        }
        val userId = (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        // todo
    }

}