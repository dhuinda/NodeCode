package com.zackmurry.nodecode.backend.controller

import com.zackmurry.nodecode.backend.exception.NotFoundException
import com.zackmurry.nodecode.backend.model.UserPrincipalResponse
import com.zackmurry.nodecode.backend.security.UserPrincipal
import com.zackmurry.nodecode.backend.service.UserService
import net.rossillo.spring.web.mvc.CacheControl
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestMapping
import org.springframework.web.bind.annotation.RestController

@RestController
@RequestMapping("/api/v1/users")
class UserController(private val userService: UserService) {

    // test method for checking auth status
    @GetMapping("/username")
    fun getUsername(): String {
        return (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).name
    }

    @GetMapping("/avatar_url")
    fun getAvatarUrl(): String {
        return (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getAvatarUrl()
    }

    @GetMapping("/user")
    fun getAccount(): UserPrincipalResponse {
        return (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).toResponse()
    }

    @CacheControl(maxAge = 60 * 60)
    @GetMapping("/name/{name}/avatar_url")
    fun getAvatarUrlByUser(@PathVariable name: String): String {
        return userService.getUserByUsername(name)?.avatarUrl ?: throw NotFoundException()
    }

}