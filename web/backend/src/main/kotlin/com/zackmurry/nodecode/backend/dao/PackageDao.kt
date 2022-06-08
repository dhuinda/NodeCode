package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.Package
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.transaction.annotation.Transactional

@Transactional
interface PackageDao : JpaRepository<Package, String> {

}
