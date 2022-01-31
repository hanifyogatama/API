﻿using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly IConfiguration _configuration;

        private readonly AccountRepository accountRepository;

        public AccountsController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
        }

        [HttpPost("Login")]
        public ActionResult<LoginVM> Post(LoginVM loginVM)
        {
            var login = accountRepository.Login(loginVM.Email, loginVM.Password);
            if(login != 0)
            {
                if(login == 1)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "account not registered" });
                }
                else if(login == 2)
                {
                    // var getProfile = accountRepository.GetProfile(loginVM.Email);
                    

                    // get role from query email and role
                    var getUserName = accountRepository.GetUserRole(loginVM.Email, loginVM.RoleName);


                    var data = new LoginDataVM()
                    {
                        Email = getUserName,
                        Role = getUserName,
                    };

                    var claims = new List<Claim>
                    {
                        new Claim("email", data.Email),
                        new Claim("role", data.Role)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // header
                    var token = new JwtSecurityToken(
                              _configuration["Jwt:Issuer"],
                              _configuration["Jwt:Audience"],
                              claims,
                              expires: DateTime.UtcNow.AddMinutes(10), // set time expired
                              signingCredentials: signIn );

                    var idToken = new JwtSecurityTokenHandler().WriteToken(token); // generate token
                    claims.Add(new Claim("TokenSecurity", idToken.ToString()));


                    return StatusCode(200, new { status = HttpStatusCode.OK, idToken, message = "login succesfully" });
                }
                else if (login == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong password" });
                }
                else if (login == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong email" });
                }
                else
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "login is failed" });
                }
            }
            else
            {
                return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "not found" });
            }
        }

        [HttpPost("ForgetPassword")]
        public ActionResult<ForgetPasswordVM> Post(ForgetPasswordVM forgetPasswordMV)
        {
            var forgotPassword = accountRepository.ForgotPassword(forgetPasswordMV.Email);
            if (forgotPassword != 0)
            {
                if (forgotPassword == 2)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "email sent successfully" });
                }
                else if (forgotPassword == 3)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email cannot send" });
                }
                else if (forgotPassword == 4)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "email not registered" });
                }
                else
                {
                    return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "not found" });
                }
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error" });
            }
        }

        [HttpPost("ChangePassword")]
        public ActionResult<ChangePasswordVM> Post(ChangePasswordVM changePasswordMV)
        {
            var changePassword = accountRepository.ChangePassword(changePasswordMV.Email, 
                changePasswordMV.OTP, changePasswordMV.NewPassword, changePasswordMV.ConfirmPassword);
            if (changePassword != 0)
            {
                if (changePassword == 5)
                {
                    return StatusCode(200, new { status = HttpStatusCode.OK, message = "password has been changed" });
                }
                else if (changePassword == 6)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "otp is used" });
                }
                else if (changePassword == 7)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "wrong confirm password" });
                }
                else if (changePassword == 8)
                {
                    return StatusCode(400, new { status = HttpStatusCode.NotFound, message = "otp is expired" });
                }
                else if (changePassword == 9)
                {
                    return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "otp is wrong" });
                }
                else
                {
                    return StatusCode(505, new { status = HttpStatusCode.NotFound, message = "change password is failed" });
                }
            }
            else
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = "internal server error"});
            }
        }

        // add test for authorize 
        [Authorize]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return Ok("test JWT Success");
        }

        // [Route("SignManager/{key}")]
        // public ActionResult SignManager()
        
        
    }
}
