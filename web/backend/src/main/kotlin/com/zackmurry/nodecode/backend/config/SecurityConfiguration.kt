package com.zackmurry.nodecode.backend.config

import com.zackmurry.nodecode.backend.security.HttpCookieOAuth2RequestRepository
import com.zackmurry.nodecode.backend.security.OAuth2AuthenticationFailureHandler
import com.zackmurry.nodecode.backend.security.OAuth2AuthenticationSuccessHandler
import com.zackmurry.nodecode.backend.service.OAuth2UserService
import org.springframework.context.annotation.Bean
import org.springframework.security.config.annotation.web.builders.HttpSecurity
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter
import org.springframework.security.config.web.servlet.invoke
import org.springframework.web.cors.CorsConfiguration
import org.springframework.web.cors.CorsConfigurationSource
import org.springframework.web.cors.UrlBasedCorsConfigurationSource


@EnableWebSecurity
class SecurityConfiguration(
    private val oAuth2AuthenticationSuccessHandler: OAuth2AuthenticationSuccessHandler,
    private val oAuth2AuthenticationFailureHandler: OAuth2AuthenticationFailureHandler,
    private val oAuth2UserService: OAuth2UserService,
    private val httpCookieOAuth2RequestRepository: HttpCookieOAuth2RequestRepository
) : WebSecurityConfigurerAdapter() {

    override fun configure(http: HttpSecurity?) {
        http {
            csrf {
                disable()
            }
            authorizeRequests {
                authorize("/", permitAll)
                authorize("/api/v1/users/name/*/avatar_url", permitAll)
                authorize(anyRequest, authenticated)
            }
            oauth2Login {
                redirectionEndpoint {
                    baseUri = "/api/v1/oauth2/callback/*"
                }
                authenticationSuccessHandler = oAuth2AuthenticationSuccessHandler
                authenticationFailureHandler = oAuth2AuthenticationFailureHandler
                userInfoEndpoint {
                    userService = oAuth2UserService
                }
                authorizationEndpoint {
                    authorizationRequestRepository = httpCookieOAuth2RequestRepository
                }
            }
            cors {
                configurationSource = corsConfigurationSource()
            }
        }
    }

    @Bean
    fun corsConfigurationSource(): CorsConfigurationSource {
        val source = UrlBasedCorsConfigurationSource()
        source.registerCorsConfiguration("/**", CorsConfiguration().applyPermitDefaultValues())
        return source
    }

}