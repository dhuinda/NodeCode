package com.zackmurry.nodecode.backend.model

import com.zackmurry.nodecode.backend.entity.Move

data class NeedReviewResponse(val total: Int, val moves: List<Move>)
