package com.zackmurry.nodecode.backend.entity

import com.zackmurry.nodecode.backend.exception.InternalServerException
import com.zackmurry.nodecode.backend.model.MoveResponse
import org.slf4j.Logger
import org.slf4j.LoggerFactory
import java.util.*
import javax.persistence.Entity
import javax.persistence.Id

val logger: Logger = LoggerFactory.getLogger(Move::class.java)

@Entity
data class Move(
    var fenBefore: String? = null,
    var san: String? = null,
    var uci: String? = null,
    var isWhite: Boolean? = null,
    @Id var id: UUID? = null,
    var userId: UUID? = null,
    var lastReviewed: Long? = null,
    var timeCreated: Long? = null,
    var numReviews: Int? = null,
    var due: Long? = null,
    var opening: String? = null,
    var cleanFen: String? = null
) {
    fun toResponse(): MoveResponse {
        if (fenBefore == null || san == null || isWhite == null || id == null || userId == null || lastReviewed == null || timeCreated == null || numReviews == null || due == null || opening == null || cleanFen == null) {
            logger.warn("Found null in Move")
            throw InternalServerException()
        }
        return MoveResponse(
            fenBefore!!,
            san!!,
            uci!!,
            isWhite!!,
            id!!,
            userId!!,
            lastReviewed!!,
            timeCreated!!,
            numReviews!!,
            due!!,
            opening!!,
            cleanFen!!
        )
    }
}
