package com.zackmurry.nodecode.backend.controller

import com.zackmurry.nodecode.backend.model.PackageCreateRequest
import com.zackmurry.nodecode.backend.service.PackageService
import org.springframework.web.bind.annotation.*
import org.springframework.web.multipart.MultipartFile

@RequestMapping("/api/v1/packages")
@RestController
class PackageController(val packageService: PackageService) {

    @GetMapping("/{name}")
    fun getPackageDetails(@PathVariable name: String) = packageService.getPackageDetails(name)

    @PostMapping
    fun createPackage(@RequestBody request: PackageCreateRequest) = packageService.createPackage(request)

    @PostMapping("/{name}/versions/{version}")
    fun addVersionToPackage(@PathVariable name: String, @PathVariable version: String, @RequestParam("file") file: MultipartFile) {
        packageService.addVersionToPackage(name, version, file)
    }

}
