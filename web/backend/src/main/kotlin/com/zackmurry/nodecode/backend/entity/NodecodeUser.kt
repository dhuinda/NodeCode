package com.zackmurry.nodecode.backend.entity

import java.util.*
import javax.persistence.Entity
import javax.persistence.Id

@Entity
data class NodecodeUser(
    var username: String? = null,
    @Id var id: UUID? = null,
    var provider: String? = null
)
