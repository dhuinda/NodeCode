package com.zackmurry.nodecode.backend.controller

import com.zackmurry.nodecode.backend.security.UserPrincipal
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/api/v1/users")
class UserController {

    // test method for checking auth status
    @GetMapping("/username")
    fun getUsername(): String {
        return (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).name
    }

}