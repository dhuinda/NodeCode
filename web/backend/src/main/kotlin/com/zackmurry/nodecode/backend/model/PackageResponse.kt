package com.zackmurry.nodecode.backend.model

import com.zackmurry.nodecode.backend.entity.Package

data class PackageResponse(
    var name: String,
    var author: UserResponse,
    var description: String,
    var lastUpdated: Long,
    var documentationUrl: String?,
    var repositoryUrl: String?,
    var downloads: Int,
    var versions: List<PackageVersionResponse> // sorted: most recent is index 0
) {
    companion object {
        fun from(pkg: Package, author: UserResponse, versions: List<PackageVersionResponse>): PackageResponse {
            return PackageResponse(pkg.name, author, pkg.description, pkg.lastUpdated.time, pkg.documentationUrl, pkg.repositoryUrl, pkg.downloads, versions)
        }
    }
}


