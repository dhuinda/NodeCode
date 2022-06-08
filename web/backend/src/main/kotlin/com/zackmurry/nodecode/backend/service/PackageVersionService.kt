package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.PackageVersionDao
import com.zackmurry.nodecode.backend.model.PackageVersionResponse
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.stereotype.Service

@Service
class PackageVersionService(@Autowired val packageVersionDao: PackageVersionDao) {

    fun getPackageVersionsByPackage(packageName: String): List<PackageVersionResponse> {
        val pkgVersions = packageVersionDao.findAllByPackageNameOrderByTimePublishedDesc(packageName)
        return pkgVersions.map {
            PackageVersionResponse(it.version, it.timePublished.toString())
        }
    }

}