package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.NodecodeUser
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.data.jpa.repository.Modifying
import org.springframework.data.jpa.repository.Query
import org.springframework.data.repository.query.Param
import org.springframework.transaction.annotation.Transactional
import java.util.*

@Transactional
interface UserDao : JpaRepository<NodecodeUser, UUID> {

    fun findByUsername(username: String): Optional<NodecodeUser>

    fun existsByUsername(username: String): Boolean

    fun deleteByUsername(username: String)

    @Modifying
    @Query("UPDATE chessrs_user SET ease_factor = :easeFactor WHERE id = :id", nativeQuery = true)
    fun updateEaseFactor(@Param("id") id: UUID, @Param("easeFactor") easeFactor: Float)

    @Modifying
    @Query("UPDATE chessrs_user SET scaling_factor = :scalingFactor WHERE id = :id", nativeQuery = true)
    fun updateScalingFactor(@Param("id") id: UUID, @Param("scalingFactor") scalingFactor: Float)

}
