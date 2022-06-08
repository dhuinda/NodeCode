package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.PackageDao
import com.zackmurry.nodecode.backend.entity.Package
import com.zackmurry.nodecode.backend.exception.BadRequestException
import com.zackmurry.nodecode.backend.exception.ConflictException
import com.zackmurry.nodecode.backend.exception.NotFoundException
import com.zackmurry.nodecode.backend.model.PackageCreateRequest
import com.zackmurry.nodecode.backend.model.PackageResponse
import com.zackmurry.nodecode.backend.security.UserPrincipal
import org.springframework.data.repository.findByIdOrNull
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.stereotype.Service
import java.sql.Timestamp
import java.time.Instant


@Service
class PackageService(
    private val packageDao: PackageDao,
    private val userService: UserService,
    private val packageVersionService: PackageVersionService
) {

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
        packageDao.save(
            Package(
                request.name,
                userId,
                request.description,
                Timestamp.from(Instant.now()),
                request.documentationUrl,
                request.repositoryUrl,
                0
            )
        )
    }

    fun existsByName(name: String) = packageDao.existsById(name)

}